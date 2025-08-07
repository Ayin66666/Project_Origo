using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_AttackCollider : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int ap;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Damage : " + damage + " / " + ap);
            //other.GetComponent<PlayerTPS_Status>().TakeDamage(damage, ap);
        }
    }
}
