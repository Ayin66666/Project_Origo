using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElite_AnimationEvent : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Enemy_Elite enemy;
    [SerializeField] private GameObject normal_Collider;
    [SerializeField] private GameObject rush_Collider;

    public void NormalAttackOver()
    {
        anim.SetBool("isNormalAttack", false);
    }

    public void Normal_AttackCollider()
    {
        enemy.NormalVFX();
        normal_Collider.SetActive(normal_Collider.activeSelf ? false : true);
    }

    public void Rush_Move()
    {
        enemy.RushMove();
    }

    public void Rush_AttackCollider()
    {
        enemy.RushAttackVFX();
    }

    public void RushOver()
    {
        anim.SetBool("isRushAttack", false);
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
        anim.SetBool("isGroggyAnim", false);
    }

    public void DieOver()
    {
        anim.SetBool("isDie", false);
    }
}
