using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy_Base : MonoBehaviour
{
    [Header("---Component---")]
    [SerializeField] protected Enemy_StatusUI statusUI;
    [SerializeField] protected GameObject body;
    [SerializeField] protected Animator anim;
    protected CharacterController controller;
    protected NavMeshAgent nav;
    protected Collider collider;


    [Header("---Target Setting---")]
    [SerializeField] protected GameObject target;
    [SerializeField] protected GameObject searchArea;
    protected Vector3 moveDir;
    protected Vector3 lookDir;
    protected float targetDistance;

    [Header("---Status---")]
    public Enemy_StatusSO status;
    public enum Enemy_Type { Normal, Elite, Boss }
    public Enemy_Type type;

    [Header("---Attack---")]
    public int damage;
    public int armorPenetration;

    [Header("---Defence---")]
    public int maxHp;
    public int curHp;
    public int maxSuperArmor;
    public int curSuperArmor;
    public float reduceDamage;
    public float groggyTime;

    [Header("---Other---")]
    public float moveSpeed;
    public float attackSpeed;
    public float groggyResetTime;
    private float groggyResetTimer;
    private float curtime;

    [Header("---State---")]
    [SerializeField] protected int attackCount;
    [SerializeField] protected bool isGroggy;
    [SerializeField] protected bool isAttack;
    [SerializeField] protected bool isIvincible;
    [SerializeField] protected bool isBackstep;
    [SerializeField] protected bool isLook = true;
    [SerializeField] protected bool haveDialog;

    public enum State { Spawn, Idle, Think, Move, Chase, Attack, Attack_Delay, Groggy, Die }
    public State state;
    public enum HitType { None, Groggy }


    protected void Awake() 
    {
        // Get Component
        controller = GetComponent<CharacterController>();
        nav = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();

        // Enemy Status Setting
        Status_Setting();
    }

    public void Target_Search(GameObject player)
    {
        if (target == null) target = player;
        searchArea.SetActive(false);
    }

    protected void LookAt()
    {
        if(!isLook)
        {
            return;
        }

        if (target != null)
        {
            // LookDir Setting
            lookDir = target.transform.position - transform.position;
            lookDir.y = 0;

            // Lookat
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            this.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    protected void HardLook()
    {
        // 기존 처다보기 기능에서 2.5배 빠르게 쳐다봄
        // 테스트 결과 0.15초 내로 반대방향 플레이어를 정확하게 쳐다볼 수 있음!
        moveDir = (target.transform.position - transform.position).normalized;
        moveDir.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 20);
    }

    protected void Status_Setting() // hp, SuperArmor Setting
    {
        nav.speed = status.MoveSpeed;

        damage = status.Damage;
        armorPenetration = status.ArmorPenetration;

        maxHp = status.Hp;
        curHp = status.Hp;
        maxSuperArmor = status.SuperArmor;
        curSuperArmor = 0;
        reduceDamage = status.ReduceDamage;
        groggyTime = status.GroggyTime;

        moveSpeed = status.MoveSpeed;
        attackSpeed = status.AttackSpeed;
    }

    protected void TargetDistance_Setting()
    {
        targetDistance = 
            (new Vector3(target.transform.position.x, 0, target.transform.position.z) 
            - new Vector3(transform.position.x, 0, transform.position.z)).magnitude;
        
        moveDir = (new Vector3(target.transform.position.x, 0, target.transform.position.z)
            - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
    }

    public void TakeDamage(int damage, int armorPenetration, HitType type) 
    {
        if(isIvincible) return;

        // Damage Cal (ArmorPenetration Cal)
        int hitDamage = (damage - (status.Defense - armorPenetration));

        // Groggy Tiemr Reset;
        groggyResetTimer = groggyResetTime;

        // UI Setting
        statusUI.TimerSetting();

        // Hit CamShacke
        Effect_Manager.instance.Camera_Shake(3f, 0.05f);

        // Hit Stop
        Effect_Manager.instance.Hit_Stop(0.005f);

        // Hit Sound
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Damage, 0);

        switch (type)
        {
            case HitType.None:
                if (isGroggy)
                {
                    // Groggy Damage Cal
                    if (hitDamage > 0)
                    {
                        hitDamage = (int)(hitDamage * 1.2f);
                        curHp -= (hitDamage - (int)(hitDamage * status.ReduceDamage));
                    }

                    // Hit Ivincible
                    if (!isIvincible)
                        StartCoroutine(nameof(Hit_Ivincible));

                    // Hp check
                    if (curHp <= 0)
                        Die();
                }
                else
                {
                    if (hitDamage > 0)
                    {
                        // Hp & SuperArmor
                        curHp -= (hitDamage - (int)(hitDamage * status.ReduceDamage));
                        curSuperArmor += (int)(hitDamage * 0.5f);
                        Debug.Log((int)(hitDamage * 0.5f));
                    }

                    // Hit Ivincible
                    if (!isIvincible)
                        StartCoroutine(nameof(Hit_Ivincible));

                    // Hp check
                    if (curHp <= 0)
                    {
                        Die();
                    }
                    else
                    {
                        // Groggy Check
                        if (curSuperArmor >= maxSuperArmor && !isGroggy)
                            Groggy();
                    }
                }
                break;

            case HitType.Groggy:
                // Groggy Damage Cal
                hitDamage = (int)(hitDamage * 1.2f);
                curHp -= (hitDamage - (int)(hitDamage * status.ReduceDamage));
                curSuperArmor = 0;

                // Hp check
                if (curHp <= 0)
                {
                    Die();
                }
                else
                {
                    Groggy();
                }
                break;
        }
    }

    protected void SuperArmor_Setting()
    {
        if(groggyResetTimer > 0)
            groggyResetTimer -= Time.deltaTime;

        if (groggyResetTimer <= 0)
        {
            if (curSuperArmor > 0)
            {
                curtime += status.SuperArmorRecovery * Time.deltaTime;
                if(curtime >= 1)
                {
                    curSuperArmor -= 1;
                    curtime = 0;
                }
            }
        }
    }

    protected void Ignore_PlayerCollider(bool isOn)
    {
        Collider targetColl = target.GetComponent<Collider>();

        if(targetColl != null)
            Physics.IgnoreCollision(targetColl, collider, isOn);
    }

    protected IEnumerator Hit_Ivincible()
    {
        isIvincible = true;
        yield return new WaitForSeconds(0.1f);
        isIvincible = false;
    }

    protected abstract IEnumerator Spawn();

    protected abstract void Groggy();

    protected abstract void Die();
}
