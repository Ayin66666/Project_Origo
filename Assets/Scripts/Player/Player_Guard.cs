using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Guard : MonoBehaviour
{
    [SerializeField] Player_Attack playerAttack;
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Guard();
        }
    }

    void Guard()
    {
        if (Player_Status.instance.isGuard ||  Player_Status.instance.chargingEnforce || Player_Status.instance.isSkill || Player_Status.instance.isUlt || Player_Status.instance.isDead || Player_Status.instance.curStamina < 10 || Player_Status.instance.allStop)
        {
            return;
        }

        Debug.Log("가드!");
        Player_Status.instance.canEnforce = false;
        Player_Status.instance.canMove = false;
        Player_Status.instance.isGuard = true;
        Player_Status.instance.isIvincible = true;
        Player_Status.instance.curStamina -= 10f;
        Player_Status.instance.staminaRecoveryDelay = 1.5f; // 1.5초 뒤에 스태미나 회복

        // Animation
        anim.SetTrigger("Guard");
        anim.SetBool("IsGuard", true);
        anim.SetBool("IsGuardDelayFinished", false);
        anim.SetBool("IsMove", false);
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsStop", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            if (Player_Status.instance.isGuard)
            {
                Debug.Log("가드 성공!");
                Player_Status.instance.isGuardAttack = true;
                Player_Status.instance.curSp += 100;
                Effect_Manager.instance.Hit_Stop(0.01f);
                //Player_Status.instance.isGuard = false;
                anim.SetBool("IsGuard", false);
                GuardAttackBool();
            }
        }
    }
    public void GuardAttackBool()
    {
        anim.SetBool("GuardSuccess", true);
        Player_Status.instance.isIvincible = true;
    }

    public void OnFinishedGuard()
    {
        Player_Status.instance.isGuard = false;
        Player_Status.instance.isAttack = false;
        Player_Status.instance.isGuardAttack = false;
        Player_Status.instance.isIvincible = false;
        playerAttack.curComboCount = 0;
        //수정필요
        anim.SetBool("IsGuard", false);
        anim.SetBool("IsAttack", false);
        anim.SetInteger("ComboCount", playerAttack.curComboCount);
    }

    public void OnFinishedGuardDelay()
    {
        anim.SetBool("IsGuardDelayFinished", true);
    }

    public void OnFinishedGuardAttack()
    {
        anim.SetBool("GuardSuccess", false);
        Player_Status.instance.isIvincible = false;
    }

    public void GuardEndToIdle()
    {
        anim.SetTrigger("GuardEndToIdle");
    }
}
