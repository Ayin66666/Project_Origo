using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_LongLaser : MonoBehaviour
{
    public Transform target;
    public Transform startTarget;
    Quaternion test;

    void Awake()
    {
        test = transform.rotation;
    }
    void OnDisable()
    {
        Vector3 start = startTarget.position - transform.position;
        Quaternion startLook = Quaternion.LookRotation(start);
        transform.rotation = startLook;
    }

    public void Rotate_Start(float time)
    {
        Debug.Log(transform.rotation);
        StartCoroutine(Rotate(time));
    }

    private IEnumerator Rotate(float totalRotTime) // 최종 구현 버전 -> 위와 동일하게 179도 이상 회전 불가
    {
        Vector3 lookDir = target.position - transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        float timer = 0;
        float rotSpeed = 1f / totalRotTime; // 초기 회전 속도 (1초에 얼마나 회전할 것인가)
        while (timer < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, timer);
            timer += Time.deltaTime * rotSpeed;

            // Speed Up (진행도 30% 이상)
            rotSpeed = (timer > 0.3f) ? rotSpeed + Time.deltaTime * 3f : rotSpeed;

            // Speed Down (진행도 70 이상 + 속도 0.25 이상)
            rotSpeed = (timer > 0.7f) && (rotSpeed > 0.25f) ? rotSpeed - Time.deltaTime * 15f : rotSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
        Debug.Log("End");
        gameObject.SetActive(false);
    }
}
