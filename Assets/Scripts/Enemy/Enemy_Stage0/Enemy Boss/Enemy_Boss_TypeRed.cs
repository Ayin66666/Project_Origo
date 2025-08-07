using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy_Boss_TypeRed : Enemy_Base
{
    [Header("---Mat---")]
    [SerializeField] private SkinnedMeshRenderer swordRenderer;
    [SerializeField] private SkinnedMeshRenderer hair;
    [SerializeField] private SkinnedMeshRenderer wing;
    [SerializeField] private SkinnedMeshRenderer head;
    [SerializeField] private SkinnedMeshRenderer[] dress;
    [SerializeField] private Material hairMat;
    [SerializeField] private Material dressMat;
    [SerializeField] private Material swordMat;
    [SerializeField] private Material wingMat;
    [SerializeField] private Material headMat;

    [Header("---Think Setting---")]
    [SerializeField] private int move_Percent;

    [Header("---Attack Status---")]
    [SerializeField] private float attackDelayF;
    [SerializeField] private float attackDelayB;
    [SerializeField] private bool bulletSpawn;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isStartStep;
    [SerializeField] private bool isCycle;
    [SerializeField] private bool orbitOn;
    [SerializeField] private bool isPhaseChange;
    [SerializeField] private bool isExplsion;
    public bool isSoundOn;

    public enum Phase { Phase1, Phase2 }
    public Phase phase;

    [SerializeField] private LayerMask wall;
    private string[] animation_Triggers = new string[]
    { "Attack", "ComboA", "ComboB", "Backstep", "GuardAttack", "isSpawn", "Step" };

    private string[] animation_Bools = new string[]
      { "isComboAttack", "stompingCharge", "isStomping", "isGuard", "isGuardAttack", "isStrike", "Strke_Charging", "isStrkeMove",
            "isUpwardAttack", "Upward_Charging", "isForwardSlash", "isForwardCharge", "isForwardMove","isLongThrust", "isLongThrustMove",
             "isLongThrustCharge", "isSwordAura", "isSwordAuraCharge", "isDJSlashMove", "isDJSlash", "isContinuousMove", "isContinuousStrike",
             "isThrustCharging", "isChargingThrust", "isChargingThrustAttack", "isLaserCharge", "isLaserAttackF", "isLaserAttack", "isLongLaser",
          "LongLaserChargeing","isStep", "isDelayMove" };


    [SerializeField] private GameObject[] attackColliders;

    #region Short Attack
    [Header("---Short Attack Collider & VFX---")]
    [Header("---Combo Attack---")]
    [SerializeField] private int curCombo; // Combo Count
    [SerializeField] private GameObject[] comboAVFX, comboBVFX;
    [SerializeField] private GameObject[] comboA1, comboA2, comboA3; // Attack Collider A
    [SerializeField] private GameObject[] comboB1, comboB2, comboB3, comboB4; // Attack Collider B

    [Header("---Stomping Attack---")]
    [SerializeField] private GameObject stompingVFX;
    [SerializeField] private GameObject[] stomping_ColliderA;
    [SerializeField] private GameObject[] stomping_ColliderB, stomping_ColliderC, stomping_ColliderD, stomping_ColliderE;

    [Header("---Guard Attack---")]
    [SerializeField] private GameObject guard_Collider;
    [SerializeField] private GameObject guardVFX;
    [SerializeField] private GameObject[] guardAttack_Collider;

    [Header("---Upward Slash---")]
    [SerializeField] private GameObject upwardVFX;
    [SerializeField] private GameObject[] upward_Collider;

    [Header("---Forwardstep Slash---")]
    [SerializeField] private GameObject forwardVFX;
    [SerializeField] private GameObject[] forward_Collider;

    [Header("---Strike Attack---")]
    [SerializeField] private GameObject strikeMoveVFX;
    [SerializeField] private GameObject strikeChargeVFX;
    [SerializeField] private GameObject[] strike_ColliderA;
    [SerializeField] private GameObject[] strike_ColliderB;
    #endregion

    #region Medium Attack
    [Header("---Medium Attack Collider & VFX---")]
    [Header("---Sword Aura---")]
    [SerializeField] private GameObject swordAuraVFX;
    [SerializeField] private GameObject swordAuraBullet;
    [SerializeField] private GameObject[] swordAura_Collider;

    [Header("---Dash Jump Slash---")]
    [SerializeField] private GameObject djVFX;
    [SerializeField] private GameObject[] dj_Collider;

    [Header("---Continuous Strike---")]
    [SerializeField] private GameObject[] continuous_ColliderA;
    [SerializeField] private GameObject[] continuous_ColliderB;
    [SerializeField] private GameObject continuous_ColliderC;

    [Header("---Charging Thrust---")]
    [SerializeField] private GameObject chargingThrust_MoveVFX;
    [SerializeField] private GameObject chargingThrust_AttackVFX;
    [SerializeField] private GameObject chargeingThrust_Collider;
    [SerializeField] private GameObject chargeingExplsion_Collider;

    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject origoBullet;
    #endregion

    #region Long Attack
    [Header("---Long Attack Collider & VFX---")]
    [Header("---long Chargeing Thrust---")]
    [SerializeField] private GameObject longChargeingThrust_Collider;
    [SerializeField] private GameObject longChargeExplsion_Collider;
    [SerializeField] private GameObject longChargeingAttackVFX;
    [SerializeField] private GameObject longChargeingMoveVFX;
    [SerializeField] private GameObject longChargeingVFX;

    [Header("---long Laser---")]
    [SerializeField] private GameObject[] longLasers;
    [SerializeField] private GameObject[] longLaser_Vertical;
    [SerializeField] private GameObject[] longLaserExplsionL;
    [SerializeField] private GameObject[] longLaserExplsionR;
    #endregion

    [Header("---Sound & Video---")]
    [SerializeField] private Sound_TypeRed sound;

    // UI
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject fadeObj;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image fadeImage2;
    [SerializeField] private Image nameFadeImage;
    [SerializeField] private Text bossNameText;

    // Video -> Spawn
    [SerializeField] private VideoPlayer videoPlayer_Spawn;
    [SerializeField] private GameObject videoObj_Spawn;

    // Video -> Phase2
    [SerializeField] private GameObject videoObj_Phase2;
    [SerializeField] private VideoPlayer videoPlayer_Phase2;

    // Video -> End
    [SerializeField] private VideoPlayer videoPlayer_End;
    [SerializeField] private GameObject videoObj_End;

    [Header("---Other VFX---")]
    [SerializeField] private GameObject[] stepMoveVFX;
    [SerializeField] private GameObject wingVFX;


    [Header("---Phase2 Attack Obj&VFX---")]
    [SerializeField] private GameObject orbit_Prefab;
    [SerializeField] private GameObject orbit_BackSet;
    [SerializeField] private Transform[] orbit_movePos;
    [SerializeField] private Transform[] orbit_StartPos;

    [SerializeField] private GameObject orbitMoveObj;
    [SerializeField] private Collider orbitMoveCollider;

    [SerializeField] private GameObject[] orbit_MoveVFX; // 오비트 사라질 때 VFX

    [Header("---Spawn & Attack Range---")]
    [SerializeField] private GameObject spawnRange;
    [SerializeField] private Collider spawnRangeCollider;
    [SerializeField] private GameObject attackRange;
    [SerializeField] private Collider attackRangeCollider;


    [Header("---Attack Type & Cur Attack---")]
    [SerializeField] private AttackType attackType;
    private enum AttackType { None ,ShortRange, MediumRange, LongRange }
    private enum ShortAttack { None, Combo_Attack, Stomping, Strike_Attack, Backstep_shooting, Guard, Upward_Slash, Forwardstep_Slash }
    [SerializeField] private ShortAttack shortAttack;
    private enum MediumAttack { None, Sword_Aura, DashJumpSlash, Continuous_Strike, Charging_Thrust, Laser_Attack }
    [SerializeField] private MediumAttack mediumAttack;
    private enum LongAttack { None, Charging_Thrust, Laser_Attack }
    [SerializeField] private LongAttack longAttack;


    [Header("---pattern & Weight (Short)---")]
    [SerializeField] private string[] shortRange_patterns;
    [SerializeField] private List<int> shortRange_Weights = new List<int>();

    [Header("---pattern & Weight (Medium)---")]
    [SerializeField] private string[] mediumRange_patterns;
    [SerializeField] private List<int> mediumRange_Weights = new List<int>();

    [Header("---pattern & Weight (Long)---")]
    [SerializeField] private string[] LongRange_patterns;
    [SerializeField] private List<int> longRange_Weights = new List<int>();

    [Header("---Chase Setting---")]
    [SerializeField] private float acceleration;
    [SerializeField] private float chaseTimer;
    [SerializeField] private bool isGround;
    [SerializeField] private LayerMask ground;
    private Vector3 dashDir;

    private enum ChaseType { None, Walk, Run, Dash }
    [SerializeField] private ChaseType chaseType;
    private enum PreviousAction { None, Move, Attack, Groggy }

    [Header("---Step Setting---")]
    [SerializeField] private Vector3 stepDir;
    private enum StepType { None ,Left, Right, Back, Forward }
    [SerializeField] private StepType stepType;


    [Header("---Gravity Setting---")]
    [SerializeField] private float gravityWegiht;
    [SerializeField] private float maxGravityWegiht;
    [SerializeField] private bool useGravity;


    [Header("---Pos Setting---")]
    [SerializeField] private Transform[] stepPos;
    [SerializeField] private Transform backStepUpPos;
    [SerializeField] private Transform SwordAura_ShotPos;

    private void Start()
    {
        StartCoroutine(nameof(Spawn));
    }

    private void Update()
    {
        if(state == State.Spawn && state == State.Groggy && state == State.Die)
        {
            return;
        }

        // Groggy Recovery
        if(state != State.Groggy || state != State.Die)
        {
           SuperArmor_Setting();
        }
        
        // Phase2 Check
        if(curHp <= 5000 && phase != Phase.Phase2)
        {
            Phase2On();
        }

        // FSM
        if (state == State.Idle && !isAttack && !isCycle && !isPhaseChange)
        {
             Think(); 
        }

        LookAt();
        Graivty_Setting();
    }

    private void Think()
    {
        TargetDistance_Setting();
        state = State.Think;

        int ran = Random.Range(0, 100);
        if (ran <= move_Percent)
        {
            // 추적 판단
            Chase_Setting();
        }
        else
        {
            // 공격 판단
            Attack_Think();
        }
    }

    private void Percent_Setting(PreviousAction action)
    {
        // 가중치 렌덤 값 조절기능 -> 이동 및 공격 / 그로기 시 초기화 / 상한 60 하한 30
        switch (action)
        {
            case PreviousAction.Move:
                move_Percent = (move_Percent > 30) ? move_Percent -= Random.Range(10, 20) : move_Percent;
                break;

            case PreviousAction.Attack:
                move_Percent = (move_Percent < 60) ? move_Percent += Random.Range(10, 20) : move_Percent;
                break;

            case PreviousAction.Groggy:
                move_Percent = 50;
                break;
        }
    }

    #region Chase
    private void Chase_Setting()
    {
        // Chase Type Setting
        state = State.Chase;
        Percent_Setting(PreviousAction.Move);
        TargetDistance_Setting();

        if (targetDistance <= 10)
        {
            chaseType = ChaseType.Walk;
            acceleration = 1f;
            StartCoroutine(nameof(Chase));
        }
        else if(targetDistance > 10)
        {
            chaseType = ChaseType.Run;
            acceleration = 2.5f;
            StartCoroutine(nameof(Chase));
        }
    }

    private IEnumerator Chase()
    {
        if(chaseType == ChaseType.Walk)
        {
            StartCoroutine(Chase_Timer(5f));
        }

        // Animation Setting
        float maxTime = chaseType == ChaseType.Walk ? 0.5f : 1f;
        float timer = 0;
        while (targetDistance > 5)
        {
            maxTime = chaseType == ChaseType.Walk ? 0.5f : 1f;
            if (timer < maxTime)
            {
                timer += 2 * Time.deltaTime;
                anim.SetFloat("MoveSpeed", timer);
            }
            else
            {
                anim.SetFloat("MoveSpeed", maxTime);
            }

            TargetDistance_Setting();
            controller.Move(status.MoveSpeed * acceleration * Time.deltaTime * moveDir);
            yield return null;
        }

        // Chase Animation Delay
        timer = maxTime;
        while(timer > 0)
        {
            timer -= 3f * Time.deltaTime;
            anim.SetFloat("MoveSpeed", timer);
            yield return null;
        }
        anim.SetFloat("MoveSpeed", 0);

        // Chase End
        StopCoroutine(nameof(Chase_Timer));
        chaseType = ChaseType.None;
        state = State.Think;
        Attack_Think();
    }

    private IEnumerator Dash_Chase()
    {
        // MoveDir Setting
        dashDir = target.transform.position - transform.position;
        acceleration = 5;

        // Dash
        float timer = 1f;
        while (dashDir.magnitude > 5)
        {
            dashDir = target.transform.position - transform.position;

            // Move
            acceleration = (acceleration <= 15) ? (acceleration + 5f * Time.deltaTime) : acceleration;
            controller.Move(acceleration * Time.deltaTime * dashDir.normalized);
            timer -= Time.deltaTime;
            yield return null;
        }

        // Dash Delay
        yield return new WaitForSeconds(0.25f);

        // Chase End
        chaseType = ChaseType.None;
        state = State.Think;
        isLook = true;
        Attack_Think();
    }

    private IEnumerator Chase_Timer(float time)
    {
        chaseTimer = time;
        while(chaseTimer > 0 && state == State.Chase)
        {
            chaseTimer -= Time.deltaTime;
            yield return null;
        }

        if(chaseTimer <= 0)
        {
            switch (chaseType)
            {
                case ChaseType.Walk:
                    StartCoroutine(Chase_Timer(3f));
                    acceleration = 2.5f;
                    chaseType = ChaseType.Run;
                    break;

                case ChaseType.Run:
                    acceleration = 2.5f;
                    chaseType = ChaseType.Dash;
                    StartCoroutine(Dash_Chase());
                    break;
            }
        }
    }
    #endregion

    #region Attack Cycle
    // 사이클 1(콤보A,B -> 중거리 돌진 -> 검기)
    // 사이클 2(중거리 돌진 -> 중거리 레이저 -> 스트라이크)
    // 사이클 3(중거리 돌진 -> 연속 스트라이크 -> 스톰핑)
    // 사이클 4(원거리 돌진 -> 검기 -> 연속 스트라이크 -> 포워드 슬래쉬)
    // 사이클 5(중거리 돌진 -> 백스텝 슈팅 -> 중거리 레이저)
    // 사이클 6(콤보A,B -> 백스텝 슈팅 -> 장거리 레이저 -> 대점슬)
    // 사이클 7(스트라이크 -> 연속 스트라이크 -> 스톰핑)
    // 사이클 8(스트라이크 -> 업워드 슬래쉬-> 백스탭 -> 대점슬)
    // 사이클 9(스톰핑 -> 대점슬 -> 콤보A,B -> 포워드 슬래쉬)

    // 근거리 사이클
    // 사이클 1, 사이클 5, 사이클 6, 사이클 9

    // 중거리 사이클
    // 사이클 2, 사이클 3, 사이클 7, 사이클 8

    // 원거리 사이클
    // 사이클 4

    // 사이클 관리는 enum을 통해서 시전중 -> 종료 확인
    // 이동 후 스탭 사용 여부는 스킬에서 제어하는 것 대신 사이클에서 제어하도록 변경
    // 즉 딜레이 스텝 or 딜레이 무브의 발동조건에 isCycle을 추가하여 관리하고 있음!

    private IEnumerator CycleA()
    {
        // 사이클 1(콤보A,B -> 중거리 돌진 -> 검기)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 콤보 공격
        StartCoroutine(nameof(Combo_Attack));
        while(shortAttack == ShortAttack.Combo_Attack)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 중거리 돌진
        StartCoroutine(nameof(Charging_Thrust));
        while (mediumAttack == MediumAttack.Charging_Thrust)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 검기
        StartCoroutine(nameof(Sword_Aura));
        while (mediumAttack == MediumAttack.Sword_Aura)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        // 사이클 종료
        Cycle_End();
    }

    private IEnumerator CycleB()
    {
        // 사이클 2(중거리 돌진 -> 중거리 레이저 -> 스트라이크)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 중거리 돌진
        StartCoroutine(nameof(Charging_Thrust));
        while (mediumAttack == MediumAttack.Charging_Thrust)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 중거리 레이저
        StartCoroutine(nameof(Laser_Attack));
        while (mediumAttack == MediumAttack.Laser_Attack)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 스트라이크
        StartCoroutine(nameof(Strike_Attack));
        while(shortAttack == ShortAttack.Strike_Attack)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        // 사이클 종료
        StartCoroutine(nameof(Cycle_End));
    }

    private IEnumerator CycleC()
    {
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 사이클 3(중거리 돌진 -> 연속 스트라이크 -> 스톰핑)
        StartCoroutine(nameof(Charging_Thrust));
        while(mediumAttack == MediumAttack.Charging_Thrust)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 연속 스트라이크
        StartCoroutine(nameof(Continuous_Strike));
        while(mediumAttack == MediumAttack.Continuous_Strike)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 스톰핑
        StartCoroutine(nameof(Stomping_Attack));
        while(shortAttack == ShortAttack.Stomping)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        Cycle_End();
    }

    private IEnumerator CycleD()
    {
        // 사이클 4(원거리 돌진 -> 검기 -> 연속 스트라이크 -> 포워드 슬래쉬)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;


        // 원거리 돌진
        StartCoroutine(nameof(Charging_LongThrust));
        while (longAttack == LongAttack.Charging_Thrust)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 검기
        StartCoroutine(nameof(Sword_Aura));
        while (mediumAttack == MediumAttack.Sword_Aura)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 연속 스트라이크
        StartCoroutine(nameof(Continuous_Strike));
        while(mediumAttack == MediumAttack.Continuous_Strike)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 포워드 슬래쉬
        StartCoroutine(nameof(Forwardstep_Slash));
        while(shortAttack == ShortAttack.Forwardstep_Slash)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        Cycle_End();
    }

    private IEnumerator CycleE()
    {
        // 사이클 5(중거리 돌진 -> 백스텝 슈팅 -> 중거리 레이저)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 중거리 돌진
        StartCoroutine(nameof(Charging_Thrust));
        while(mediumAttack == MediumAttack.Charging_Thrust)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 백스텝 슈팅
        StartCoroutine(nameof(Backstep_shooting));
        while(shortAttack == ShortAttack.Backstep_shooting)
        {
            yield return null;
        }

        // 중거리 레이저
        StartCoroutine(nameof(Laser_Attack));
        while (mediumAttack == MediumAttack.Laser_Attack)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        Cycle_End();
    }

    private IEnumerator CycleF()
    {
        // 사이클 6(콤보A,B -> 백스텝 슈팅 -> 장거리 레이저 -> 대점슬)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 콤보 공격
        StartCoroutine(nameof(Combo_Attack));
        while(shortAttack == ShortAttack.Combo_Attack)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 백스텝 슈팅
        StartCoroutine(nameof(Backstep_shooting));
        while (shortAttack == ShortAttack.Backstep_shooting)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 장거리 레이저
        StartCoroutine(nameof(Laser_LongAttack));
        while (longAttack == LongAttack.Laser_Attack)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 대점슬
        StartCoroutine(nameof(DashJumpSlash));
        while(mediumAttack == MediumAttack.DashJumpSlash)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        Cycle_End();
    }

    private IEnumerator CycleG()
    {
        // 사이클 7(스트라이크 -> 연속 스트라이크 -> 스톰핑)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 스트라이크
        StartCoroutine(nameof(Strike_Attack));
        while(shortAttack == ShortAttack.Strike_Attack)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 연속 스트라이크
        StartCoroutine(nameof(Continuous_Strike));
        while (mediumAttack == MediumAttack.Continuous_Strike)
        {
            yield return null;
        }

        // 스톰핑
        StartCoroutine(nameof(Stomping_Attack));
        while(shortAttack == ShortAttack.Stomping)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        Cycle_End();
    }

    private IEnumerator CycleH()
    {
        // 사이클 8(스트라이크 -> 업워드 슬래쉬-> 백스탭 -> 대점슬)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 스트라이크
        StartCoroutine(nameof(Strike_Attack));
        while (shortAttack == ShortAttack.Strike_Attack)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 업워드 슬래쉬
        StartCoroutine(nameof(Upward_Slash));
        while(shortAttack == ShortAttack.Upward_Slash)
        {
            yield return null;
        }

        // 백스탭
        StartCoroutine(nameof(Backstep_shooting));
        while(shortAttack == ShortAttack.Backstep_shooting)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 대점슬
        StartCoroutine(nameof(DashJumpSlash));
        while(mediumAttack == MediumAttack.DashJumpSlash)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        Cycle_End();
    }

    private IEnumerator CycleI()
    {
        // 사이클 9(스톰핑 -> 대점슬 -> 콤보A,B -> 포워드 슬래쉬)
        state = State.Attack;
        isAttack = true;
        isCycle = true;
        int ran;

        // 스톰핑
        StartCoroutine(nameof(Stomping_Attack));
        while (shortAttack == ShortAttack.Stomping)
        {
            yield return null;
        }

        // 스텝
        ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // 스텝 딜레이
        while (isStartStep)
        {
            yield return null;
        }

        // 대점슬
        StartCoroutine(nameof(DashJumpSlash));
        while(mediumAttack == MediumAttack.DashJumpSlash)
        {
            yield return null;
        }

        // 콤보A,B
        StartCoroutine(nameof(Combo_Attack));
        while(shortAttack == ShortAttack.Combo_Attack)
        {
            yield return null;
        }

        // 포워드 슬래쉬
        StartCoroutine(nameof(Forwardstep_Slash));
        while (shortAttack == ShortAttack.Forwardstep_Slash)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(attackDelayB);

        Cycle_End();
    }

    private void Cycle_End()
    {
        // 사이클 종료 후 행동
        shortAttack = ShortAttack.None;
        mediumAttack = MediumAttack.None;
        longAttack = LongAttack.None;
        state = State.Attack_Delay;
        isStartStep = false;
        isAttack = false;
        isCycle = false;
        isLook = true;

        // Orbit Setting
        if(!orbitOn)
        {
            // Orbit On
            orbitOn = true;
            orbit_BackSet.SetActive(true);

            // Orbit VFX
            for (int i = 0; i < orbit_MoveVFX.Length; i++)
            {
                orbit_MoveVFX[i].SetActive(true);
            }
        }

        // Delay Move
        int ran = Random.Range(0, 100);
        StartCoroutine(ran <= 50 ? nameof(Delay_Move) : nameof(Delay_Step));
    }
    #endregion

    #region Attack
    private void Attack_Think()
    {
        state = State.Think;
        Percent_Setting(PreviousAction.Attack);
        TargetDistance_Setting();

        if(targetDistance <= 20)
        {
            // Call Short Attack Patterns
            attackType = AttackType.ShortRange;
            StartCoroutine(shortRange_patterns[WeightedRandomIndex(shortRange_Weights)]);
            Debug.Log(shortRange_patterns[WeightedRandomIndex(shortRange_Weights)]);
        }
        else if(targetDistance <= 40)
        {
            // Call Medium Attack Patterns
            attackType = AttackType.MediumRange;
            StartCoroutine(mediumRange_patterns[WeightedRandomIndex(mediumRange_Weights)]);
            Debug.Log(mediumRange_patterns[WeightedRandomIndex(mediumRange_Weights)]);
        }
        else
        {
            // Call Long Attack Patterns
            attackType = AttackType.LongRange;
            StartCoroutine(LongRange_patterns[WeightedRandomIndex(longRange_Weights)]);
            Debug.Log(LongRange_patterns[WeightedRandomIndex(longRange_Weights)]);
        }
    }

    private int WeightedRandomIndex(List<int> weights) 
    {
        // 가중치 랜덤 계산
        float totalWeight = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            totalWeight += weights[i];
        }

        float randomValue = Random.value * totalWeight;
        for (int i = 0; i < weights.Count; i++)
        {
            if (randomValue < weights[i])
            {
                return i;
            }
            randomValue -= weights[i];
        }
        Debug.Log(weights.Count - 1);
        return weights.Count - 1;
    }

    #region ShortRange Attack
    private IEnumerator Combo_Attack()
    {
        shortAttack = ShortAttack.Combo_Attack;
        state = State.Attack;
        isAttack = true;
        isLook = true;
        curCombo = 0;

        // Step
        Vector3 targetDir = target.transform.position - transform.position;
        if(targetDir.magnitude > 5)
        {
            StartCoroutine(Start_Step(StepType.Forward));
        }
        else
        {
            int ran = Random.Range(0, 100);
            if(ran <= 50)
            {
                StartCoroutine(Start_Step(StepType.Left));
            }
            else
            {
                StartCoroutine(Start_Step(StepType.Right));
            }
        }

        // Step Delay
        while(isStartStep)
        {
            yield return null;
        }
        isLook = false;

        // Target Look -> HardLook
        float timer = 0.15f;
        while (timer > 0)
        {
            HardLook();
            timer -= Time.deltaTime;
            yield return null;
        }

        // Attack Type Setting
        // ( A : 3 Combo Attack / B : 4 Combo Attack )
        int count = (Random.Range(0, 100) <= 50) ? 3 : 4;
        for (int i = 0; i < count; i++)
        {
            anim.SetTrigger(count == 3 ? "ComboA" : "ComboB");
            anim.SetBool("isComboAttack", true);
            while (anim.GetBool("isComboAttack"))
            {
                yield return null;
            }
            curCombo++;

            // Delay
            if(curCombo <= count)
            {
                // Delay
                yield return new WaitForSeconds(0.35f);

                // Target FastLook
                timer = 0.15f;
                while (timer > 0)
                {
                    HardLook();
                    timer -= Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                // End Delay
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Attack End
        shortAttack = ShortAttack.None;
        state = State.Attack_Delay;
        isAttack = false;

        // Backstep || Delay Move
        if(!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    public void Combo_Move()
    {
        StartCoroutine(nameof(ComboMove));
    }

    public void ComboA_Collider()
    {
        // Sound
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.ComboA, curCombo);

        if (curCombo == 0)
        {
            comboAVFX[0].SetActive(true);
            StartCoroutine(AttackColliderOnOff(comboA1, 0.08f, 0.05f));
        }
        else if(curCombo == 1)
        {
            comboAVFX[1].SetActive(true);
            StartCoroutine(AttackColliderOnOff(comboA2, 0.07f, 0.05f));
        }
        else
        {
            comboAVFX[2].SetActive(true);
            StartCoroutine(AttackColliderOnOff(comboA3, 0.05f, 0.05f));
        }
    }

    public void ComboB_Collider()
    {
        // Sound
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.ComboB, curCombo);

        if (curCombo == 0)
        {
            comboBVFX[0].SetActive(true);
            StartCoroutine(AttackColliderOnOff(comboB1, 0.08f, 0.05f));
        }
        else if (curCombo == 1)
        {
            comboBVFX[1].SetActive(true);
            StartCoroutine(AttackColliderOnOff(comboB2, 0.07f, 0.05f));
        }
        else if(curCombo == 2)
        {
            comboBVFX[2].SetActive(true);
            StartCoroutine(AttackColliderOnOff(comboB3, 0.07f, 0.05f));
        }
        else
        {
            comboBVFX[3].SetActive(true);
            StartCoroutine(AttackColliderOnOff(comboB4, 0.07f, 0.05f));
        }
    }

    private IEnumerator AttackColliderOnOff(GameObject[] colliders, float time, float endTime)
    {
        // Attack Start
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].SetActive(true);
            yield return new WaitForSeconds(time);
            colliders[i].SetActive(false);
        }

        // Delay
        yield return new WaitForSeconds(endTime);

        /*
        // Attack End
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].SetActive(false);
        }
        */
    }

    private IEnumerator ComboMove()
    {
        // Collider Setting
        Ignore_PlayerCollider(true);

        // Move
        acceleration = 50f;
        float timer = 0;
        while (timer < 1 && anim.GetBool("isComboAttack"))
        {
            moveDir.y = 0;
            acceleration = (acceleration > 0) ? (acceleration - 300f * Time.deltaTime) : acceleration = 1;
            controller.Move(acceleration * Time.deltaTime * moveDir);
            timer += Time.deltaTime * 2f;
            yield return null;
        }

        // Collider Setting
        Ignore_PlayerCollider(false);
    }

    private IEnumerator Stomping_Attack()
    {
        shortAttack = ShortAttack.Stomping;
        state = State.Attack;
        isAttack = true;
        isLook = false;

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("stompingCharge", true);
        anim.SetBool("isStomping", true);
        anim.SetFloat("StompingTime", 0);

        // Charge
        float timer = 0;
        float a = 1;
        while (timer < 1)
        {
            anim.SetFloat("StompingTime", timer);
            timer += Time.deltaTime * a; // 1초 가량
            yield return null;
        }
        isLook = false;
        anim.SetFloat("StompingTime", 1);
        anim.SetBool("stompingCharge", false);

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        // Animation Delay
        while (anim.GetBool("isStomping") || isExplsion)
        {
            yield return null;
        }

        // Effect Delay -> 상황 보고 조절할 것!
        yield return new WaitForSeconds(0.25f);

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        shortAttack = ShortAttack.None;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if (!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    public void Stomping_Collider()
    {
        stompingVFX.SetActive(true);
        StartCoroutine(nameof(StompingCollider));
    }

    private IEnumerator StompingCollider()
    {
        isExplsion = true;
        yield return StartCoroutine(StompingOn(stomping_ColliderA));

        yield return StartCoroutine(StompingOn(stomping_ColliderB));

        yield return StartCoroutine(StompingOn(stomping_ColliderC));

        yield return StartCoroutine(StompingOn(stomping_ColliderD));

        yield return StartCoroutine(StompingOn(stomping_ColliderE));

        // Delay
        yield return new WaitForSeconds(0.5f);
        isExplsion = false;
    }

    private IEnumerator StompingOn(GameObject[] obj)
    {
        // Sound
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Stomping, 0);

        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(true);
        }

        yield return new WaitForSeconds(0.15f);
    }

    private IEnumerator Strike_Attack()
    {
        shortAttack = ShortAttack.Strike_Attack;
        state = State.Attack;
        isAttack = true;
        isLook = true;

        anim.SetTrigger("Attack");
        anim.SetBool("Strke_Charging", true);
        anim.SetBool("isStrkeMove", true);
        anim.SetBool("isStrike", true);

        // Sound
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Strike, 0);

        // VFX
        strikeChargeVFX.SetActive(true);
        StartCoroutine(SwordOnOff(true));

        // Attack Charge
        float timer = 1f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetBool("Strke_Charging", false);

        // Collider Setting
        Ignore_PlayerCollider(true);

        // Sound
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Strike, 1);

        // VFX
        wingVFX.SetActive(true);
        strikeChargeVFX.SetActive(false);
        Instantiate(strikeMoveVFX, new Vector3(transform.position.x, transform.position.y -1.5f, transform.position.z - 0.5f), transform.rotation);

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        // Move
        isLook = false;
        Ignore_PlayerCollider(false);
        Vector3 targetDir = target.transform.position - transform.position;
        moveDir = target.transform.position - transform.position;
        moveDir.y = 0;
        acceleration = 10;
        while (anim.GetBool("isStrkeMove"))
        {
            // Dir Check
            targetDir = target.transform.position - transform.position;
            if(targetDir.magnitude < 5)
            {
                acceleration = 1;
            }

            // Move
            acceleration = (acceleration > 5) ? (acceleration - 15 * Time.deltaTime) : acceleration;
            controller.Move(acceleration * status.MoveSpeed * Time.deltaTime * moveDir.normalized);
            yield return null;
        }

        // Animation Delay
        StartCoroutine(SwordOnOff(false));
        Ignore_PlayerCollider(true);
        while (anim.GetBool("isStrike") && isExplsion)
        {
            yield return null;
        }

        // Collider Setting
        Ignore_PlayerCollider(false);

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        shortAttack = ShortAttack.None;
        state = State.Attack_Delay;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if (!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    public void Strike_Collider()
    {
        wingVFX.SetActive(false);
        StartCoroutine(nameof(StrikeCollider));
    }

    private IEnumerator StrikeCollider()
    {
        isExplsion = true;

        // Sound
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Strike, 1);

        // Start Explsion
        for (int i = 0; i < strike_ColliderA.Length; i++)
        {
            strike_ColliderA[i].SetActive(true);
        }

        yield return new WaitForSeconds(0.35f);

        int start = 0;
        int end = 4;
        // count = 폭팔 횟수 -> 5회로 상정
        for (int count = 0; count < 4; count++)
        {
            // Sound
            sound.ShortSound_Call(Sound_TypeRed.ShortSound.Strike, 1);

            // Explsion ON
            for (int i = start; i < end; i++)
            {
                strike_ColliderB[i].SetActive(true);
            }

            yield return new WaitForSeconds(0.15f);

            // Add
            start += 4;
            end += 4;
        }

        // Delay
        yield return new WaitForSeconds(0.15f);

        isExplsion = false;
    }

    private IEnumerator Backstep_shooting()
    {
        shortAttack = ShortAttack.Backstep_shooting;
        state = State.Attack;
        isAttack = true;
        isLook = true;

        // Move Setting
        moveDir = (stepPos[2].position - transform.position).normalized;
        moveDir.y = 0;
        acceleration = 30f;

        // Collider Setting
        Ignore_PlayerCollider(true);

        // Move
        anim.SetTrigger("Backstep");
        anim.SetBool("isStep", true);

        float timer = 1f;
        while(timer > 0)
        {
            if(acceleration > 0)
            {
                acceleration -= 30f * Time.deltaTime;
            }

            controller.Move(moveDir * acceleration * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        Ignore_PlayerCollider(false);

        // Animation Wait
        while(anim.GetBool("isStep"))
        {
            yield return null;
        }

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        // Spawn + Attack
        StartCoroutine(nameof(Origo_Bullet_Spawn));
        while (bulletSpawn)
        {
            // ShotPos Setting
            attackRange.transform.position = new Vector3(target.transform.position.x, -1f, target.transform.position.z);

            // Lookat
            TargetDistance_Setting();
            yield return null;
        }

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        shortAttack = ShortAttack.None;
        isAttack = false;
        isLook = true;
        attackCount++;
    }

    private IEnumerator Guard()
    {
        shortAttack = ShortAttack.Guard;
        state = State.Attack;
        isAttack = true;

        // Sound On
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Guard, 0);

        // Guard
        guard_Collider.SetActive(true);
        anim.SetTrigger("Attack");
        anim.SetBool("isGuard", true);
        float timer = 2f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isGuard", false);
        guard_Collider.SetActive(false);

        // Attack Delay B
        yield return new WaitForSeconds(attackDelayB);

        // Attack End
        shortAttack = ShortAttack.None;
        state = State.Attack_Delay;
        isAttack = false;

        // Backstep || Delay Move
        if (attackCount > 3)
        {
            // Step Setting
            StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
        }
        else
        {
            attackCount++;
            StartCoroutine(nameof(Delay_Move));
        }
    }

    public void Guard_Hit()
    {
        Debug.Log("Call1");
        // Animation Event 에서 호출 할 것! -> 근데 그러려면 결국 스크립트 분할을 해야하는게?
        // 최종적으론 Animation Event 대신 가드 관련 스크립트에서 호출하는 거로 변경
        // 기존 데미지 시스템이 잡몹도 사용하는거라 데미지 부분에서 재활용이 불가!

        // 23년 11월 25일 기준으로 위에 말이 뭔소린지 모르겠다 ㅈ됨 ㄷㄷ;;
        StopCoroutine("Guard");
        StartCoroutine(nameof(Guard_Attack));
    }

    private IEnumerator Guard_Attack()
    {
        anim.SetBool("isGuard", false);
        shortAttack = ShortAttack.Guard;
        state = State.Attack;
        isAttack = true;

        // LookAt
        float timer = 0.1f;
        while (timer > 0)
        {
            HardLook();
            timer -= Time.deltaTime;
            yield return null;
        }

        // Attack
        anim.SetTrigger("GuardAttack");
        anim.SetBool("isGuardAttack", true);

        // Animation Delay
        while(anim.GetBool("isGuardAttack"))
        {
            yield return null;
        }

        // Attack End
        shortAttack = ShortAttack.None;
        state = State.Idle;
        isAttack = false;
        attackCount++;
    }

    public void GuardAttackCollider()
    {
        // Sound On
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Guard, 0);

        guardVFX.SetActive(true);
        StartCoroutine(AttackColliderOnOff(guardAttack_Collider, 0.07f, 0.05f));
    }

    private IEnumerator Upward_Slash()
    {
        shortAttack = ShortAttack.Upward_Slash;
        state = State.Attack;
        isAttack = true;
        isLook = false;

        // Sound On
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Upward, 0);

        // Step
        Vector3 targetDir = target.transform.position - transform.position;
        Debug.Log(targetDir.magnitude);
        int ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // Delay
        while (isStartStep)
        {
            yield return null;
        }

        anim.SetTrigger("Attack");
        anim.SetBool("isUpwardAttack", true);
        anim.SetBool("Upward_Charging", true);

        // Charge
        float timer = 1.8f;
        while(timer > 0)
        {
            HardLook();
            timer -= Time.deltaTime;
            yield return null;
        }

        // Attack
        anim.SetBool("Upward_Charging", false);
        while(anim.GetBool("isUpwardAttack"))
        {
            yield return null;
        }

        // Attack Delay B
        yield return new WaitForSeconds(attackDelayB);

        // Attack End
        shortAttack = ShortAttack.None;
        state = State.Attack_Delay;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if (attackCount > 3)
        {
            // Step Setting
            StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
        }
        else
        {
            attackCount++;
            StartCoroutine(nameof(Delay_Move));
        }
    }

    public void Upward_Collider()
    {
        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        StartCoroutine(nameof(UpwardCollider));
    }

    private IEnumerator UpwardCollider()
    {
        // Slash VFX
        upwardVFX.SetActive(true);

        isExplsion = true;
        // Sound On
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Upward, 1);

        upwardVFX.SetActive(true);
        for (int i = 0; i < upward_Collider.Length; i++)
        {
            // Sound On
            sound.ShortSound_Call(Sound_TypeRed.ShortSound.Upward, 2);

            upward_Collider[i].SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
        isExplsion = false;
    }

    private IEnumerator Forwardstep_Slash()
    {
        shortAttack = ShortAttack.Forwardstep_Slash;
        state = State.Attack;
        isAttack = true;
        isLook = false;

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isForwardSlash", true);
        anim.SetBool("isForwardCharge", true);
        anim.SetBool("isForwardMove", true);

        // Sound On
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Forward, 0);

        // Move
        Ignore_PlayerCollider(true);
        moveDir = (target.transform.position - transform.position).normalized;
        moveDir.y = 0;
        acceleration = 10;
        float timer = 1f;
        while (timer > 0)
        {
            acceleration = acceleration > 0 ? acceleration -= 30f * Time.deltaTime : 0;
            controller.Move(acceleration * status.MoveSpeed * Time.deltaTime * moveDir);
            timer -= Time.deltaTime;
            yield return null;
        }

        // Animation Delay
        while (anim.GetBool("isForwardMove"))
        {
            yield return null;
        }
        Ignore_PlayerCollider(false);

        // Attack Charge
        isLook = true;
        float a = 1;
        wingVFX.SetActive(true);
        anim.SetFloat("AttackSpeed", a);
        while(anim.GetBool("isForwardCharge"))
        {
            a = (a > 0.5f) ? (a -= 2.5f * Time.deltaTime) : 0.5f;
            anim.SetFloat("AttackSpeed", a);
            yield return null;
        }
        isLook = false;

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        // Delay
        while (anim.GetBool("isForwardSlash"))
        {
            yield return null;
        }

        // Attack End
        shortAttack = ShortAttack.None;
        state = State.Attack_Delay;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if (!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    public void Forward_Collider()
    {
        // Sound On
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Forward, 1);

        // VFX
        wingVFX.SetActive(false);
        forwardVFX.SetActive(true);
        StartCoroutine(AttackColliderOnOff(forward_Collider, 0.04f, 0.05f));

        // Sword Aura Attack
        GameObject obj = Instantiate(swordAuraBullet, transform.position, Quaternion.identity);
        Vector3 dir = (target.transform.position - obj.transform.position).normalized;
        obj.GetComponent<Boss_SwordAura>().Setting(status.Damage, status.ArmorPenetration, dir);
    }
    #endregion

    #region MediumRange Attack
    private IEnumerator Sword_Aura()
    {
        mediumAttack = MediumAttack.Sword_Aura;
        state = State.Attack;
        isAttack = true;
        isLook = true;

        anim.SetTrigger("Attack");
        anim.SetBool("isSwordAura", true);
        anim.SetBool("isSwordAuraCharge", true);

        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.SwordAura, 0);

        // Charge
        float timer = 1f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        isLook = false;
        anim.SetBool("isSwordAuraCharge", false);

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_Focus));
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        // Delay
        while (anim.GetBool("isSwordAura"))
        {
            yield return null;
        }

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        mediumAttack = MediumAttack.None;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if(!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    public void SwordAura_Collider()
    {
        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.SwordAura, 1);

        // Attack
        swordAuraVFX.SetActive(true);
        StartCoroutine(AttackColliderOnOff(swordAura_Collider, 0.05f, 0.2f));

        // Sword Aura Shotting
        GameObject obj = Instantiate(swordAuraBullet, SwordAura_ShotPos.position, Quaternion.identity);
        Vector3 shotDir = (target.transform.position - obj.transform.position).normalized;
        shotDir.y = 0;
        obj.GetComponent<Boss_SwordAura>().Setting((int)(status.Damage * 1.1f), status.ArmorPenetration, shotDir);
    }

    private IEnumerator DashJumpSlash()
    {
        mediumAttack = MediumAttack.DashJumpSlash;
        state = State.Attack;
        isAttack = true;
        isLook = false;

        StartCoroutine(Start_Step(StepType.Forward));
        while(isStartStep)
        {
            yield return null;
        }

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isDJSlash", true);
        anim.SetBool("isDJSlashMove", true);
        anim.SetFloat("DJ", 0.25f);

        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.DJ, 0);

        // Attack & Move
        wingVFX.SetActive(true);
        Ignore_PlayerCollider(true);
        moveDir = target.transform.position - transform.position;
        moveDir.y = 0;
        acceleration = 15;
        float timer = 0.25f;
        while (timer < 1f) 
        {
            HardLook();
            // 3초동안 이동할 예정 -> 타이머의 경우 애니메이션의 진행 정도를 0~1로 나타내는 것
            // 실제 타이머는 Time.deltaTime / 3f로 계산되어 1초가 3초로 취급됨!
            moveDir = target.transform.position - transform.position;
            moveDir.y = 0;

            // Animation Speed Setting
            timer += (moveDir.magnitude > 5) ? (Time.deltaTime / 1.5f) : (Time.deltaTime * 3f);
            anim.SetFloat("DJ", timer);

            // Move
            acceleration = acceleration > 3 ? acceleration -= Time.deltaTime : 3;
            if(moveDir.magnitude > 3)
            {
                controller.Move(acceleration * status.MoveSpeed * Time.deltaTime * moveDir.normalized);
            }
            yield return null;
        }
        anim.SetFloat("DJ", 1f);

        // Animation Wait
        while (anim.GetBool("isDJSlash"))
        {
            yield return null;
        }
        wingVFX.SetActive(false);
        Ignore_PlayerCollider(false);

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        mediumAttack = MediumAttack.None;
        state = State.Attack_Delay;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if(!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    public void DJAttackCall()
    {
        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.DJ, 1);

        djVFX.SetActive(true);
        StartCoroutine(AttackColliderOnOff(dj_Collider, 0.05f, 0.15f));
    }

    private IEnumerator Continuous_Strike()
    {
        mediumAttack = MediumAttack.Continuous_Strike;
        state = State.Attack;
        isAttack = true;
        isLook = false;

        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.Continuous, 0);

        // Charge
        float timer = 0.15f;
        while(timer > 0)
        {
            HardLook();
            timer -= Time.deltaTime;
            yield return null;
        }

        // Attack
        for (int i = 0; i < 2; i++)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("isContinuousMove", true);
            anim.SetBool("isContinuousStrike", true);

            // Sound On
            sound.MediumSound_Call(Sound_TypeRed.MediumSound.Continuous, 0);

            // VFX
            Instantiate(strikeMoveVFX, new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z - 0.5f), transform.rotation);
            
            // Attack & Move
            Ignore_PlayerCollider(true);
            moveDir = (target.transform.position - transform.position).normalized;
            moveDir.y = 0;
            acceleration = 10;

            // Animation Delay
            while (anim.GetBool("isContinuousStrike") || isExplsion)
            {
                controller.Move(Vector3.zero);
                yield return null;
            }
            Ignore_PlayerCollider(false);

            // Delay
            yield return new WaitForSeconds(0.35f);

            // Look Delay
            timer = 0.25f;
            while(timer > 0)
            {
                HardLook();
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        // Animation Delay
        while(anim.GetBool("isContinuousStrike"))
        {
            yield return null;
        }

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        mediumAttack = MediumAttack.None;
        isAttack = false;
        isLook = true;
    }

    public void Continuous_Move()
    {
        StartCoroutine(ContinuousMovement());
    }

    private IEnumerator ContinuousMovement()
    {
        Vector3 dir = target.transform.position - transform.position;
        while (anim.GetBool("isContinuousMove"))
        {
            // Dir Check
            dir = target.transform.position - transform.position;
            if (dir.magnitude < 5)
            {
                acceleration = 1;
            }

            if (acceleration > 5)
                acceleration -= 15 * Time.deltaTime;

            controller.Move(acceleration * status.MoveSpeed * Time.deltaTime * moveDir.normalized);
            yield return null;
        }
    }

    public void Continuous_ColliderA()
    {
        StartCoroutine(nameof(ContinuousAttackA));

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }
    }

    public void Continuous_ColliderB()
    {
        StartCoroutine(nameof(ContinuousAttackB));

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }
    }

    public IEnumerator ContinuousAttackA()
    {
        isExplsion = true;

        // Attack 1
        continuous_ColliderC.SetActive(true);
        yield return new WaitForSeconds(0.35f);

        // Attack 2
        int start = 0;
        int end = 4;
        // count = 폭팔 횟수 -> 3회로 상정
        for (int count = 0; count < 3; count++)
        {
            // Sound On
            sound.MediumSound_Call(Sound_TypeRed.MediumSound.Continuous, 1);

            // Explsion ON
            for (int i = start; i < end; i++)
            {
                continuous_ColliderA[i].SetActive(true);
            }

            yield return new WaitForSeconds(0.15f);

            // Add
            start += 4;
            end += 4;
        }

        // Delay
        yield return new WaitForSeconds(0.5f);

        isExplsion = false;
    }

    public IEnumerator ContinuousAttackB()
    {
        isExplsion = true;

        // Attack 1
        continuous_ColliderC.SetActive(true);
        yield return new WaitForSeconds(0.35f);

        // Attack 2
        int start = 0;
        int end = 4;
        // count = 폭팔 횟수 -> 5회로 상정
        for (int count = 0; count < 4; count++)
        {
            // Sound On
            sound.MediumSound_Call(Sound_TypeRed.MediumSound.Continuous, 1);

            // Explsion ON
            for (int i = start; i < end; i++)
            {
                continuous_ColliderB[i].SetActive(true);
            }

            yield return new WaitForSeconds(0.15f);

            // Add
            start += 4;
            end += 4;
        }

        // Delay
        yield return new WaitForSeconds(0.5f);

        isExplsion = false;
    }

    private IEnumerator Charging_Thrust()
    {
        mediumAttack = MediumAttack.Charging_Thrust;
        state = State.Attack;
        isAttack = true;
        isLook = false;

        anim.SetTrigger("Attack");
        anim.SetBool("isThrustCharging", true);
        anim.SetBool("isChargingThrustAttack", true);
        anim.SetBool("isChargingThrust", true);

        // Charge
        float timer = 1.5f;
        while(timer > 0)
        {
            HardLook();
            timer -= Time.deltaTime;
            yield return null;
        }

        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.Charging, 0);

        // Attack
        anim.SetBool("isThrustCharging", false);
        chargeingThrust_Collider.SetActive(true);
        Ignore_PlayerCollider(true);
        StartCoroutine(ChargeingThrust_Explsion(0.05f));

        // VFX
        chargingThrust_MoveVFX.SetActive(true);
        chargingThrust_AttackVFX.SetActive(true);

        // Move
        moveDir = (target.transform.position - transform.position).normalized;
        moveDir.y = 0;
        acceleration = 15f;
        timer = 0.25f;
        while(timer > 0)
        {
            controller.Move(acceleration * status.MoveSpeed * Time.deltaTime * moveDir);
            timer -= Time.deltaTime;
            yield return null;
        }
        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.Charging, 1);

        anim.SetBool("isChargingThrustAttack", false);
        chargeingThrust_Collider.SetActive(false);

        // Delay
        while (anim.GetBool("isChargingThrust"))
        {
            yield return null;
        }
        Ignore_PlayerCollider(false);

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        mediumAttack = MediumAttack.None;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if(!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    private IEnumerator ChargeingThrust_Explsion(float time)
    {
        // Explsion Pos Setting
        Vector3[] explsionPos = new Vector3[5];
        for (int i = 0; i < 5; i++)
        {
            explsionPos[i] = transform.position;
            yield return new WaitForSeconds(time);
        }

        // Explsion Delay
        yield return new WaitForSeconds(0.15f);

        // Explsion
        for (int i = 0; i < explsionPos.Length; i++)
        {
            GameObject obj = Instantiate(chargeingExplsion_Collider, explsionPos[i], Quaternion.identity);
            obj.transform.position = new Vector3(obj.transform.position.x, (explsionPos[i].y + 0.5f), obj.transform.position.z);
            obj.GetComponent<Enemy_AttackCollider>().enemy = this;

            // Next Explsion Delay
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator Laser_Attack()
    {
        mediumAttack = MediumAttack.Laser_Attack;
        state = State.Attack;
        isAttack = true;
        isLook = false;

        // Step
        Vector3 targetDir = target.transform.position - transform.position;
        Debug.Log(targetDir.magnitude);
        if (targetDir.magnitude > 20)
        {
            StartCoroutine(Start_Step(StepType.Forward));
        }
        else
        {
            int ran = Random.Range(0, 100);
            if (ran <= 50)
            {
                StartCoroutine(Start_Step(StepType.Left));
            }
            else
            {
                StartCoroutine(Start_Step(StepType.Right));
            }
        }

        // Delay
        while(isStartStep)
        {
            yield return null;
        }

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isLaserCharge", true);
        anim.SetBool("isLaserAttackF", true);
        anim.SetBool("isLaserAttack", true);

        // Laser Chargeing
        float timer = 0.5f;
        while(timer > 0)
        {
            HardLook();
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isLaserCharge", false);

        // Animation Wait
        while(anim.GetBool("isLaserAttackF"))
        {
            yield return null;
        }

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        // Delay
        yield return new WaitForSeconds(0.25f);

        // Sound
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.Laser, 1);

        // Attack Rotate 
        laser.SetActive(true);
        laser.GetComponent<Boss_Laser>().Rotate_Start(3f);
        timer = 3f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        laser.SetActive(false);
        anim.SetBool("isLaserAttack", false);

        // Attack End
        mediumAttack = MediumAttack.None;
        isAttack = false;
        isLook = true;

        // Backstep || Delay Move
        if (!isCycle)
        {
            if (attackCount > 3)
            {
                // Step Setting
                StartCoroutine(Random.Range(0, 100) <= 70 ? nameof(Delay_Step) : nameof(Delay_Move));
            }
            else
            {
                attackCount++;
                StartCoroutine(nameof(Delay_Move));
            }
        }
    }

    public void LaserForwardAttack()
    {
        // Sound On
        sound.MediumSound_Call(Sound_TypeRed.MediumSound.Laser, 0);

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_Focus));
        }

        // Attack Forward
        laser.SetActive(true);
        laser.GetComponent<Boss_Laser>().Forward_Setting();
    }

    public void LaserForwardEnd()
    {
        laser.SetActive(false);
    }

    #endregion

    #region LongRange Attack
    private IEnumerator Charging_LongThrust()
    {
        longAttack = LongAttack.Charging_Thrust;
        state = State.Attack;
        isAttack = true;
        isLook = false;
        useGravity = false;

        // Animation Setting
        anim.SetTrigger("Attack");
        anim.SetBool("isLongThrust", true);
        anim.SetBool("isLongThrustMove", true);
        anim.SetBool("isLongThrustCharge", true);

        // Charge
        longChargeingVFX.SetActive(true);
        float timer = 1f;
        while (timer > 0)
        {
            moveDir = (target.transform.position - transform.position).normalized;
            moveDir.y = 0;
            HardLook();

            timer -= Time.deltaTime;
            yield return null;
        }
        isLook = false;
        longChargeingVFX.SetActive(false);
        anim.SetBool("isLongThrustCharge", false);

        // Attack & Move
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(LongChargingTrust_Explsion(0.05f));
            longChargeingThrust_Collider.SetActive(true);
            Ignore_PlayerCollider(true);

            // VFX
            Instantiate(longChargeingAttackVFX, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z -1f), transform.rotation, transform);
            Instantiate(longChargeingMoveVFX, new Vector3(transform.position.x, transform.position.y -0.5f, transform.position.z), transform.rotation);

            // Sound On
            sound.LongSound_Call(Sound_TypeRed.LongSound.LongCharging, 0);

            // Move
            acceleration = 15f;
            timer = 0.25f;
            while (timer > 0)
            {
                acceleration = (acceleration > 5) ? acceleration -= Time.deltaTime : acceleration;
                controller.Move(acceleration * status.MoveSpeed * Time.deltaTime * moveDir);
                timer -= Time.deltaTime;
                yield return null;
            }
            longChargeingThrust_Collider.SetActive(false);

            // Sound On
            sound.LongSound_Call(Sound_TypeRed.LongSound.LongCharging, 1);

            // Attack Delay
            yield return new WaitForSeconds(0.25f);

            // Move Setting + LookAt
            moveDir = (target.transform.position - transform.position).normalized;
            moveDir.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            this.transform.rotation = targetRotation;
        }

        // Animation Delay
        anim.SetBool("isLongThrustMove", false);
        while(anim.GetBool("isLongThrust"))
        {
            yield return null;
        }
        Ignore_PlayerCollider(false);

        // Attack Delay B
        if (!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        longAttack = LongAttack.None;
        state = State.Idle;
        isAttack = false;
        isLook = true;
        useGravity = true;
    }

    private IEnumerator LongChargingTrust_Explsion(float time)
    {
        // Explsion Pos Setting
        Vector3[] explsionPos = new Vector3[10];
        for (int i = 0; i < 10; i++)
        {
            explsionPos[i] = transform.position;
            yield return new WaitForSeconds(time);
        }

        // Explsion
        for (int i = 0; i < explsionPos.Length; i++)
        {
            Instantiate(chargeingExplsion_Collider, new Vector3(explsionPos[i].x, explsionPos[i].y +1, explsionPos[i].z), Quaternion.identity);
            yield return new WaitForSeconds(0.15f);
        }
    }

    private IEnumerator Laser_LongAttack()
    {
        longAttack = LongAttack.Laser_Attack;
        state = State.Attack;
        isAttack = true;
        isLook = true;

        // Step
        Vector3 targetDir = target.transform.position - transform.position;
        Debug.Log(targetDir.magnitude);
        int ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            StartCoroutine(Start_Step(StepType.Left));
        }
        else
        {
            StartCoroutine(Start_Step(StepType.Right));
        }

        // Delay
        while (isStartStep)
        {
            yield return null;
        }

        // Animation
        anim.SetTrigger("Attack");
        anim.SetBool("LongLaserChargeing", true);
        anim.SetBool("isLongLaser", true);
        anim.SetFloat("LongLaser", 0);

        // Charge
        isLook = false;
        float timer = 0f;
        while(timer < 1)
        {
            HardLook();
            timer += Time.deltaTime / 1.5f;
            anim.SetFloat("LongLaser", timer);
            yield return null;
        }
        anim.SetFloat("LongLaser", 1);
        anim.SetBool("LongLaserChargeing", false);

        // Sound On
        sound.LongSound_Call(Sound_TypeRed.LongSound.LongLaser, 0);
        sound.LongSound_Call(Sound_TypeRed.LongSound.LongLaser, 1);
        
        // Laser Horizontal
        for (int i = 0; i < longLasers.Length; i++)
        {
            longLasers[i].SetActive(true);
            longLasers[i].GetComponent<Boss_LongLaser>().Rotate_Start(5f);
        }

        // Laser Vertical
        for (int i = 0; i < longLaser_Vertical.Length; i++)
        {
            longLaser_Vertical[i].SetActive(true);
            longLaser_Vertical[i].GetComponent<Boss_LongLaser_Vertical>().Setting(5f);
        }

        // Explsion
        StartCoroutine(nameof(LongLaser_Explsion));

        // 2Phase Attack Add
        if (phase == Phase.Phase2)
        {
            StartCoroutine(nameof(Orbit_Focus));
            StartCoroutine(nameof(Orbit_SpreadOut));
        }

        // Attack
        timer = 5f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // Laser Off
        for (int i = 0; i < longLasers.Length; i++)
        {
            longLasers[i].SetActive(false);
        }

        while(anim.GetBool("isLongLaser"))
        {
            yield return null;
        }

        // Attack Delay B
        if(!isCycle)
        {
            yield return new WaitForSeconds(attackDelayB);
        }

        // Attack End
        longAttack = LongAttack.None;
        state = State.Idle;
        isAttack = false;
        isLook = true;
        attackCount++;
    }

    private IEnumerator LongLaser_Explsion()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < longLaserExplsionL.Length; i++)
        {
            // Sound On
            sound.LongSound_Call(Sound_TypeRed.LongSound.LongLaser, 2);

            longLaserExplsionL[i].SetActive(true);
            longLaserExplsionR[i].SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
    }
    #endregion

    #region phase2 Add
    // 2페이즈 돌입 후 공격에 나오는 추가 공격

    // Orbit_SpreadOut
    // (180도 회전) -> 오비트 전개 후 179도 회전 레이저 공격

    // Orbit_Focus
    // (산개) -> 오비트 산개 후 일점 집중 레이저 공격 (원거리 공격의 후속타 / SwordAura, Laser)

    private void Orbit_SpreadOut()
    {
        // Orbit Off
        orbitOn = false;
        orbit_BackSet.SetActive(false);

        // Orbit VFX
        for (int i = 0; i < orbit_MoveVFX.Length; i++)
        {
            orbit_MoveVFX[i].SetActive(true);
        }

        // Sound On
        sound.DroneSound_Call(Sound_TypeRed.DroneSound.Move);

        // Orbit Move
        for (int i = 0; i < orbit_movePos.Length; i++)
        {
            GameObject obj = Instantiate(orbit_Prefab, orbit_movePos[i].position, orbit_movePos[i].rotation);
            TypeRad_Orbit obj2 = obj.GetComponent<TypeRad_Orbit>();
            obj2.sound = sound;
            obj2.Orbit_SpreadOut(this, target, Return_RandomPosition(orbitMoveObj, orbitMoveCollider), 1f);
        }
    }

    private void Orbit_Focus()
    {
        // Orbit Off
        orbitOn = false;
        orbit_BackSet.SetActive(false);

        // Orbit VFX
        for (int i = 0; i < orbit_MoveVFX.Length; i++)
        {
            orbit_MoveVFX[i].SetActive(true);
        }

        // Sound On
        sound.DroneSound_Call(Sound_TypeRed.DroneSound.Move);

        // Orbit Move
        for (int i = 0; i < orbit_StartPos.Length; i++)
        {
            GameObject obj = Instantiate(orbit_Prefab, orbit_StartPos[i].position, orbit_StartPos[i].transform.rotation);
            TypeRad_Orbit obj2 = obj.GetComponent<TypeRad_Orbit>();
            obj2.sound = sound;
            obj.GetComponent<TypeRad_Orbit>().Orbit_Focus(this, target, orbit_movePos[i].position, 1.5f);
        }
    }
    #endregion

    private IEnumerator SwordOnOff(bool isOn)
    {
        if(isOn == true)
        {
            float timer = 0;
            while (timer < 1)
            {
                timer += 3 * Time.deltaTime;
                float a = Mathf.Lerp(0, 100, timer);

                swordRenderer.SetBlendShapeWeight(0, a);
                swordRenderer.SetBlendShapeWeight(2, a);
                swordRenderer.SetBlendShapeWeight(6, a);
                yield return null;
            }

            swordRenderer.SetBlendShapeWeight(0, 100);
            swordRenderer.SetBlendShapeWeight(2, 100);
            swordRenderer.SetBlendShapeWeight(6, 100);

        }
        else
        {
            float timer = 0;
            while (timer < 1)
            {
                timer += 3 * Time.deltaTime;
                float a = Mathf.Lerp(100, 0, timer);

                swordRenderer.SetBlendShapeWeight(0, a);
                swordRenderer.SetBlendShapeWeight(2, a);
                swordRenderer.SetBlendShapeWeight(6, a);
                yield return null;
            }

            swordRenderer.SetBlendShapeWeight(0, 0);
            swordRenderer.SetBlendShapeWeight(2, 0);
            swordRenderer.SetBlendShapeWeight(6, 0);
        }
    }

    private IEnumerator Origo_Bullet_Spawn()
    {
        // Sound On
        sound.ShortSound_Call(Sound_TypeRed.ShortSound.Backstepshooting, 0);

        bulletSpawn = true;

        List<GameObject> list = new List<GameObject>();
        int ran = Random.Range(20, 30);
        for (int i = 0; i < ran; i++)
        {
            // Spawn
            GameObject bullet = Instantiate(origoBullet, Return_RandomPosition(spawnRange, spawnRangeCollider), Quaternion.identity);
            bullet.GetComponent<Enemy_BossBullet>().Bullet_MoveUp();
            list.Add(bullet);

            // Spawn Delay
            yield return new WaitForSeconds(0.025f);
        }

        for (int i = 0; i < list.Count; i++)
        {
            // list[i].GetComponent<Enemy_BossBullet>().Shot_TypeB(list[i].transform, Return_RandomPosition(attackRange, attackRangeCollider), 1, 6, 3);
            if(list[i] != null)
            {
                Vector3 dir = (Return_RandomPosition(attackRange, attackRangeCollider) - list[i].gameObject.transform.position).normalized;
                list[i].GetComponent<Enemy_BossBullet>().Shot_TypeA(Enemy_BossBullet.BulletType.TypeA, dir, 15, 90);
            }

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }

        bulletSpawn = false;
    }

    private Vector3 Return_RandomPosition(GameObject obj, Collider collider)
    {
        Vector3 originPosition = obj.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = collider.bounds.size.x;
        float range_Y = collider.bounds.size.y;
        float range_Z = collider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
        #endregion

    private void Phase2On()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(Phase2));
    }

    private IEnumerator Phase2()
    {
        // Cam VFX
        Effect_Manager.instance.Camera_Shake(15f, 0.1f);

        playerUI.SetActive(false);

        // Reset Boss
        BossReset();
        phase = Phase.Phase2;
        state = State.Groggy;
        isGroggy = true;
        isPhaseChange = true;
        anim.SetTrigger("Return");

        // Boss Change
        swordRenderer.material = swordMat;
        head.material = headMat;
        hair.material = hairMat;
        dress[0].material = dressMat;
        dress[1].material = dressMat;
        wing.material = wingMat;

        Player_Status.instance.StopPlayer(false);

        // Video On
        videoObj_Phase2.SetActive(true);
        videoPlayer_Phase2.Play();
        float timer = 11f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        videoPlayer_Phase2.Stop();
        videoObj_Phase2.SetActive(false);
        playerUI.SetActive(true);

        Player_Status.instance.StopPlayer(true);

        // Orbit VFX
        for (int i = 0; i < orbit_MoveVFX.Length; i++)
        {
            orbit_MoveVFX[i].SetActive(true);
        }
        orbit_BackSet.SetActive(true);

        // Dialog
        Stage_Manager.instance.Dialog_Call(3, Dialog_Manager.DialogType.TypeB, gameObject);

        // Animation
        anim.SetTrigger("Phase2");
        anim.SetBool("isPhase2", true);
        while(anim.GetBool("isPhase2"))
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.25f);

        // End Phase
        state = State.Idle;
        isPhaseChange = false;
        isGroggy = false;
        isLook = true;
    }

    protected override void Groggy()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(Groggy_Setting));
    }

    private IEnumerator Groggy_Setting()
    {
        // Cam Effect
        Effect_Manager.instance.Camera_Shake(15, 0.1f);

        // Boss Reset
        BossReset();
        state = State.Groggy;
        isGroggy = true;

        // Sound Setting
        sound.NormalLoopSound_Call(false);
        sound.NormalSound_Call(Sound_TypeRed.NormalSound.Groggy);

        // Animation Setting
        anim.SetTrigger("Groggy");
        anim.SetBool("isGroggy", true);
        anim.SetBool("isGroggyOver", true);

        // Groggy
        float timer = status.GroggyTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // Animation Setting
        anim.SetBool("isGroggy", false);
        while(anim.GetBool("isGroggyOver"))
        {
            yield return null;
        }

        // Drone On
        if(phase == Phase.Phase2)
        {
            orbit_BackSet.SetActive(true);
        }

        // End Groggy
        Percent_Setting(PreviousAction.Groggy);
        state = State.Idle;
        curSuperArmor = 0;
        isGroggy = false;
        isAttack = false;
    }

    private void BossReset()
    {
        // Status Reset
        shortAttack = ShortAttack.None;
        mediumAttack = MediumAttack.None;
        longAttack = LongAttack.None;
        isIvincible = false;
        isGroggy = false;
        isAttack = false;
        isLook = false;
        isCycle = false;
        isExplsion = false;
        bulletSpawn = false;

        // Animation Reset
        for (int i = 0; i < animation_Triggers.Length; i++)
        {
            anim.ResetTrigger(animation_Triggers[i]);
        }
        for (int i = 0; i < animation_Bools.Length; i++)
        {
            anim.SetBool(animation_Bools[i], false);
        }

        // Attack Collider Off
        for (int i = 0; i < attackColliders.Length; i++)
        {
            attackColliders[i].SetActive(false);
        }

        // Reset Collider Setting & Sword+Wing VFX
        Ignore_PlayerCollider(true);
        wingVFX.SetActive(false);
        StartCoroutine(SwordOnOff(false));
    }

    private IEnumerator Delay_Move()
    {
        // Setting FSM to Attack Delay
        state = State.Attack_Delay;
        isAttack = false;
        Debug.Log("Call Delay Move");

        // Attack Delay Move (Right or Left)
        anim.SetBool("isDelayMove", true);
        float time = attackDelayB * 3f;
        int ran = Random.Range(0, 100);
        if (ran < 50)
        {
            while (time > 0)
            {
                //Vector3 move = (transform.right + (-transform.forward)).normalized;
                //controller.Move((status.MoveSpeed * 0.25f) * Time.deltaTime * move);
                //anim.SetFloat("MoveZ", move.z);
                //anim.SetFloat("MoveX", move.x);

                anim.SetFloat("MoveX", 1);
                anim.SetFloat("MoveZ", -1);
                controller.Move((status.MoveSpeed * 0.25f) * Time.deltaTime * transform.right);
                controller.Move((status.MoveSpeed * 0.15f) * Time.deltaTime * -transform.forward);

                time -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (time > 0)
            {
                //Vector3 move = ((-transform.right) + (-transform.forward)).normalized;
                //controller.Move((status.MoveSpeed * 0.25f) * Time.deltaTime * move);
                //anim.SetFloat("MoveZ", move.z);
                //anim.SetFloat("MoveX", move.x);
                anim.SetFloat("MoveX", -1);
                anim.SetFloat("MoveZ", -1);

                controller.Move((status.MoveSpeed * 0.25f) * Time.deltaTime * -transform.right);
                controller.Move((status.MoveSpeed * 0.15f) * Time.deltaTime * -transform.forward);

                time -= Time.deltaTime;
                yield return null;
            }
        }

        // Animation Delay
        float timer1 = anim.GetFloat("MoveZ");
        float timer2 = anim.GetFloat("MoveX");
        while (timer1 > 0 && timer2 > 0)
        {
            timer1 -= 0.75f * Time.deltaTime;
            timer2 -= 0.75f * Time.deltaTime;
            anim.SetFloat("MoveZ", timer1);
            anim.SetFloat("MoveX", timer2);
            yield return null;
        }
        anim.SetFloat("MoveZ", 0);
        anim.SetFloat("MoveX", 0);
        anim.SetBool("isDelayMove", false);

        // Move End
        attackType = AttackType.None;
        shortAttack = ShortAttack.None;
        mediumAttack = MediumAttack.None;
        longAttack = LongAttack.None;
        state = State.Idle;
    }

    private IEnumerator Delay_Step()
    {
        // Step Dir Setting
        Debug.Log("Call Delay Move");
        int ran = Random.Range(0, 100);
        if (ran <= 50)
        {
            stepType = StepType.Left;
            stepDir = (stepPos[0].position - transform.position).normalized;
            anim.SetInteger("StepType", 1);
        }
        else
        {
            stepType = StepType.Right;
            stepDir = (stepPos[1].position - transform.position).normalized;
            anim.SetInteger("StepType", 2);
        }

        stepDir.y = 0;
        anim.SetTrigger("Step");
        anim.SetBool("isStep", true);

        // Collider Setting
        Ignore_PlayerCollider(true);

        // Step Move
        acceleration = 60f;
        float timer = 0;
        while (timer < 1)
        {
            acceleration = (acceleration > 1) ? (acceleration - 150f * Time.deltaTime) : acceleration = 1;
            controller.Move(acceleration * Time.deltaTime * stepDir);
            timer += Time.deltaTime;
            yield return null;
        }

        // Animation Delay
        while (anim.GetBool("isStep"))
        {
            yield return null;
        }

        // Collider Setting
        Ignore_PlayerCollider(false);

        // Step End
        attackType = AttackType.None;
        shortAttack = ShortAttack.None;
        mediumAttack = MediumAttack.None;
        longAttack = LongAttack.None;
        state = State.Idle;
        isAttack = false;
    }

    private IEnumerator Start_Step(StepType type)
    {
        isStartStep = true;
        isLook = false;
 
        // Animation
        anim.SetTrigger("Step");
        anim.SetBool("isStep", true);
        anim.SetInteger("StepType", (int)type);

        // Step VFX
        switch (type)
        {
            case StepType.Left:
                stepMoveVFX[1].SetActive(true);
                break;

            case StepType.Right:
                stepMoveVFX[2].SetActive(true);
                break;

            case StepType.Forward:
                stepMoveVFX[0].SetActive(true);
                break;
        }

        // Collider Setting
        Ignore_PlayerCollider(true);

        // Step Move
        moveDir = (stepPos[(int)type - 1].position - transform.position).normalized;
        moveDir.y = 0;
        acceleration = 60f;
        float timer = 0;
        while (timer < 1)
        {
            acceleration = (acceleration > 0) ? (acceleration - 150f * Time.deltaTime) : acceleration = 1;
            controller.Move(acceleration * Time.deltaTime * moveDir);
            timer += Time.deltaTime;
            yield return null;
        }

        // Animation Delay
        while (anim.GetBool("isStep"))
        {
            yield return null;
        }

        // Collider Setting
        Ignore_PlayerCollider(false);
        isStartStep = false;
    }

    private void Graivty_Setting()
    {
        isGround = Physics.Raycast(transform.position, -transform.up, controller.bounds.extents.y + 0.01f, ground);
        if (!isGround && useGravity)
        {
            if(gravityWegiht < maxGravityWegiht)
            {
                gravityWegiht += Time.deltaTime;
            }

            controller.Move(9.81f * gravityWegiht * Time.deltaTime * Vector3.down);
        }

        if(isGround)
        {
            gravityWegiht = 3f;
        }
    }

    protected override IEnumerator Spawn()
    {
        Player_Status.instance.StopPlayer(false);

        state = State.Spawn;
        isLook = false;
        
        // Video On -> Stage_Base로 이동함!
        videoObj_Spawn.SetActive(true);
        videoPlayer_Spawn.Play();

        float timer = 20;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        
        videoPlayer_Spawn.Stop();
        fadeObj.SetActive(true);

        Player_Status.instance.StopPlayer(true);

        // Fade In -> Stage_Base로 이동함!
        float a1 = 0;
        while(a1 < 1)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, a1);
            a1 += Time.deltaTime * 1.2f;
            yield return null;
        }

        // Delay
        videoObj_Spawn.SetActive(false);
        yield return new WaitForSeconds(0.25f);

        // Fade Out
        a1 = 1;
        while(a1 > 0)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, a1);
            a1 -= Time.deltaTime * 1.2f;
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(nameof(SpawnNameOn));

        // Animation
        anim.SetTrigger("Spawn");
        anim.SetBool("isSpawn", true);

        // Sound Off
        sound.NormalLoopSound_Call(true);

        // Animation
        while (anim.GetBool("isSpawn"))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
        shortAttack = ShortAttack.None;
        mediumAttack = MediumAttack.None;
        longAttack = LongAttack.None;
        state = State.Idle;
        isAttack = false;
        isCycle = false;
        isLook = true;
    }

    private IEnumerator SpawnNameOn()
    {
        isSoundOn = true;

        // 강조선 On
        float a = 0;
        while(a < 1)
        {
            a += Time.deltaTime;
            nameFadeImage.color = new Color(nameFadeImage.color.r, nameFadeImage.color.g, nameFadeImage.color.b, a);
            yield return null;
        }

        // Name Text On
        a = 0;
        while(a < 1)
        {
            a += Time.deltaTime;
            bossNameText.color = new Color(bossNameText.color.r, bossNameText.color.g, bossNameText.color.b, a);
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.25f);

        // Text Off
        a = 1;
        while(a > 0)
        {
            a -= Time.deltaTime;
            bossNameText.color = new Color(bossNameText.color.r, bossNameText.color.g, bossNameText.color.b, a);
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.15f);

        // 강조선 Off
        a = 1;
        while(a > 0)
        {
            nameFadeImage.color = new Color(nameFadeImage.color.r, nameFadeImage.color.g, nameFadeImage.color.b, a);
            a -= Time.deltaTime;
            yield return null;
        }

        fadeObj.SetActive(false);

        Stage_Manager.instance.Dialog_Call(1, Dialog_Manager.DialogType.TypeB, gameObject);
    }

    protected override void Die()
    {
        if(state == State.Die)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(nameof(DieCall));
    }

    private IEnumerator DieCall()
    {
        // Cam VFX
        Effect_Manager.instance.Camera_Shake(15, 0.1f);

        // Boss Reset
        state = State.Die;
        BossReset();
        anim.SetTrigger("Return");

        // Sound Setting
        sound.NormalLoopSound_Call(false);
        sound.NormalSound_Call(Sound_TypeRed.NormalSound.Die);

        // Animation Setting
        anim.SetTrigger("Die");
        anim.SetBool("isDie", true);
        anim.SetBool("isDieMove", true);

        // Dialog
        Stage_Manager.instance.Dialog_Call(6, Dialog_Manager.DialogType.TypeB, gameObject);

        // DieStep Move
        stepDir = (stepPos[2].position - transform.position).normalized;
        acceleration = 60f;
        float timer = 0;
        while (timer < 1)
        {
            acceleration = (acceleration > 1) ? (acceleration - 150f * Time.deltaTime) : acceleration = 1;
            controller.Move(acceleration * Time.deltaTime * stepDir);
            timer += Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isDieMove", false);

        while (Dialog_Manager.instance.isDialogPrint)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(0.25f);

        // End Video -> Stage_Base 로 이동함!
        StartCoroutine(nameof(DieVideo));
    }

    private IEnumerator DieVideo()
    {
        playerUI.SetActive(false);
        videoObj_End.SetActive(true);
        videoPlayer_End.Play();
        Player_Status.instance.StopPlayer(false);

        float timer = 43;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        playerUI.SetActive(true);
        videoPlayer_End.Stop();
        Player_Status.instance.StopPlayer(true);

        // Fade2
        fadeImage2.gameObject.SetActive(true);
        float a = 0;
        while(a < 1)
        {
            a += Time.deltaTime;
            fadeImage2.color = new Color(fadeImage2.color.r, fadeImage.color.g, fadeImage.color.b, a);
            yield return null;
        }

        videoObj_End.SetActive(false);

        // Delay
        yield return new WaitForSeconds(3f);

        // Next Scene Move
        SceneManager.LoadScene("End");

        //Stage_Manager.instance.Stage_End(Stage_Base.EndType.End, "End");
    }
}
