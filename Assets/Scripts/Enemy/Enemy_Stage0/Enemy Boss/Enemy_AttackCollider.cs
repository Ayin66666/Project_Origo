using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackCollider : MonoBehaviour
{
    [Header("---Setting---")]
    public Enemy_Base enemy;
    [SerializeField] private float damage_multiplier; // 데미지 계산 시 보스의 공격력에 곱해지는 값
    [SerializeField] private int damage; // 해당 공격이 플레이어에게 줄 데미지 (방어력 관통력 계산 X)

    private void Start()
    {
        damage = (int)(enemy.damage * damage_multiplier);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Debug.Log("isHit! / Damage : " + damage + " / ArmorPenetration : " + enemy.armorPenetration);
            other.GetComponent<Player_Status>().TakeDamage(damage, enemy.status.ArmorPenetration);
        }
    }
}
