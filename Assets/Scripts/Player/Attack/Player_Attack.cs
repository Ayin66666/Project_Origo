using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public CharacterController characterController;

    [Header("---참조 관련---")]
    [SerializeField] Player_Enforce playerEnforce;
    [SerializeField] Player_Movement playerMovement;

    [Header("---공격 관련---")]
    [SerializeField] private GameObject[] attack1Collider; //1번째 공격
    [SerializeField] private GameObject[] attack2Collider; //2번째 공격
    [SerializeField] private GameObject[] attack3Collider; //3번째 공격의 1타
    [SerializeField] private GameObject[] attack4Collider; //3번째 공격의 2타
    [SerializeField] private GameObject attack5Collider; //4번째 공격
    public int attackColliderCount;
    [SerializeField] private GameObject ultDamageCollider;
    [SerializeField] private GameObject ultGroggyCollider;
    [SerializeField] private ParticleSystem[] attackEffect;


    private Animator anim;
    public float attacktimer;
    public float moveTimer;
    public float canRotateTimer;
    public float attackDelay;
    public int curComboCount;
    public int maxComboCount;
    public enum States {Attack1, Attack2, Attack3, Attack4, GuardAttack}
    public States currentState;
    private int calAttackDamage;
    public float[] attackValue;

    void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        curComboCount = 0;
        maxComboCount = 4;
        attacktimer = 1;
        attackValue = new float[5] { 0.8f, 1.2f, 0.8f, 1.8f, 3 };
        attackColliderCount = 0;
    }

    public int CalAttackDamage()
    {
        //공격배율 계산
        //value[4] = 가드어택 공격 데미지 배율
        if((int)currentState >= 0)
        {
            calAttackDamage = (int)(Player_Status.instance.damage * attackValue[(int)currentState]);
        }
        else if(currentState == States.GuardAttack)
        {
            calAttackDamage = (int)(Player_Status.instance.damage * attackValue[4]);
        }
        return calAttackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        attacktimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && curComboCount < maxComboCount && Player_Status.instance.canNextAttack)
        {
            ComboAtk();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StateGuardAttack();
        }
    }

    void ComboAtk()
    {
        if(Player_Status.instance.isGuard || Player_Status.instance.isSkill || Player_Status.instance.isDash || Player_Status.instance.chargingEnforce || Player_Status.instance.isUlt || Player_Status.instance.isDead || Player_Status.instance.allStop)
        {
            return;
        }

        Player_Status.instance.isAttack = true;
        Player_Status.instance.canMove = false;
        Player_Status.instance.canEnforce = false;
        Player_Status.instance.canNextAttack = false;
        curComboCount++;

        if (curComboCount > maxComboCount)
        {
            curComboCount = 1;
        }

        anim.SetBool("IsAttack", true);
        anim.SetTrigger("ComboAttack");
        anim.SetInteger("ComboCount", curComboCount);

        currentState = (States)(curComboCount - 1);

        attacktimer = 0f;
        moveTimer = currentState == States.Attack4 ? 0.5f : 0.2f;
        canRotateTimer = 0.01f;

        StartCoroutine(nameof(MoveWhileAttack));
    }

    IEnumerator MoveWhileAttack()
    {
        while (moveTimer > 0) //공격 중 소폭 이동
        {
            while (canRotateTimer > 0)
            {
                transform.parent.rotation = Quaternion.LookRotation(playerMovement.camDir);
                canRotateTimer -= Time.deltaTime;
                yield return null;
            }

            if (currentState == States.Attack4)
            {
                characterController.Move(4f * Time.deltaTime * transform.parent.forward);
            }
            else
            {
                characterController.Move(4f * Time.deltaTime * transform.parent.forward);
            }

            moveTimer -= Time.deltaTime;
            yield return null;
        }
    }

    void StateGuardAttack()
    {
        currentState = States.GuardAttack;
    }


    public void OnAttack1Collider()
    {
        if(attackColliderCount > 0)
        {
            attack1Collider[attackColliderCount - 1].SetActive(!attack1Collider[attackColliderCount - 1].activeSelf ? true : false);
        }

        attack1Collider[attackColliderCount].SetActive(!attack1Collider[attackColliderCount].activeSelf ? true : false);

        if(attackColliderCount < 2)
        {
            attackColliderCount++;
        }
    }

    public void OnAttack2Collider()
    {
        if (attackColliderCount > 2)
        {
            attackColliderCount = 2;
        }

        if (attackColliderCount > 0)
        {
            attack2Collider[attackColliderCount - 1].SetActive(!attack2Collider[attackColliderCount - 1].activeSelf ? true : false);
        }

        attack2Collider[attackColliderCount].SetActive(!attack2Collider[attackColliderCount].activeSelf ? true : false);

        if (attackColliderCount < 2)
        {
            attackColliderCount++;
        }
    }

    public void OnAttack3Collider()
    {
        if (attackColliderCount > 2)
        {
            attackColliderCount = 2;
        }

        if (attackColliderCount > 0)
        {
            attack3Collider[attackColliderCount - 1].SetActive(!attack3Collider[attackColliderCount - 1].activeSelf ? true : false);
        }

        attack3Collider[attackColliderCount].SetActive(!attack3Collider[attackColliderCount].activeSelf ? true : false);

        if (attackColliderCount < 2)
        {
            attackColliderCount++;
        }
    }

    public void OnAttack4Collider()
    {
        if (attackColliderCount > 2)
        {
            attackColliderCount = 2;
        }

        if (attackColliderCount > 0)
        {
            attack4Collider[attackColliderCount - 1].SetActive(!attack4Collider[attackColliderCount - 1].activeSelf ? true : false);
        }

        attack4Collider[attackColliderCount].SetActive(!attack4Collider[attackColliderCount].activeSelf ? true : false);

        if (attackColliderCount < 2)
        {
            attackColliderCount++;
        }
    }

    public void OnAttack5Collider()
    {
        attack5Collider.SetActive(true);
    }

    public void AttackColliderCountReset()
    {
        attackColliderCount = 0;
    }

    public void AttackColliderOff()
    {
        for (int i = 0; i < 3; i++)
        {
            attack1Collider[i].SetActive(false);
            attack2Collider[i].SetActive(false);
            attack3Collider[i].SetActive(false);
            attack4Collider[i].SetActive(false);
        }
        attack5Collider.SetActive(false);
    }

    public void OnFinishedAttack()
    {
        Player_Status.instance.isAttack = false;
        Player_Status.instance.isSkill = false;
        Player_Status.instance.canMove = true;

        if(!Player_Status.instance.isEnforce)
        {
            Player_Status.instance.canEnforce = true;
        }
        //Effect_Manager.instance.Camera_FOV(!isAttack);

        curComboCount = 0;
        anim.SetBool("IsAttack", false);
        anim.SetInteger("ComboCount", curComboCount);

        Player_Status.instance.canNextAttack = true;
        for (int i = 0; i < attackEffect.Length; i++)
        {
            attackEffect[i].gameObject.SetActive(false); //공격 이펙트 비활성화
        }
    }

    public void OnUltGroggyCollider()
    {
        ultGroggyCollider.SetActive(true);
    }

    public void OnUltDamageCollider()
    {
        ultDamageCollider.SetActive(true);
    }

    public void Ult_SkillBoolReturn()
    {
        anim.SetBool("Ult_SkillFinished", false);
        Player_Status.instance.isUlt = false;
        Player_Status.instance.canMove = true;
        Player_Status.instance.isIvincible = false;
    }
}
