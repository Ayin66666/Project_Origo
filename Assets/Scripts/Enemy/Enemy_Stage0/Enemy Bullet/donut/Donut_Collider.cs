using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut_Collider : MonoBehaviour
{
    [SerializeField] private Enemy_Base enemy;
    [SerializeField] private float damage_multiplier;
    [SerializeField] private int damage;

    [Header("---Object---")]
    [SerializeField] private CapsuleCollider safeZone;
    [SerializeField] private CapsuleCollider DamageZone;
    [SerializeField] private Dount_Collider_HitCheck hitCollider;

    [Header("---Size Setting---")]
    public float safeZone_Size;
    public float damageZone_Size;

    [Header("---Hit Check---")]
    public bool isHit;
    public bool isSafe;
    private bool isDelay;

    void Start()
    {
        damage = (int)(enemy.damage * damage_multiplier);
    }

    void Update()
    {
        // Size Setting
        safeZone.radius = safeZone_Size;
        DamageZone.radius = damageZone_Size;

        // Damage
        if(isHit && !isSafe && !isDelay)
        {
            Debug.Log("isHit!");
            hitCollider.Target_Damage(damage, enemy.armorPenetration);
        }
    }

    private IEnumerator Delay()
    {
        isDelay = true;
        yield return new WaitForSeconds(0.1f);
        isDelay = false;
    }

    private void OnEnable()
    {
        Invoke(nameof(Stop), 0.25f);
    }

    private void Stop()
    {
        gameObject.SetActive(false);
    }
}
