using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Status : MonoBehaviour
{
    public static Player_Status instance;
    public Animator anim;
    private Player_Effect_Manager playerEffectManager;
    [SerializeField] private Player_UI player_UI;

    [Header("---Attack Status---")]
    public bool isAttack;
    public bool isSkill;
    public bool isGuardAttack;
    public bool isUlt;
    public bool isDash;
    public int damage;
    public int armorPenetration;
    public bool canNextAttack;
    public bool canSkill;
    public bool canUlt;

    [Header("---Guard Status---")]
    public bool isGuard;

    [Header("---Defense Status---")]
    public int curHp;
    public int maxHp;
    public int defense;
    public float getDamaged; // hp바 구현용

    [Header("---Utility Status---")]
    public float moveSpeed;
    public float attackSpeed;
    public float curSp;
    public float maxSp;
    public float curStamina;
    public float maxStamina;
    public float staminaRecovery;
    public float staminaRecoveryDelay;
    public bool isStaminaRecover;
    public bool isEnforce;
    public bool canEnforce;
    public bool chargingEnforce;
    public bool isIvincible;

    [Header("---Move Bool---")]
    public bool canMove;
    public bool isMove;
    public bool isStop;
    public bool isDead;
    public bool allStop;

    [Header("---Potion Status---")]
    public int curPotionCount;
    public int maxPotionCount;
    public int hpRecoveryAmount;
    [SerializeField] private float potionDelay;
    [SerializeField] private float potionDelay_Max;
    [SerializeField] private bool canUsePotion;
    public Text potionCountText;
    public Image potionDisable;

    public void Awake()
    {
        Application.targetFrameRate = 60;

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        anim = GetComponentInChildren<Animator>();
        playerEffectManager = GetComponentInChildren<Player_Effect_Manager>();
        // player_UI = GetComponentInChildren<Player_UI>();

        #region 불값
        canMove = true;
        isStaminaRecover = true;
        canEnforce = true;
        canNextAttack = true;
        canSkill = true;
        canUlt = true;
        canUsePotion = true;
        #endregion

        #region 플레이어 스탯
        armorPenetration = 30;
        damage = 100;
        defense = 2;
        maxHp = 3000;
        curHp = 3000;
        getDamaged = curHp;
        maxSp = 1000;
        curSp = 1000;
        maxStamina = 100f;
        curStamina = 80f;
        staminaRecovery = 10f;
        curPotionCount = 3;
        maxPotionCount = 3;
        hpRecoveryAmount = 1500;
        potionDelay_Max = 2f;
        #endregion
    }

    public void StopPlayer(bool inOn)
    {
        canMove = inOn;
        isStaminaRecover = inOn;
        canEnforce = inOn;
        canNextAttack = inOn;
        canSkill = inOn;
        canUlt = inOn;
        canUsePotion = inOn;
    }

    public void Update()
    {
        StatRecovery();
        SetStaminaRecoverBool();
        potionCountText.text = curPotionCount.ToString();

        if (Input.GetKeyDown(KeyCode.C) && canUsePotion)
        {
            StartCoroutine(UsePotion()); // C 키로 포션 사용
            //TakeDamage(9999, 0);
        }
        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(200, 0);
        }
        */
    }
    public void StatRecovery()
    {
        if(curStamina < maxStamina && isStaminaRecover)
        {
            curStamina += staminaRecovery * Time.deltaTime;
        }

        if(curHp > maxHp) //현재 체력이 최대 체력 초과시 최대 체력으로 고정
        {
            curHp = maxHp;
            getDamaged = (float)maxHp;
        }
    }
    public void TakeDamage(int damage, int armorPenetration) // 플레이어 피격 데미지 계산
    {
        if(isDead)
        {
            return;
        }

        if(isDash)
        {
            Effect_Manager.instance.Slow_Motion(1f);
            return;
        }

        if (isIvincible)
        {
            return;
        }

        // Hit Stop
        Effect_Manager.instance.Slow_Motion(0.3f);
        Effect_Manager.instance.Camera_Shake(5, 0.15f);

        int damaged = damage - (defense - armorPenetration); // 데미지 계산
        if (damaged > 0)
        {
            curHp -= damaged;
            Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Hit, 0);
            StartCoroutine(SetGetdamaged());

            if(curHp > 0)
            {
                StartCoroutine(nameof(Hit_Ivincible));
            }
            if(curHp <= 0) // 데미지 계산 후 사망 판정 확인
            {
                Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Die, 0);

                // 사망 기능
                curHp = 0;
                isDead = true;
                canMove = false;
                canEnforce = false;
                anim.SetBool("IsDead", true);

                Cursor.lockState = CursorLockMode.None;
                player_UI.DieCall();
            }
        }
    }

    protected IEnumerator Hit_Ivincible()
    {
        isIvincible = true;
        yield return new WaitForSeconds(0.1f);
        isIvincible = false;
    }

    #region UI관련
    IEnumerator SetGetdamaged() // 닼소처럼 hp바 구현
    {
        yield return new WaitForSeconds(1f);
        getDamaged = curHp;
    }

    public void SetStaminaRecoverBool()
    {
        staminaRecoveryDelay -= Time.deltaTime;
        if(staminaRecoveryDelay < 0)
        {
            isStaminaRecover = true;
        }
        else
        {
            isStaminaRecover = false;
        }
    }
    #endregion

    #region Potion
    IEnumerator UsePotion()
    {
        if (curPotionCount > 0)
        {
            Debug.Log("포션 사용!");
            curPotionCount--;
            curHp += hpRecoveryAmount;
            getDamaged += hpRecoveryAmount;
            potionDelay = potionDelay_Max;
            StartCoroutine(CoolTime());
            playerEffectManager.OnHealEffect();
        }
        else
        {
            Debug.Log("포션 부족!");
        }

        yield return null;
    }

    IEnumerator CoolTime()
    {
        canUsePotion = false;
        while (potionDelay > 0)
        {
            potionDelay -= Time.deltaTime;
            potionDisable.fillAmount = potionDelay / potionDelay_Max;
            yield return null;
        }
        canUsePotion = true;
    }

    public void Supply()
    {
        curPotionCount = maxPotionCount;
        curHp = maxHp;
        playerEffectManager.OnHealEffect();
    }
    #endregion
}
