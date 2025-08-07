using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] Player_Attack playerAttack;
    [SerializeField] Player_Skill playerSkill;
    [SerializeField] LayerMask layerMask;

    private void OnEnable()
    {
        Collider[] hit = Physics.OverlapBox(transform.position, new Vector3((transform.localScale.x * 0.5f), (transform.localScale.y * 0.5f), (transform.localScale.z * 0.5f)), Quaternion.identity, layerMask);

        for (int i = 0; i < hit.Length; i++)
        {
            // Hit Stop
            Effect_Manager.instance.Hit_Stop(0.01f);

            hit[i].GetComponent<Enemy_Base>().TakeDamage(playerAttack.CalAttackDamage(), Player_Status.instance.armorPenetration, Enemy_Base.HitType.None);
            if (!Player_Status.instance.isSkill)
            {
                Debug.Log("SP 회복");
                Player_Status.instance.curSp += 30;
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(Player_Status.instance.isAttack || Player_Status.instance.isGuardAttack || Player_Status.instance.isSkill)
            {
                // Hit Stop
                Effect_Manager.instance.Hit_Stop(0.01f);

                // Damage
                other.GetComponent<Enemy_Base>().TakeDamage(playerAttack.CalAttackDamage(), Player_Status.instance.armorPenetration, Enemy_Base.HitType.None);
                if(!Player_Status.instance.isSkill)
                {
                    Debug.Log("SP 회복");
                    Player_Status.instance.curSp += 30;
                }
            }
        }
    }*/
}
