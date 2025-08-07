using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Laser : MonoBehaviour
{
    [Header("---Setting---")]
    [SerializeField] private int damage;
    [SerializeField] private int armorPenetration;
    [SerializeField] private GameObject laserCollider;
    [SerializeField] private Transform[] laserPos;
    LineRenderer line;

    [SerializeField] private bool isLaser;

    void Start()
    {
        if(!isLaser)
        {
            // Laser Setting
            line = GetComponent<LineRenderer>();
            line.SetPosition(0, laserPos[0].position);
            line.SetPosition(1, laserPos[1].position);
        }
    }

    public void OnOff(bool isOn)
    {
        if(isOn)
        {
            line.enabled = true;
            laserCollider.SetActive(true);
        }
        else
        {
            line.enabled = false;
            laserCollider.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isLaser)
        {
            other.GetComponent<Player_Status>().TakeDamage(damage, armorPenetration);
        }
    }
}
