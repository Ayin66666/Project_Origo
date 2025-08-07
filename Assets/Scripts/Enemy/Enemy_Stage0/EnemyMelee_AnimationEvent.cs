using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee_AnimationEvent : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Enemy_Melee enemy;
    public void Normal_AttackCollider()
    {
        enemy.NormalAttack();
    }

    public void NormalOver()
    {
        anim.SetBool("isNormalAttack", false);
    }

    public void Dash_AttackCollider()
    {
        enemy.DashAttack();
    }

    public void DashMove()
    {
        enemy.DashMove();
    }

    public void DushOver()
    {
        anim.SetBool("isDashAttack", false);
    }
    public void StepMove()
    {
        enemy.StepMove();
    }

    public void StepOver()
    {
        anim.SetBool("isStep", false);
    }

    public void GroggyOver()
    {
        anim.SetBool("isGroggy", false);
    }

    public void DieOver()
    {
        anim.SetBool("isDie", false);
    }
}
