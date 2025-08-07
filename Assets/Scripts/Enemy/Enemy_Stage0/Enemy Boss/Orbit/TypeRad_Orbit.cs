using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRad_Orbit : MonoBehaviour
{
    [Header("---Target & Pos---")]
    [SerializeField] private GameObject target;
    [SerializeField] private Transform sTarget;
    [SerializeField] private Transform eTarget;

    [Header("---Prefab---")]
    [SerializeField] private GameObject laser_VFX;
    [SerializeField] private GameObject laser_Collider;
    [SerializeField] private GameObject endVFX;

    [Header("---Attack Setting---")]
    [SerializeField] private float attackTime;
    [SerializeField] private float attackDelay;
    private Vector3 attackDir;
    private Enemy_Base enemy;

    [Header("---Sound---")]
    public Sound_TypeRed sound;

    #region Orbit_SpreadOut
    public void Orbit_SpreadOut(Enemy_Base enemy, GameObject player, Vector3 endPos, float moveTime)
    {
        this.enemy = enemy;
        target = player;
        StartCoroutine(SpreadOut_Move(endPos, moveTime));
    }

    private IEnumerator SpreadOut_Move(Vector3 endPos, float moveTime)
    {
        // 지정한 시간만큼, 지정한 위치로 Lerp 이동
        Vector3 startPos = transform.position;

        // Move
        float timer = 0;
        float speed = 1;
        while (timer < 1)
        {
            // Target Look
            attackDir = (target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(attackDir);
            transform.forward = attackDir;

            // Move
            //speed = (timer < 0.7f) ? (speed + 1.5f * Time.deltaTime) : speed;
            timer += speed * Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, InOutCirc(timer));
            yield return null;
        }

        // Delay -> 딜레이 기간동안 쳐다보게 할 것인지? => 하지 말자
        yield return new WaitForSeconds(0.25f);

        // Attack
        StartCoroutine(SpreadOut_Attack(3f));
    }

    private IEnumerator SpreadOut_Attack(float totalRotTime) // 최종 구현 버전 -> 위와 동일하게 179도 이상 회전 불가
    {
        // Look rotation Setting
        Vector3 end = sTarget.localPosition - transform.localPosition;
        Quaternion startLook = transform.localRotation;
        Quaternion endLook = Quaternion.LookRotation(end);

        // rotation
        float timer = 0.25f;
        while(timer > 0)
        {
            transform.rotation = Quaternion.Slerp(startLook, endLook, timer);
            timer -= Time.deltaTime;
            yield return null;
        }

        // Rotation Setting
        Vector3 lookDir = eTarget.position - transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        // Attack collider On
        laser_Collider.GetComponent<Enemy_AttackCollider>().enemy = this.enemy;
        laser_VFX.SetActive(true);

        timer = 0;
        float rotSpeed = 1f / totalRotTime; // 초기 회전 속도 (1초에 얼마나 회전할 것인가)
        while (timer < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, timer);
            timer += Time.deltaTime * rotSpeed;

            // Speed Up (진행도 30% 이상)
            rotSpeed = (timer > 0.4f) ? rotSpeed + Time.deltaTime * 3f : rotSpeed;

            // Speed Down (진행도 70 이상 + 속도 0.25 이상)
            rotSpeed = (timer > 0.75f) && (rotSpeed > 0.25f) ? rotSpeed - Time.deltaTime * 15f : rotSpeed;
            yield return null;
        }

        // Laser Off Delay
        yield return new WaitForSeconds(0.25f);
        laser_VFX.SetActive(false);

        // Delay
        yield return new WaitForSeconds(0.25f);

        // VFX
        Instantiate(endVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion

    #region Orbit_Focus
    public void Orbit_Focus(Enemy_Base enemy, GameObject target, Vector3 endPos, float moveTime)
    {
        this.enemy = enemy;
        StartCoroutine(Focus_Move(target, endPos, moveTime));
    }

    private IEnumerator Focus_Move(GameObject target, Vector3 endPos, float moveTime)
    {
        // 지정한 시간만큼, 지정한 위치로 Lerp 이동
        Vector3 startPos = transform.position;

        // Move
        float timer = 0;
        float speed = 1;
        while (timer < 1)
        {
            // Target Look
            attackDir = (target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(attackDir);
            transform.forward = attackDir;

            // Move
            speed = (timer < 0.7f) ? (speed + 1.5f * Time.deltaTime) : speed;
            timer += speed * Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(startPos, endPos, InOutCirc(timer));
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(1f);
        StartCoroutine(nameof(Forcus_Attack));
    }

    private IEnumerator Forcus_Attack()
    {
        // Sound On
        //sound.DroneSound_Call(Sound_TypeRed.DroneSound.Attack);

        // Attack
        laser_Collider.GetComponent<Enemy_AttackCollider>().enemy = this.enemy;
        laser_VFX.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        laser_VFX.SetActive(false);

        // Delay
        yield return new WaitForSeconds(0.25f);

        // VFX
        Instantiate(endVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion

    public static float InCirc(float t) => -((float)Mathf.Sqrt(1 - t * t) - 1);
    public static float OutCirc(float t) => 1 - InCirc(1 - t);
    public static float InOutCirc(float t)
    {
        if (t < 0.5) return InCirc(t * 2) / 2;
        return 1 - InCirc((1 - t) * 2) / 2;
    }
}
