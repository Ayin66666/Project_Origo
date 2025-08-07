using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ult_Damage_Collision : MonoBehaviour
{
    [SerializeField] Player_UltSkill playerUlt;
    [SerializeField] LayerMask layerMask;
    void Awake()
    {

    }

    private void OnEnable()
    {
        Collider[] hit = Physics.OverlapBox(transform.position, new Vector3((transform.localScale.x * 0.5f), (transform.localScale.y * 0.5f), (transform.localScale.z * 0.5f)), Quaternion.identity, layerMask);

        for (int i = 0; i < hit.Length; i++)
        {
            hit[i].GetComponent<Enemy_Base>().TakeDamage(playerUlt.ultDamage, Player_Status.instance.armorPenetration, Enemy_Base.HitType.None);
        }
        Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Damage, 0);

        StartCoroutine(nameof(AutoDisable));
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Player_Status.instance.isUlt)
            {
                other.GetComponent<Enemy_Base>().TakeDamage(playerUlt.ultDamage, Player_Status.instance.armorPenetration, Enemy_Base.HitType.None);
                Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Damage, 0);
            }
        }
    }*/
}
