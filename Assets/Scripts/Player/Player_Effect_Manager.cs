using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Effect_Manager : MonoBehaviour
{
    [SerializeField] private Transform ultEffectPos;
    [SerializeField] private Transform attackEffectPos;
    [SerializeField] private ParticleSystem[] ultEffect;
    public ParticleSystem[] skillEffect;
    public ParticleSystem[] attackEffect;
    [SerializeField] private ParticleSystem dashEffect;
    [SerializeField] private ParticleSystem guardSuccessEffect;
    [SerializeField] private ParticleSystem guardAttackEffect;
    [SerializeField] private ParticleSystem[] enforceEffect;
    [SerializeField] private ParticleSystem healEffect;

    [SerializeField] Player_Skill playerSkill;
    [SerializeField] Player_Attack playerAttack;
    // Start is called before the first frame update
    void Awake()
    {

    }

    #region 소환함수
    public void OnSkillEffect1()
    {
        skillEffect[playerSkill.curSkillCount - 1].gameObject.SetActive(true);
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Skill, playerSkill.curSkillCount - 1);
    }

    public void OnSkillEffect2() //스킬 2번째 공격 중 2타 이펙트
    {
        skillEffect[3].gameObject.SetActive(true);
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Skill, 3);
    }

    public void OnAttackEffect()  //이펙트 소환용 
    {
        attackEffect[playerAttack.curComboCount - 1].gameObject.SetActive(true);
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Combo, playerAttack.curComboCount - 1);
    }

    public void OnAttackEffect2()  //3번째 공격 2타 이펙트
    {
        attackEffect[4].gameObject.SetActive(true);
    }

    public void OnAttackEffect3()  //4번째 공격 지면 이펙트
    {
        attackEffect[5].gameObject.SetActive(true);
        Instantiate(attackEffect[5], new Vector3(transform.parent.position.x, transform.parent.position.y + 1f, transform.parent.position.z), transform.rotation * Quaternion.Euler(0, -90, 0));
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Combo, playerAttack.curComboCount - 1);
    }
    public void OnDashEffect()
    {
        Instantiate(dashEffect, new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z), transform.rotation);
        Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Dash, 0);
    }

    public void OnGuardSuccessEffect()
    {
        guardSuccessEffect.gameObject.SetActive(true);
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Guard, 0);
    }

    public void OnGuardAttackEffect()
    {
        guardAttackEffect.gameObject.SetActive(true);
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Guard, 1);
    }

    public void OnEnforceEffect1()
    {
        enforceEffect[0].gameObject.SetActive(true);
        Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Enforce, 0);
    }

    public void OnEnforceEffect2()
    {
        enforceEffect[1].gameObject.SetActive(true);
        Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Enforce, 1);
    }

    public void OnUltEffect()
    {
        ultEffect[2].gameObject.SetActive(true);
    }

    public void OnHealEffect()
    {
        Instantiate(healEffect, transform.parent);
        Sound_PlayerSystem.instance.Sound_OhterOn(Sound_PlayerSystem.OtherSound.Heal, 0);
    }
    #endregion
}
