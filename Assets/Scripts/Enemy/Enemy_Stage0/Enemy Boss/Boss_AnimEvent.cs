using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AnimEvent : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Enemy_Boss_TypeRed boss;

    void Update()
    {
        // 모델링 위치 보간
        //transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y, transform.position.z);
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Spawn
    public void SpawnOver()
    {
        anim.SetBool("isSpawn", false);
    }

    // Phase 2
    public void Phase2Over()
    {
        anim.SetBool("isPhase2", false);
    }

    // Step
    public void StepOver()
    {
        anim.SetBool("isStep", false);
        anim.SetInteger("StepType", 0);
    }

    // Groggy
    public void GroggyOver()
    {
        anim.SetBool("isGroggyOver", false);
    }

    // Combo
    public void ComboMove()
    {
        boss.Combo_Move();
    }

    public void ComboA_Attack()
    {
        boss.ComboA_Collider();
    }

    public void ComboB_Attack()
    {
        boss.ComboB_Collider();
    }

    public void ComboOver()
    {
        // Short Attack Combo Check
        anim.SetBool("isComboAttack", false);
    }
     
    // Stomping
    public void Stomping_Spawn()
    {
        boss.Stomping_Collider();
    }

    public void StompingOver()
    {
        anim.SetBool("isStomping", false);
    }

    // Strike
    public void Strike_Spawn()
    {
        anim.SetBool("isStrkeMove", false);
        boss.Strike_Collider();
    }

    public void StrikeOver()
    {
        anim.SetBool("isStrike", false);
    }

    // Guard
    public void Guard_Attack()
    {
        boss.GuardAttackCollider();
    }

    public void GuardOver()
    {
        anim.SetBool("isGuardAttack", false);
    }

    // UpWard Slash
    public void Upward_Spawn()
    {
        boss.Upward_Collider();
    }

    public void UpwardOver()
    {
        anim.SetBool("isUpwardAttack", false);
    }


    // Forward Slash
    public void ForwardMoveOver()
    {
        anim.SetBool("isForwardMove", false);
    }

    public void ForwardChargeOver()
    {
        anim.SetBool("isForwardCharge", false);
    }
    public void ForwardAttack()
    {
        boss.Forward_Collider();
    }

    public void ForwardSlashOver()
    {
        anim.SetBool("isForwardSlash", false);
    }

    // Sword Aura
    public void SwordAuraShot()
    {
        boss.SwordAura_Collider();
    }

    public void SwordAuraOver()
    {
        anim.SetBool("isSwordAura", false);
    }

    // DJ Slash
    public void DashJumpSlashMoveOver()
    {
        anim.SetBool("isDJSlashMove", false);
    }

    public void DashJumpSlashAttack()
    {
        boss.DJAttackCall();
    }

    public void DashJumpSlashOver()
    {
        anim.SetBool("isDJSlash", false);
    }

    // Continuous Attack
    public void Continuous_Move()
    {
        boss.Continuous_Move();
    }

    public void Continuous_MoveOver()
    {
        anim.SetBool("isContinuousMove", false);
    }

    public void Continuous_StrikeOver()
    {
        anim.SetBool("isContinuousStrike", false);
    }

    public void ContinuousAttackA()
    {
        // 1번 내려찍기
        boss.Continuous_ColliderA();
    }

    public void ContinuousAttackB()
    {
        // 2번 내려찍기
        boss.Continuous_ColliderB();
    }

    // Charging Thrust
    public void ChargingThrustOver()
    {
        anim.SetBool("isChargingThrust", false);
    }

    // Laser Attack
    public void LaserForwardAttack()
    {
        boss.LaserForwardAttack();
    }

    public void LaserForwardAttackEnd()
    {
        boss.LaserForwardEnd();
    }

    public void LaserAttackFOver()
    {
        anim.SetBool("isLaserAttackF", false);
    }

    // Long Charging Thrust
    public void LongThrustOver()
    {
        anim.SetBool("isLongThrust", false);
    }


    // LongLaser Attack
    public void LongLaserOver()
    {
        anim.SetBool("isLongLaser", false);
    }
}
