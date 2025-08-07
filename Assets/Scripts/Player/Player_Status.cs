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
    public float getDamaged; // hp�� ������

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

        #region �Ұ�
        canMove = true;
        isStaminaRecover = true;
        canEnforce = true;
        canNextAttack = true;
        canSkill = true;
        canUlt = true;
        canUsePotion = true;
        #endregion

        #region �÷��̾� ����
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
            StartCoroutine(UsePotion()); // C Ű�� ���� ���
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

        if(curHp > maxHp) //���� ü���� �ִ� ü�� �ʰ��� �ִ� ü������ ����
        {
            curHp = maxHp;
            getDamaged = (float)maxHp;
        }
    }
    public void TakeDamage(int damage, int armorPenetration) // �÷��̾� �ǰ� ������ ���
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

        int damaged = damage - (defense - armorPenetration); // ������ ���
        if (damaged > 0)
        {
            curHp -= damaged;
            Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Hit, 0);
            StartCoroutine(SetGetdamaged());

            if(curHp > 0)
            {
                StartCoroutine(nameof(Hit_Ivincible));
            }
            if(curHp <= 0) // ������ ��� �� ��� ���� Ȯ��
            {
                Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Die, 0);

                // ��� ���
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

    #region UI����
    IEnumerator SetGetdamaged() // ����ó�� hp�� ����
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
            Debug.Log("���� ���!");
            curPotionCount--;
            curHp += hpRecoveryAmount;
            getDamaged += hpRecoveryAmount;
            potionDelay = potionDelay_Max;
            StartCoroutine(CoolTime());
            playerEffectManager.OnHealEffect();
        }
        else
        {
            Debug.Log("���� ����!");
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
