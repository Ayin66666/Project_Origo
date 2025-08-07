using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScarecrow_AnimationEvent : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Enemy_Scarecrow enemy;

    public void Attack()
    {
        enemy.AttackCollider();
    }

    public void AttackOver()
    {
        anim.SetBool("isAttack", false);
    }
        
    public void HitOver()
    {
        anim.SetBool("isGroggy", false);
    }

    public void DieOver()
    {
        anim.SetBool("isDie", false);
    }
}
