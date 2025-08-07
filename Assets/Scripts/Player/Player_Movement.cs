using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSY.Tweening;

public class Player_Movement : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private PlayerController1D playerController;
    [SerializeField] private Animator anim;


    [Header("---Move Status---")]
    public float speed;
    public float dashSpeed;
    [SerializeField] float dashDelay;
    [SerializeField] bool isMove;
    [SerializeField] bool isStop;
    [SerializeField] bool canDash;
    Vector3 moveDir;
    Vector3 dashDir;
    public Vector3 camDir;

    [Header("---Camera Status---")]
    [SerializeField] float turnSmoothTime;
    float turnSmoothVelocity;

    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerController = GetComponentInChildren<PlayerController1D>();
        anim = GetComponentInChildren<Animator>();
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Move();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Player_Status.instance.curStamina >= 10)
        {
            StartCoroutine(Dash());
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(h, 0f, v).normalized;

        float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        camDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        dashDir = camDir;
        if(moveDir.magnitude == 0)
        {
            dashDir = transform.forward;
        }

        if (Player_Status.instance.isDash || Player_Status.instance.isGuard || !Player_Status.instance.canMove || Player_Status.instance.isDead || Player_Status.instance.isUlt || Player_Status.instance.allStop)
        {
            return;
        }

        //플레이어 정면 조절
        if (moveDir.magnitude != 0)
        {
            Player_Status.instance.isMove = true;
            Player_Status.instance.isStop = false;
            anim.SetBool("IsMove", true);
            anim.SetBool("IsStop", false);

            //Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Move, 0);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(speed * Time.deltaTime * camDir.normalized);
        }
        else
        {
            Player_Status.instance.isMove = false;
            Player_Status.instance.isStop = true;
            anim.SetBool("IsMove", false);
            anim.SetBool("IsStop", true);
            anim.SetTrigger("Stop");
            //Sound_PlayerSystem.instance.Move_SoundOff();

        }
    }

    IEnumerator Dash()
    {
        if(Player_Status.instance.isSkill || Player_Status.instance.isUlt || Player_Status.instance.chargingEnforce || Player_Status.instance.isDead || Player_Status.instance.allStop)
        {
            // 스킬, 궁극기, 광폭화 중 일땐 발동 X
            yield break;
        }

        Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Dash, 0);

        Player_Status.instance.isDash = true;
        Player_Status.instance.canMove = true;
        Player_Status.instance.isGuard = false;
        Player_Status.instance.isGuardAttack = false;
        Player_Status.instance.isIvincible = true;
        Player_Status.instance.curStamina -= 10;

        // Animation
        anim.SetBool("IsDash", true);
        anim.SetBool("IsStop", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsGuard", false);
        anim.SetBool("GuardSuccess", false);
        anim.SetTrigger("Dash");

        float timer = 0.3f;
        float dashMultiply = 25f;
        float canRotateTime = 0.01f;
        Player_Status.instance.staminaRecoveryDelay = 1.5f; // 1.5f뒤에 스태미나 회복 시작
        dashSpeed = 0f;
        dashDelay = 0.5f;
        StartCoroutine(nameof(CoolTime));

        while (timer > 0)
        {
            while(canRotateTime > 0)
            {
                transform.rotation = Quaternion.LookRotation(dashDir);
                canRotateTime -= Time.deltaTime;
                yield return null;
            }
            dashDir = transform.forward;
            timer -= Time.deltaTime;
            dashSpeed += 3f * Time.deltaTime;
            controller.Move(EasingFunctions.InOutExpo(dashSpeed) * dashMultiply * Time.deltaTime * dashDir.normalized);
            yield return null;
        }

        Player_Status.instance.isDash = false;
        Player_Status.instance.isIvincible = false;
        anim.SetBool("IsDash", false);
        anim.SetBool("IsStop", true);
    }

    IEnumerator CoolTime()
    {
        canDash = false;
        while (dashDelay > 0)
        {
            dashDelay -= Time.deltaTime;
            yield return null;
        }
        canDash = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && Player_Status.instance.isDash)
        {
            Debug.Log("회피 성공");
            //Effect_Manager.instance.Slow_Motion(1f);
            StartCoroutine(nameof(Dash_Dodge_Success));
        }
    }

    IEnumerator Dash_Dodge_Success()
    {
        yield return new WaitForSeconds(0.2f);
        Effect_Manager.instance.Hit_Stop(0.02f);
    }
}
