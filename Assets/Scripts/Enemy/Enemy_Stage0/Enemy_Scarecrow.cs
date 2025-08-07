using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Scarecrow : Enemy_Base
{
    // Animation Setting
    private string[] animTrigger = { "Attack", "Groggy" };
    private string[] animBool = { "isAttack", "isGroggy" };

    [Header("---Attack Setting---")]
    [SerializeField] private GameObject normal_Collider;
    [SerializeField] private float delayB;

    [Header("---Sound Setting---")]
    [SerializeField] private Sound_Scarecrow sound;

    void OnEnable()
    {
        StartCoroutine(Spawn());
        nav.enabled = false;
    }

    void Update()
    {
        // Test
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (state == State.Die && state == State.Groggy && state == State.Spawn)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // TakeDamage(9099, 1, HitType.Groggy);
        }

        // Enemy Look Setting
        LookAt();

        // FSM
        if (state == State.Idle && !isAttack)
        {
            Think();
        }
    }

    protected override IEnumerator Spawn()
    {
        yield return null;
        state = State.Idle;
    }

    private void Think()
    {
        state = State.Think;

        // Distance Check
        TargetDistance_Setting();
        if (targetDistance > 3)
        {
            StartCoroutine(nameof(Chase));
        }
        else
        {
            StartCoroutine(nameof(Attack));
        }
    }

    private IEnumerator Chase()
    {
        // State to Chase
        state = State.Chase;
        nav.enabled = true;
        isLook = true;

        // Sound
        // sound.SoundCall_Loop(Sound_Scarecrow.SoundType.Move, true);

        // Move
        while (targetDistance > 2)
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

        // Sound
        // sound.SoundCall_Loop(Sound_Scarecrow.SoundType.Move, false);

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


    private IEnumerator Attack()
    {
        // Set FSM to Attack
        state = State.Attack;
        isAttack = true;
        isLook = false;

        // Look Delay
        float timer = 0.25f;
        while(timer > 0)
        {
            HardLook();
            timer -= Time.deltaTime;
            yield return null;
        }

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isAttack", true);

        // Animation Wait
        while (anim.GetBool("isAttack"))
        {
            yield return null;
        }

        yield return new WaitForSeconds(delayB);

        state = State.Idle;
        isAttack = false;
        isLook = true;
    }

    public void AttackCollider()
    {
        // Sound
        sound.SoundCall_OneShot(Sound_Scarecrow.SoundType.Attack);
        normal_Collider.SetActive(normal_Collider.activeSelf ? false : true);
    }

    protected override void Groggy()
    {
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
        normal_Collider.SetActive(false);

        // Sound
        sound.SoundCall_OneShot(Sound_Scarecrow.SoundType.Groggy);

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
        anim.SetBool("isGroggy", false);

        yield return new WaitForSeconds(0.25f);

        // Reset
        state = State.Idle;
        curSuperArmor = 0;
        isGroggy = false;
        isLook = true;
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
        nav.enabled = false;
        collider.enabled = false;
        controller.Move(Vector3.zero);

        // Sound
        sound.SoundCall_OneShot(Sound_Scarecrow.SoundType.Die);

        // Attack Collider Reset
        normal_Collider.SetActive(false);

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
        anim.SetTrigger("Die");
        anim.SetBool("isDie", true);

        // Animation Wait
        while (anim.GetBool("isDie"))
        {
            yield return null;
        }

        // body Object Setting
        body.transform.parent = null;

        // Delay
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
