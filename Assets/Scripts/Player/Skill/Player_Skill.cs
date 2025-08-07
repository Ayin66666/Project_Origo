using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Skill : MonoBehaviour
{
    [Header("---Components---")]
    private Animator anim;
    public CharacterController characterController;


    [SerializeField] private Player_Attack playerAttack;
    [SerializeField] private GameObject[] skill1Collider; //1번째 공격
    [SerializeField] private GameObject[] skill2Collider; //2번째 공격
    [SerializeField] private GameObject skill3Collider; //3번째 공격


    [Header("---변수---")]
    public int skillColliderCount;
    public int curSkillCount;
    public enum States { Skill1, Skill2, Skill3 }
    public States currentState;
    private int calSkillDamage;
    public float[] skillValue;
    [SerializeField] private float skillCooltime;
    private float chain_skillCooltime_Max;
    private float skillCooltime_Max;
    public Image skillDisable;

    Coroutine skillCooltime_Cor;

    void Awake()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterController>();

        curSkillCount = 0;
        skillValue = new float[3] { 1, 1.5f, 2.5f };
        chain_skillCooltime_Max = 1.5f;
        skillCooltime_Max = 5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(nameof(Skill));
        }
        currentState = (States)(curSkillCount - 1);
    }

    IEnumerator Skill()
    {
        if(Player_Status.instance.isSkill || !Player_Status.instance.canSkill || Player_Status.instance.curSp < 200 || Player_Status.instance.isUlt || Player_Status.instance.isDead || Player_Status.instance.allStop || Player_Status.instance.chargingEnforce)
        {
            yield break;
        }

        Player_Status.instance.isIvincible = true;
        Player_Status.instance.isAttack = false;
        Player_Status.instance.isSkill = true;
        Player_Status.instance.canMove = false;
        Player_Status.instance.isGuard = false;
        Player_Status.instance.isGuardAttack = false;
        Player_Status.instance.curSp -= 100;

        anim.SetTrigger("Skill");
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsGuard", false);
        anim.SetBool("IsGuardDelayFinished", true);


        Cooltime();

        yield return null;
    }

    private void Cooltime()
    {
        Player_Status.instance.canSkill = false;

        if (skillCooltime_Cor != null)
        {
            StopCoroutine(skillCooltime_Cor);
        }

        if(curSkillCount < 2)
        {
            skillCooltime_Cor = StartCoroutine(Cal_SkillCooltime(chain_skillCooltime_Max));
        }
        else
        {
            skillCooltime_Cor = StartCoroutine(Cal_SkillCooltime(skillCooltime_Max));
        }
    }

    IEnumerator Cal_SkillCooltime(float val)
    {
        Debug.Log("실행");
        skillCooltime = val;
        while (skillCooltime > 0)
        {
            skillCooltime -= Time.deltaTime;
            skillDisable.fillAmount = skillCooltime / val;
            yield return null;
        }
        Player_Status.instance.canSkill = true;
    }

    public int CalSkillDamage()
    {
        //공격배율 계산
        if ((int)currentState >= 0)
        {
            calSkillDamage = (int)(Player_Status.instance.damage * skillValue[(int)currentState]);
        }

        return calSkillDamage;
    }

    public void SkillCount()
    {
        curSkillCount++;
        anim.SetInteger("SkillCount", curSkillCount);
    }

    public void OnFinishedSkill()
    {
        Player_Status.instance.canMove = true;
        Player_Status.instance.canNextAttack = true;
        Player_Status.instance.isSkill = false;
        Player_Status.instance.isIvincible = false;

        //Effect_Manager.instance.Camera_FOV(!isAttack);
        //curSkillCount = 0;
        playerAttack.curComboCount = 0;
    }

    public void OnFinishedAllSkill()
    {
        Debug.Log("스킬 종료");
        Player_Status.instance.canMove = true;
        Player_Status.instance.canNextAttack = true;
        Player_Status.instance.isSkill = false;
        Player_Status.instance.isIvincible = false;

        curSkillCount = 0;
        anim.SetInteger("SkillCount", curSkillCount);
        playerAttack.curComboCount = 0;
    }

    public void OnSkill1Collider()
    {
        if (skillColliderCount > 0)
        {
            skill1Collider[skillColliderCount - 1].SetActive(!skill1Collider[skillColliderCount - 1].activeSelf ? true : false);
        }

        skill1Collider[skillColliderCount].SetActive(!skill1Collider[skillColliderCount].activeSelf ? true : false);

        if (skillColliderCount < 2)
        {
            skillColliderCount++;
        }
    }
    public void OnSkill2Collider()
    {
        if (skillColliderCount > 0)
        {
            skill2Collider[skillColliderCount - 1].SetActive(!skill2Collider[skillColliderCount - 1].activeSelf ? true : false);
        }

        skill2Collider[skillColliderCount].SetActive(!skill2Collider[skillColliderCount].activeSelf ? true : false);

        if (skillColliderCount < 2)
        {
            skillColliderCount++;
        }
    }

    public void OnSkill3Collider()
    {
        skill3Collider.SetActive(true);
    }
    public void SkillColliderCountReset()
    {
        skillColliderCount = 0;
    }

    public void SkillColliderOff()
    {
        skill1Collider[skillColliderCount].SetActive(false);
        skill2Collider[skillColliderCount].SetActive(false);
        skill3Collider.SetActive(false);
    }

    public void MoveWhileSkill()
    {
        StartCoroutine(MoveWhileSkillCor());
    }

    public IEnumerator MoveWhileSkillCor()
    {
        float moveTimer;
        switch (currentState)
        {
            case States.Skill1:
                moveTimer = 0.2f;
                Debug.Log("실행");
                while (moveTimer > 0)
                {
                    moveTimer -= Time.deltaTime;
                    characterController.Move(3f * Time.deltaTime * transform.parent.forward);
                    yield return null;
                }
                break;

            case States.Skill2:
                moveTimer = 0.2f;
                Debug.Log("실행");
                while (moveTimer > 0)
                {
                    moveTimer -= Time.deltaTime;
                    characterController.Move(5f * Time.deltaTime * transform.parent.forward);
                    yield return null;
                }
                break;

            case States.Skill3:
                moveTimer = 0.5f;
                Debug.Log("실행");
                while (moveTimer > 0)
                {
                    moveTimer -= Time.deltaTime;
                    characterController.Move(6f * Time.deltaTime * transform.parent.forward);
                    yield return null;
                }
                break;
        }
        /*
        if(currentState == States.Skill1)
        {
            moveTimer = 0.2f;
            Debug.Log("실행");
            while (moveTimer > 0)
            {
                moveTimer -= Time.deltaTime;
                characterController.Move(3f * Time.deltaTime * transform.parent.forward);
                yield return null;
            }
        }
        else if (currentState == States.Skill2)
        {
            moveTimer = 0.2f;
            Debug.Log("실행");
            while (moveTimer > 0)
            {
                moveTimer -= Time.deltaTime;
                characterController.Move(5f * Time.deltaTime * transform.parent.forward);
                yield return null;
            }       
        }
        else if (currentState == States.Skill3)
        {
            moveTimer = 0.5f;
            Debug.Log("실행");
            while (moveTimer > 0)
            {
                moveTimer -= Time.deltaTime;
                characterController.Move(6f * Time.deltaTime * transform.parent.forward);
                yield return null;
            }
        }
        */
    }
}
