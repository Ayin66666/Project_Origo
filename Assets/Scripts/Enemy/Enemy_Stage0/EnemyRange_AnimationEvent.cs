using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange_AnimationEvent : MonoBehaviour
{
    [SerializeField] private Enemy_Range enemy;
    [SerializeField] private Animator anim;

    public void NormalAttack()
    {
        enemy.NormalAttack();
    }

    public void NormalChageOver()
    {
        anim.SetBool("isNormalCharge", false);
    }

    public void NormalOver()
    {
        anim.SetBool("isNormalAttack", false);
    }

    public void MovingChargeOver()
    {
        anim.SetBool("isMovingCharge", false);
    }

    public void MovingDownVFX()
    {
        enemy.MoveDownVFX();
    }

    public void MovingShotOver()
    {
        anim.SetBool("isMovingAttack", false);
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
