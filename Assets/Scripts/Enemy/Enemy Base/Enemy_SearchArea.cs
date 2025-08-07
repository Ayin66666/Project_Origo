using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SearchArea : MonoBehaviour
{
    [SerializeField] private Enemy_Base enemyBase;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject);
            enemyBase.Target_Search(other.gameObject);
        }
    }
}
