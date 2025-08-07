using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Spawn_New : MonoBehaviour
{
    [Header("---Spawn Setting---")]
    [SerializeField] private SpawnType spawnType;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private Enemy_Base[] enemyBase;

    private enum SpawnType { Normal, Boss }


    [Header("---Dialog Setting---")]
    [SerializeField] private bool haveStartDialog;
    [SerializeField] private int startDialogIndex;
    [SerializeField] private bool haveEndDialog;
    [SerializeField] private int endDialogIndex;


    [Header("---Door---")]
    private GameObject wall;
    private Door[] doors;


    private void Awake()
    {
        
    }

    public void Use()
    {
        Door_Setting(true);

        Enemy_Spawn();

        if(haveStartDialog)
        {
            Dialog(startDialogIndex);
        }

        if(haveEndDialog)
        {

        }
    }

    private void Dialog(int index)
    {
        Stage_Manager.instance.Dialog_Call(index, Dialog_Manager.DialogType.TypeB, gameObject);
    }

    private void Door_Setting(bool isOn)
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Use(isOn);
        }
    }

    private void Enemy_Spawn()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].transform.position = spawnPos[i].position;
            enemys[i].SetActive(true);
        }
    }
}
