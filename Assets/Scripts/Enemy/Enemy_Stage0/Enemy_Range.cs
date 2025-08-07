using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy_Base
{
    [Header("---Attack Setting---")]
    [SerializeField] private GameObject bulletA;
    [SerializeField] private GameObject bulletB;
    [SerializeField] private Transform shotPosA;
    [SerializeField] private float attackDeley;

    [SerializeField] private GameObject rushMoveStartVFX;
    [SerializeField] private GameObject rushMoveEndVFX;
    [SerializeField] private GameObject shotVFX;

    private Vector3 shotDir;

    // Animation Trigger & Bool
    private string[] animTrigger = { "Attack", "Groggy" };
    private string[] animBool = { "isNormalAttack", "isNormalCharge", "isDelayMove", "isMovingCharge", "isMoving", "isMovingAttack", "isGroggy" };

    [Header("---Sound---")]
    [SerializeField] private Sound_Range sound;

    void OnEnable()
    {
        StartCoroutine(Spawn());
        nav.enabled = false;
    }
    private void Update()
    {
        // Test
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (target == null)
        {
            return;
        }

        // Enemy Look Setting
        LookAt();

        // FSM
        if (state == State.Idle && !isAttack)
        {
            Think();
        }
    }

    private void Think()
    {
        state = State.Think;

        // Distance Check
        if(target != null)
        {
            TargetDistance_Setting();
        }
        else
        {
            return;
        }

        if (targetDistance > 15)
        {
            StartCoroutine(nameof(Chase));
        }
        else
        {
            Debug.Log("Call Attack think");
            Attack_Think();
        }
    }

    private void Attack_Think()
    {
        TargetDistance_Setting();
        int ran = Random.Range(0, 100);
        if(ran < 70)
        {
            StartCoroutine(nameof(NormalShot_Attack));
        }
        else
        {
            StartCoroutine(nameof(MovingShot_Attack));
        }
    }

    private IEnumerator Attack_Delay(float time)
    {
        // Setting FSM to Attack Delay
        state = State.Attack_Delay;
        isAttack = false;

        // Animation
        anim.SetBool("isDelayMove", true);

        // Attack Delay Move (Right or Left)
        int ran = Random.Range(0, 100);
        if (ran < 50)
        {
            while (time > 0)
            {
                // Movement
                Vector3 dir = ((Vector3.right) + (-transform.forward)).normalized;
                dir.y = 0;
                controller.Move(2.5f * Time.deltaTime * dir);

                // Animation
                anim.SetFloat("MoveX", dir.x);
                anim.SetFloat("MoveZ", dir.z);
                time -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (time > 0)
            {
                // Movement
                Vector3 dir = ((-Vector3.right) + (-transform.forward)).normalized;
                dir.y = 0;
                controller.Move(2.5f * Time.deltaTime * dir);

                // Animation
                anim.SetFloat("MoveX", dir.x);
                anim.SetFloat("MoveZ", dir.z);
                time -= Time.deltaTime;
                yield return null;
            }
        }

        // Animation Reset
        anim.SetBool("isDelayMove", false);
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveZ", 0);

        // Reset FSM to Idle
        state = State.Idle;
    }

    private void Backstep_Setting()
    {
        int ran = Random.Range(0, 100);
        if (ran > 30)
        {
            isBackstep = true;
        }
        else
        {
            isBackstep = false;
        }
    }

    private IEnumerator Backstep()
    {
        // Attack Reset
        StopCoroutine(nameof(NormalShot_Attack));
        StopCoroutine(nameof(MovingShot_Attack));

        // Reset Backstep Value
        state = State.Attack_Delay;
        isBackstep = true;
        attackCount = 0;

        // Animation
        anim.SetTrigger("Step");
        anim.SetBool("isStepChage", true);
        anim.SetBool("isStep", true);

        // Delay
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isStepChage", false);

        // Animation Delay
        while (anim.GetBool("isStep"))
        {
            yield return null;
        }

        // Backstep Delay
        yield return new WaitForSeconds(attackDeley);

        // Reset FSM to Idle
        state = State.Idle;
        isBackstep = false;
        isAttack = false;
        isLook = true;

    }

    public void StepMove()
    {
        StartCoroutine(StepMovement());
    }

    private IEnumerator StepMovement()
    {
        // MoveDir Setting
        moveDir = (transform.position - target.transform.position).normalized;
        moveDir.y = 0;
        isLook = false;

        // Move
        float acceleration = 60f;
        float timer = 0;
        while (timer < 1)
        {
            acceleration = (acceleration > 1) ? (acceleration - 360 * Time.deltaTime) : acceleration = 1;
            controller.Move(acceleration * Time.deltaTime * moveDir);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        // State to Chase
        state = State.Chase;
        nav.enabled = true;

        // Sound On
        // sound.SoundCall_Loop(true);

        // Move
        while (targetDistance > 15)
        {
            // Speed Setting
            anim.SetFloat("Move", 1);
            TargetDistance_Setting();
            if (targetDistance > 15)
            {
                nav.speed = status.MoveSpeed * 0.7f;
            }
            else if (targetDistance < 10)
            {
                nav.speed = status.MoveSpeed;
            }

            nav.SetDestination(target.transform.position);
            yield return null;
        }

        // Sound On
        // sound.SoundCall_Loop(false);

        // Move Animation End
        float timer = 1;
        while (timer > 0)
        {
            anim.SetFloat("Move", timer);
            timer -= Time.deltaTime * 10f;
            yield return null;
        }
        anim.SetFloat("Move", 0);

        // Reset State
        state = State.Idle;
        nav.enabled = false;
    }

    private IEnumerator NormalShot_Attack()
    {
        state = State.Attack;
        isAttack = true;

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isNormalCharge", true);
        anim.SetBool("isNormalAttack", true);

        // Attack Delay F
        while (anim.GetBool("isNormalCharge"))
        {
            yield return null;
        }
        isLook = false;

        // Animation Wait
        while (anim.GetBool("isNormalAttack"))
        {
            yield return null;
        }

        // Delay B
        yield return new WaitForSeconds(attackDeley);
        isLook = true;

        // Backstep
        if (attackCount > 3)
        {
            Backstep_Setting();
            if (isBackstep)
            {
                StartCoroutine(nameof(Backstep));
            }
            else
            {
                // Attack Delay B
                StartCoroutine(Attack_Delay(1.25f));
            }
        }
        else
        {
            // Attack Delay B
            attackCount++;
            StartCoroutine(Attack_Delay(1.25f));
        }
    }

    public void NormalAttack()
    {
        StartCoroutine(NormalShot());
    }

    private IEnumerator NormalShot()
    {
        for (int i = 0; i < 3; i++)
        {
            // VFX
            Instantiate(shotVFX, shotPosA.position, Quaternion.identity);

            // Sound On
            sound.SoundCall_OneShot(Sound_Range.SoundType.Shot);

            // Attack
            shotDir = (target.transform.position - shotPosA.transform.position).normalized;
            GameObject bullet = Instantiate(bulletA, shotPosA.position, Quaternion.identity);
            bullet.GetComponent<Enemy_Bullet>().Shot_Setting(Enemy_Bullet.BulletType.BulletA, target, shotDir, 15, status.Damage, status.ArmorPenetration);
            yield return new WaitForSeconds(0.15f);
        }
    }

    private IEnumerator MovingShot_Attack()
    {
        state = State.Attack;
        isAttack = true;

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isMovingCharge", true);
        anim.SetBool("isMoving", true);
        anim.SetBool("isMovingAttack", true);

        // VFX
        rushMoveStartVFX.SetActive(true);

        // Animatinon Wait
        while(anim.GetBool("isMovingCharge"))
        {
            yield return null;
        }

        // Attack Delay F
        yield return new WaitForSeconds(0.25f);

        // Move & Attack
        StartCoroutine(nameof(MovingShot));
        float timer = 2f;
        while(timer > 0)
        {
            controller.Move((status.MoveSpeed * 1.2f) * Time.deltaTime * transform.right);
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isMoving", false);

        // Animation Wait
        while(anim.GetBool("isMovingAttack"))
        {
            yield return null;
        }


        // Attack Delay B
        yield return new WaitForSeconds(attackDeley);

        // Backstep
        if (attackCount > 3)
        {
            Backstep_Setting();
            if (isBackstep)
            {
                StartCoroutine(nameof(Backstep));
            }
            else
            {
                // Attack Delay B
                StartCoroutine(Attack_Delay(1.25f));
            }
        }
        else
        {
            // Attack Delay B
            attackCount++;
            StartCoroutine(Attack_Delay(1.25f));
        }
    }

    public void MoveDownVFX()
    {
        // VFX
        rushMoveEndVFX.SetActive(true);
    }

    private IEnumerator MovingShot()
    {
        int shotCount = 6;
        while(shotCount > 0)
        {
            shotDir = (target.transform.position - transform.position).normalized;
            shotDir.y = -0.5f;

            // VFX
            Instantiate(shotVFX, shotPosA.position, Quaternion.identity);

            // Sound On
            sound.SoundCall_OneShot(Sound_Range.SoundType.Shot);

            // Attack
            GameObject bullet = Instantiate(bulletB, shotPosA.position, Quaternion.identity);
            bullet.GetComponent<Enemy_Bullet>().Shot_Setting(Enemy_Bullet.BulletType.BulletA, target, shotDir, 15, status.Damage,status.ArmorPenetration);
            shotCount--;
            yield return new WaitForSeconds(0.25f);
        }
    }

    protected override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieCall());
    }

    private IEnumerator DieCall()
    {
        state = State.Die;
        nav.enabled = false;
        isGroggy = false;
        isAttack = false;
        isLook = false;
        collider.enabled = false;

        // Sound On
        sound.SoundCall_OneShot(Sound_Range.SoundType.Die);

        // Animation Reset
        for (int i = 0; i < animTrigger.Length; i++)
        {
            anim.ResetTrigger(animTrigger[i]);
        }
        for (int i = 0; i < animBool.Length; i++)
        {
            anim.SetBool(animBool[i], false);
        }

        // Animation Setting
        anim.SetTrigger("Die");
        anim.SetBool("isDie", true);

        // Animation Wait
        while (anim.GetBool("isDie"))
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    protected override void Groggy()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(Groggy_Setting));
    }

    private IEnumerator Groggy_Setting()
    {
        state = State.Groggy;
        isGroggy = true;
        isAttack = false;
        isLook = false;
        isIvincible = false;

        // Sound On
        sound.SoundCall_OneShot(Sound_Range.SoundType.Groggy);

        // Animation Reset
        for (int i = 0; i < animTrigger.Length; i++)
        {
            anim.ResetTrigger(animTrigger[i]);
        }
        for (int i = 0; i < animBool.Length; i++)
        {
            anim.SetBool(animBool[i], false);
        }
        anim.SetFloat("Move", 0);
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveZ", 0);
        anim.SetFloat("GroggyT", 0);

        // Groggy Count
        float timer = 1;
        while (timer < 1)
        {
            timer += Time.deltaTime / status.GroggyTime;
            anim.SetFloat("GroggyT", timer);
            yield return null;
        }
        curSuperArmor = 0;
        state = State.Idle;
        isGroggy = false;
        isLook = true;
    }

    protected override IEnumerator Spawn()
    {
        yield return null;
        state = State.Idle;
    }
}
