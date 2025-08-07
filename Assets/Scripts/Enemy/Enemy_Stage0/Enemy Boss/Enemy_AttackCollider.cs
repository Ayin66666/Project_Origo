using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackCollider : MonoBehaviour
{
    [Header("---Setting---")]
    public Enemy_Base enemy;
    [SerializeField] private float damage_multiplier; // ������ ��� �� ������ ���ݷ¿� �������� ��
    [SerializeField] private int damage; // �ش� ������ �÷��̾�� �� ������ (���� ����� ��� X)

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
