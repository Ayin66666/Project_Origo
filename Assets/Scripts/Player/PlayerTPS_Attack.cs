using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPS_Attack : MonoBehaviour
{
    private PlayerTPS_Status status;

    private Animator anim;

    private enum CurAttack { None, Combo1, Combo2, Combo3, Combo4, Skill1, Skill2 }
    private CurAttack curAttack; // 현재 공격
    private int damageB; // 관통력을 제외한 데미지 계산값

    // Test
    private bool isAttack;
    private int count;
    private float delayTime;


    private void Awake()
    {
        status = GetComponent<PlayerTPS_Status>();
    }

    private void Update()
    {
        Attack_Timer();
        Attack();
    }

    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !isAttack)
        {
            isAttack = true;
            delayTime = 1f;
            count++;

            // Animation
            anim.SetInteger("Count", count);
            anim.SetTrigger("Attack");
            anim.SetBool("isAttack", true);
        }
    }

    void Attack_Timer()
    {
        if(delayTime > 0)
        {
            delayTime -= Time.deltaTime;
        }

        if (delayTime <= 0)
        {

            delayTime = 0;
            count = 0;
            anim.SetInteger("Count", count);
        }
    }

    private void DamageCalculation() // 데미지 계산 함수
    {
        damageB = (int)(status.damage * status.motionValue[(int)curAttack]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemey"))
        {
            other.GetComponent<Enemy_Base>().TakeDamage(damageB, status.armorPenetration, Enemy_Base.HitType.None);
        }
    }
}
