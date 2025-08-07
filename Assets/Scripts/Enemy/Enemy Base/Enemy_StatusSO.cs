using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Status",menuName = "Scriptable Object/Enemy_Status", order = int.MaxValue)]
public class Enemy_StatusSO : ScriptableObject
{
    [SerializeField] private string name;
    public string Name { get { return name; } }

    [Header("---Attack Status---")]
    [SerializeField] private int damage;
    public int Damage { get { return damage; } }

    [SerializeField] private int armorPenetration;
    public int ArmorPenetration { get { return armorPenetration; } }

    [Header("---Defense Status---")]
    [SerializeField] private int hp;
    public int Hp { get { return hp; } }

    [SerializeField] private int defense;
    public int Defense { get { return defense; } }

    [SerializeField] private int superArmor;
    public int SuperArmor { get { return superArmor; } }

    [SerializeField] private float superArmorRecovery;
    public float SuperArmorRecovery { get { return superArmorRecovery; } }

    [SerializeField] private float reduceDamage;
    public float ReduceDamage { get { return reduceDamage; } }

    [SerializeField] private float groggyTime;
    public float GroggyTime { get { return groggyTime; } }

    [Header("---Utillity Status---")]
    [SerializeField] private int moveSpeed;
    public int MoveSpeed { get { return moveSpeed; } }

    [SerializeField] private float attackSpeed;
    public float AttackSpeed { get { return attackSpeed; } }
}
