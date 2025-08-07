using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Guard : MonoBehaviour
{
    [SerializeField] private Enemy_Boss_TypeRed boss;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerAttack"))
        {
            boss.Guard_Hit();
        }
    }
}
