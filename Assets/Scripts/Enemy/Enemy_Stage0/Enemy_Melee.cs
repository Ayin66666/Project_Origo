using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : Enemy_Base
{
    [Header("---Attack Setting---")]
    [SerializeField] private GameObject dashAttackVFX;
    [SerializeField] private GameObject dashMoveVFX;
    [SerializeField] private GameObject normalVFX;

    [SerializeField] private GameObject normal_Collider;
    [SerializeField] private GameObject dash_Collider;
    [SerializeField] private float delayB;

    private string[] animTrigger =  { "Attack", "Groggy", "Step" };
    private string[] animBool = { "isDelayMove", "isNormalAttack", "isDashCharge", "isDashAttack", "isGroggy", "isDashCharge", "isStep" };

    [Header("---Sound---")]
    [SerializeField] private Sound_Melee sound;

    void OnEnable()
    {
        StartCoroutine(Spawn());
        nav.enabled = false;
    }

    private void Update()
    {
        // Test
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        // Target
        if (target == null || state == State.Groggy)
        {
            return;
        }

        // Enemy Look Setting
        LookAt();

        // FSM
        if(state == State.Idle && !isAttack && !isGroggy)
        {
            Think();
        }
    }

    private void Think()
    {
        state = State.Think;

        // Distance Check
        TargetDistance_Setting();
        if (targetDistance > 5)
        {
            StartCoroutine(nameof(Chase));
        }
        else
        {
            Attack_Think();
        }
    }

    private void Attack_Think()
    {
        TargetDistance_Setting();
        if(targetDistance <= 3)
        {
            StartCoroutine(nameof(Normal_Attack));
        }
        else if(targetDistance > 3)
        {
            StartCoroutine(nameof(Dash_Attack));
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
                controller.Move(2 * Time.deltaTime * dir);

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
                controller.Move(2 * Time.deltaTime * dir);

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
        if(ran > 30)
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
        StopCoroutine(nameof(Normal_Attack));
        StopCoroutine(nameof(Dash_Attack));

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
        yield return new WaitForSeconds(delayB);

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
        while (timer < 1 && state != State.Groggy)
        {
            acceleration = (acceleration > 1) ? (acceleration - 150f * Time.deltaTime) : acceleration = 1;
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
        isLook = true;

        // Sound On
        sound.SoundCall_Loop(Sound_Melee.SoundType.Move, true);

        // Move
        while (targetDistance > 5)
        {
            // Speed Setting
            anim.SetFloat("Move", 1);
            TargetDistance_Setting();
            if (targetDistance > 15)
            {
                nav.speed = status.MoveSpeed * 0.7f;
            }
            else if(targetDistance < 10)
            {
                nav.speed = status.MoveSpeed;
            }

            nav.SetDestination(target.transform.position);
            yield return null;
        }

        // Sound false
        sound.SoundCall_Loop(Sound_Melee.SoundType.Move, false);

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

    private IEnumerator Normal_Attack()
    {
        // Set FSM to Attack
        state = State.Attack;
        isAttack = true;
        isLook = true;

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isNormalAttack", true);

        // Animation Wait
        while(anim.GetBool("isNormalAttack"))
        {
            yield return null;
        }

        yield return new WaitForSeconds(delayB);

        // Backstep
        if(attackCount > 3)
        {
            Backstep_Setting();
            if(isBackstep)
            {
                StartCoroutine(nameof(Backstep));
            }
            else
            {
                StartCoroutine(Attack_Delay(2f));
            }
        }
        else
        {
            attackCount++;
            StartCoroutine(Attack_Delay(2f));
        }
    }

    public void NormalAttack()
    {
        // Sound On
        sound.SoundCall_OneShot(Sound_Melee.SoundType.NormalAttack);

        normalVFX.SetActive(true);
        normal_Collider.SetActive(normal_Collider.activeSelf ? false : true);
    }

    private IEnumerator Dash_Attack()
    {
        state = State.Attack;
        isAttack = true;

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isDashCharge", true);
        anim.SetBool("isDashAttack", true);

        // Attack Delay F
        yield return new WaitForSeconds(0.5f);

        // Attack
        isLook = false;
        TargetDistance_Setting();
        anim.SetBool("isDashCharge", false);
        dashAttackVFX.SetActive(true);

        // Animation Wait
        while (anim.GetBool("isDashAttack"))
        {
            yield return null;
        }

        // Attack Delay B
        yield return new WaitForSeconds(delayB);
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

    public void DashMove()
    {
        StartCoroutine(DashMovement());
    }

    private IEnumerator DashMovement()
    {
        dashMoveVFX.SetActive(true);

        float acceleration = 60f;
        float timer = 0;
        while (timer < 1 && state != State.Groggy)
        {
            moveDir.y = 0;
            acceleration = (acceleration > 0) ? (acceleration - 150f * Time.deltaTime) : acceleration = 1;
            controller.Move(acceleration * Time.deltaTime * moveDir);
            timer += Time.deltaTime * 2f;
            yield return null;
        }
    }

    public void DashAttack()
    {
        // Sound On
        sound.SoundCall_OneShot(Sound_Melee.SoundType.RushAttack);
        dash_Collider.SetActive(dash_Collider.activeSelf ? false : true);
    }

    protected override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieCall());
    }

    private IEnumerator DieCall()
    {
        state = State.Die;
        isAttack = false;
        isGroggy = false;
        isLook = false;

        // Sound On
        sound.SoundCall_OneShot(Sound_Melee.SoundType.Die);

        // Attack Collider Reset
        normal_Collider.SetActive(false);
        dash_Collider.SetActive(false);

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

        // Animation Setting
        anim.SetTrigger("Die");
        anim.SetBool("isDie", true);

        // Animation Wait
        while(anim.GetBool("isDie"))
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
        StartCoroutine(Groggy_Setting());
    }

    private IEnumerator Groggy_Setting()
    {
        // Groggy State Reset
        state = State.Groggy;
        isGroggy = true;
        isLook = false;
        isAttack = false;
        isBackstep = false;
        isIvincible = false;
        normal_Collider.SetActive(false);
        dash_Collider.SetActive(false);

        // Sound On
        sound.SoundCall_OneShot(Sound_Melee.SoundType.Groggy);

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

        // Animation Setting
        anim.SetTrigger("Groggy");
        anim.SetBool("isGroggy", true);

        // Groggy Count
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / status.GroggyTime;
            yield return null;
        }

        // Animation Wait
        while(anim.GetBool("isGroggy"))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        // Reset
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
