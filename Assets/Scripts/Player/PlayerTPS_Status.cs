using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPS_Status : MonoBehaviour
{
    [Header("---Attack Status---")]
    public int damage;
    public int criticalChance;
    public float criticalMultiplier;
    public int armorPenetration;
    public float[] motionValue;

    [Header("---Defense Status---")]
    public int curHp;
    public int maxHp;
    public int defense;
    public int hpRecovery;

    [Header("---Utility Status---")]
    public float moveSpeed;
    public float attackSpeed;
    public int curSp;
    public int maxSp;
    public int curStamina;
    public int maxStamina;
    public int staminaRecovery;
    public int guardInvincible;
    public int dashInvincible;

    public void TakeDamage(int damage, int armorPenetration) // �÷��̾� �ǰ� ������ ���
    {

        int damaged = damage - (defense - armorPenetration); // ������ ���
        if(damaged  > 0)
        {
            curHp -= damaged;

            if(curHp <= 0) // ������ ��� �� ��� ���� Ȯ��
            {
                // ��� ���
            }
        }
    }
}
