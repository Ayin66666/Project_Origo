using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dount_Collider_HitCheck : MonoBehaviour
{
    [SerializeField] private Donut_Collider dount;

    private enum ZoneType { Safe, Damage }
    [SerializeField] private ZoneType zoneType;
    private GameObject target;

    public void Target_Damage(int damage, int armorPenetration)
    {
        //target.GetComponent<Player_Status>().TakeDamage(damage, armorPenetration);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch (zoneType)
            {
                case ZoneType.Safe:
                    dount.isSafe = true;
                    break;

                case ZoneType.Damage:
                    dount.isHit = true;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (zoneType)
            {
                case ZoneType.Safe:
                    dount.isSafe = false;
                    break;

                case ZoneType.Damage:
                    dount.isHit = false;
                    break;
            }
        }
    }
}
