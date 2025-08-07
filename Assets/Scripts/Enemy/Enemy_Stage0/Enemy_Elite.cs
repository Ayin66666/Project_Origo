using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Elite : Enemy_Base
{
    [Header("---Attack Setting---")]
    [SerializeField] private GameObject attack_Collider;
    [SerializeField] private GameObject rush_Collider;
    [SerializeField] private GameObject bulletB;
    [SerializeField] private GameObject shotPos;

    [Header("---Attack VFX---")]
    [SerializeField] private GameObject normalAttackVFX;
    [SerializeField] private GameObject dashAttackVFX;
    [SerializeField] private GameObject dashMoveVFX;
    [SerializeField] private GameObject shotVFX;

    [Header("---Attack Status---")]
    [SerializeField] private bool isGuard;
    [SerializeField] private float delayF;
    [SerializeField] private float delayB;
    [SerializeField] private float bulletSpeed;
    private Vector3 shotDir;

    [Header("---Die Explison---")]
    [SerializeField] private GameObject dialog;
    [SerializeField] private GameObject explsionVFX;
    [SerializeField] private Transform[] explsionPos;
    [SerializeField] private GameObject[] parts;

    [Header("---Sound---")]
    [SerializeField] private Sound_Elite sound;
    [SerializeField] private GameObject uiObj;

    [Header("---UI---")]
    [SerializeField] private GameObject fadeSet;
    [SerializeField] private Image nameFadeImage;
    [SerializeField] private Text bossNameText;

    private string[] animTrigger = { "Attack", "Groggy", "Step" };
    private string[] animBool = { "isNormalAttack", "isRushAttack", "isCharge", "isDelayMove", "isGroggy", "isGroggyAnim", "isStepChage", "isStep" };

    void OnEnable()
    {
        StartCoroutine(Spawn());
        nav.enabled = false;
    }

    private void Update()
    {
        // Test
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (state == State.Die && state == State.Groggy && state == State.Spawn)
        {
            return;
        }

        if(target != null && !uiObj.gameObject.activeSelf)
        {
            StartCoroutine(nameof(SpawnNameOn));
            uiObj.SetActive(true);
        }

        if (target == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Die();
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
        TargetDistance_Setting();
        if (targetDistance > 10)
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
        int ran = Random.Range(0, 100);
        if (ran <= 40)
        {
            StartCoroutine(nameof(Normal_Attack));
        }
        else if (ran < 70)
        {
            StartCoroutine(nameof(Dash_Attack));
        }
        else
        {
            StartCoroutine(nameof(Guard_Shoting));
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
                controller.Move(5 * Time.deltaTime * dir);

                // Animation
                anim.SetFloat("MoveX", dir.x);
                anim.SetFloat("MoveY", dir.z);
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
                controller.Move(5 * Time.deltaTime * dir);

                // Animation
                anim.SetFloat("MoveX", dir.x);
                anim.SetFloat("MoveY", dir.z);
                time -= Time.deltaTime;
                yield return null;
            }
        }

        // Animation Reset
        anim.SetBool("isDelayMove", false);
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", 0);

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
        StopCoroutine(nameof(Normal_Attack));
        StopCoroutine(nameof(Guard_Shoting));

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
        Debug.Log(moveDir);

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
        isLook = true;

        // Sound On
        sound.SoundCall_Loop(true);

        // Move
        float maxTime = 1;
        float timer = 0;
        while (targetDistance > 9)
        {
            if (timer < maxTime)
            {
                timer += 2 * Time.deltaTime;
                anim.SetFloat("Move", timer);
            }
            else
            {
                anim.SetFloat("Move", maxTime);
            }

            // Speed Setting
            anim.SetFloat("Move", 1);
            TargetDistance_Setting();
            if (targetDistance > 20)
            {
                nav.speed = status.MoveSpeed * 0.7f;
            }
            else if (targetDistance < 15)
            {
                nav.speed = status.MoveSpeed;
            }

            nav.SetDestination(target.transform.position);
            yield return null;
        }

        // Sound Off
        sound.SoundCall_Loop(false);

        // Move Animation End
        timer = maxTime;
        while(timer > 0)
        {
            anim.SetFloat("Move", timer);
            timer -= 3f * Time.deltaTime;
            yield return null;
        }
        anim.SetFloat("Move", 0);

        // Reset State
        state = State.Idle;
        nav.enabled = false;
    }

    private IEnumerator Normal_Attack()
    {
        state = State.Attack;
        isAttack = true;
        isLook = false;

        // Attack Delay F
        yield return new WaitForSeconds(delayF);

        // Attack
        anim.SetTrigger("Attack");
        anim.SetBool("isNormalAttack", true);

        // Animation Wait
        while(anim.GetBool("isNormalAttack"))
        {
            yield return null;
        }
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
                StartCoroutine(Attack_Delay(delayB));
            }
        }
        else
        {
            // Attack Delay B
            attackCount++;
            StartCoroutine(Attack_Delay(delayB));
        }
    }

    public void NormalVFX()
    {
        // Sound On
        sound.SoundCall_OneShot(Sound_Elite.SoundType.NormalAttack);

        normalAttackVFX.SetActive(true);
    }

    private IEnumerator Dash_Attack()
    {
        state = State.Attack;
        isAttack = true;

        // Animaton Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isCharge", true);
        anim.SetBool("isRushAttack", true);

        // Attack Delay F
        float timer = 1f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            TargetDistance_Setting();
            moveDir.y = 0;
            yield return null;
        }
        isLook = false;
        anim.SetBool("isCharge", false);
        Instantiate(dashMoveVFX, transform.position, Quaternion.identity);
        Ignore_PlayerCollider(true);

        // Attack
        while (anim.GetBool("isRushAttack"))
        {
            yield return null;
        }
        Ignore_PlayerCollider(false);

        // Delay B
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
                StartCoroutine(Attack_Delay(delayB));
            }
        }
        else
        {
            // Attack Delay B
            attackCount++;
            StartCoroutine(Attack_Delay(delayB));
        }
    }

    public void RushAttackVFX()
    {
        // Sound On
        sound.SoundCall_OneShot(Sound_Elite.SoundType.DashAttack);

        dashAttackVFX.SetActive(true);
        StartCoroutine(AttackCollider());
    }

    private IEnumerator AttackCollider()
    {
        rush_Collider.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        rush_Collider.SetActive(false);
    }

    public void RushMove()
    {
        StartCoroutine(RushMovement());
    }

    private IEnumerator RushMovement()
    {
        float acceleration = 45f;
        float timer = 0;
        while (timer < 1)
        {
            acceleration = (acceleration > 0) ? (acceleration - 45f * Time.deltaTime) : acceleration = 1;
            controller.Move(acceleration * Time.deltaTime * moveDir.normalized);
            timer += Time.deltaTime * 2f;
            yield return null;
        }
    }

    private IEnumerator Guard_Shoting()
    {
        state = State.Attack;
        isAttack = true;
        isGuard = true;

        // Attack Delay F
        yield return new WaitForSeconds(delayF);

        // Guard Up
        reduceDamage = status.ReduceDamage;

        // Attack
        StartCoroutine(nameof(Shoting));

        // Animation
        anim.SetBool("isDelayMove", true);
 
        // Move
        float timer = 3f;
        while(timer > 0)
        {
            // Movement
            Vector3 dir = ((transform.right) + (-transform.forward)).normalized;
            dir.y = 0;
            controller.Move(5 * Time.deltaTime * dir);

            // Animation
            anim.SetFloat("MoveX", dir.x);
            anim.SetFloat("MoveY", dir.z);
            timer -= Time.deltaTime;
            yield return null;
        }

        // Guard Down
        reduceDamage = 0f;

        // Animation Reset
        float x = anim.GetFloat("MoveX");
        float y = anim.GetFloat("MoveY");
        while(x > 0 || y > 0)
        {
            x = (x > 0) ? x - 3f * Time.deltaTime : 0;
            y = (y > 0) ? y - 3f * Time.deltaTime : 0;
            anim.SetFloat("MoveX", x);
            anim.SetFloat("MoveY", y);
            yield return null;
        }

        anim.SetBool("isDelayMove", false);
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", 0);

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
                StartCoroutine(Attack_Delay(delayB));
            }
        }
        else
        {
            // Attack Delay B
            attackCount++;
            StartCoroutine(Attack_Delay(delayB));
        }
    }

    private IEnumerator Shoting()
    {
        // Shoting
        float count = 15f;
        while(count > 0)
        {
            // VFX
            Instantiate(shotVFX, shotPos.transform.position, Quaternion.identity);

            // Sound On
            sound.SoundCall_OneShot(Sound_Elite.SoundType.Shot);

            // Attack
            GameObject bullet = Instantiate(bulletB, shotPos.transform.position, Quaternion.identity);
            shotDir = (target.transform.position - shotPos.transform.position).normalized;
            bullet.GetComponent<Enemy_Bullet>().Shot_Setting(Enemy_Bullet.BulletType.BulletA, target, shotDir, bulletSpeed, status.Damage,status.ArmorPenetration);
            count--;
            yield return new WaitForSeconds(0.2f);
        }
    }

    protected override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieCall());
    }

    private IEnumerator Die_Explsion() // 아직 테스트용임!
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].transform.parent = null;
            Rigidbody rigid = parts[i].AddComponent<Rigidbody>();
            rigid.AddForce(new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(0, 2), Random.Range(-1.2f, 1.2f)) * Random.Range(10, 15), ForceMode.Impulse);
            yield return new WaitForSeconds(Random.Range(0, 0.1f));
        }
    }

    private IEnumerator DieVFX()
    {
        for (int i = 0; i < explsionPos.Length; i++)
        {
            // Sound On
            sound.SoundCall_OneShot(Sound_Elite.SoundType.Die);

            // VFX
            Instantiate(explsionVFX, explsionPos[i].position, Quaternion.identity);

            // Delay
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
        }
    }

    private IEnumerator DieCall()
    {
        state = State.Die;
        nav.enabled = false;
        isGroggy = false;
        isAttack = false;
        isLook = false;
        collider.enabled = false;

        // Attack Reset
        attack_Collider.SetActive(false);
        rush_Collider.SetActive(false);

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

        // Die VFX
        //StartCoroutine(Die_Explsion()); -> 이거 전에는 동작했는데 왜 이젠 안되지?
        StartCoroutine(DieVFX());

        // Animation Wait
        while(anim.GetBool("isDie"))
        {
            yield return null;
        }

        GameObject obj = Instantiate(explsionVFX, explsionPos[2].position, Quaternion.identity);
        obj.transform.localScale = new Vector3(3, 3, 3);

        // Dialog Obj On
        if(haveDialog)
        {
            dialog.SetActive(true);
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
        isIvincible = false;
        nav.enabled = false;
        isAttack = false;
        isLook = false;

        // Sound On
        sound.SoundCall_OneShot(Sound_Elite.SoundType.Groggy);

        // Attack Reset
        attack_Collider.SetActive(false);
        rush_Collider.SetActive(false);

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
        anim.SetTrigger("Groggy");
        anim.SetBool("isGroggy", true);

        // Groggy Count
        float timer = status.GroggyTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isGroggy", false);

        // Animation Wait
        while(anim.GetBool("isGroggyAnim"))
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.25f);
        curSuperArmor = 0;
        state = State.Idle;
        isGroggy = false;
        isLook = true;
    }

    protected override IEnumerator Spawn()
    {
        // Delay
        yield return new WaitForSeconds(0.25f);
        state = State.Idle;
    }

    private IEnumerator SpawnNameOn()
    {
        fadeSet.SetActive(true);

        // 강조선 On
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime;
            nameFadeImage.color = new Color(nameFadeImage.color.r, nameFadeImage.color.g, nameFadeImage.color.b, a);
            yield return null;
        }

        // Name Text On
        a = 0;
        while (a < 1)
        {
            a += Time.deltaTime;
            bossNameText.color = new Color(bossNameText.color.r, bossNameText.color.g, bossNameText.color.b, a);
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.25f);

        // Text Off
        a = 1;
        while (a > 0)
        {
            a -= Time.deltaTime;
            bossNameText.color = new Color(bossNameText.color.r, bossNameText.color.g, bossNameText.color.b, a);
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.15f);

        // 강조선 Off
        a = 1;
        while (a > 0)
        {
            nameFadeImage.color = new Color(nameFadeImage.color.r, nameFadeImage.color.g, nameFadeImage.color.b, a);
            a -= Time.deltaTime;
            yield return null;
        }

        fadeSet.SetActive(false);
        Destroy(fadeSet);
    }
}
