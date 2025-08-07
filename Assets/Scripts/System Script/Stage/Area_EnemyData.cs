using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_EnemyData : MonoBehaviour
{
    [Header("---Enemy (Boss)---")]
    [SerializeField] private Enemy_Base boss;
    [SerializeField] private List<int> bossDailog_IndexData;
    [SerializeField] private List<int> bossHp_IndexData;

    [Header("---Enemy (Normal)---")]
    public List<Enemy_Base> enemys;
    [SerializeField] private List<int> dailog_EnemyKillCount;
    [SerializeField] private List<int> dailog_IndexData;
    [SerializeField] private int enemyCount;

    [SerializeField] private int startdialog_IndexData;
    [SerializeField] private int endDialog_IndexData;

    [Header("---Spawn---")]
    [SerializeField] private GameObject[] enemy_Prefabs;
    [SerializeField] private Collider spawnArea;

    [Header("---State---")]
    [SerializeField] private Type type;
    private enum Type { Normal, Boss }
    [SerializeField] private bool enemyOver;
    [SerializeField] private bool bossOver;

    [SerializeField] private bool haveStartDialog;
    [SerializeField] private bool haveEndDialog;
    [SerializeField] private bool isSpawn;
    public bool areaOver;

    private void Spawn()
    {
        isSpawn = true;

        // Spawn Collider Off
        spawnArea.enabled = false;

        // Start Dialog
        if (haveStartDialog)
        {
            Stage_Manager.instance.Dialog_Call(startdialog_IndexData, Dialog_Manager.DialogType.TypeB, gameObject);
        }

        // Spawn
        enemyCount = enemy_Prefabs.Length;
        for (int i = 0; i < enemy_Prefabs.Length; i++)
        {
            enemy_Prefabs[i].transform.parent = null;
            enemy_Prefabs[i].SetActive(true);
        }

        // Area Check
        switch (type)
        {
            case Type.Normal:
                bossOver = true;
                enemyOver = false;
                InvokeRepeating(nameof(Enemy_Check), 1, 1);
                InvokeRepeating(nameof(Area_Check), 1, 1);
                break;

            case Type.Boss:
                bossOver = false;
                enemyOver = false;
                InvokeRepeating(nameof(Enemy_Check), 1, 1);
                InvokeRepeating(nameof(Boss_Check), 1, 1);
                InvokeRepeating(nameof(Area_Check), 1, 1);
                break;
        }
    }

    private void Enemy_Check() // Check Enemy Count
    {
        // List Check
        enemys.RemoveAll(enemy => enemy == null);
        enemyCount = enemy_Prefabs.Length;

        // Dailog Check
        for (int i = 0; i < dailog_EnemyKillCount.Count; i++)
        {
            // Text Data Remove
            if (dailog_EnemyKillCount[i] >= enemyCount)
            {

                // Dailog Call
                if(dailog_IndexData != null)
                {
                    Debug.Log(dailog_EnemyKillCount[i] + " / " + enemyCount);
                    Debug.Log("Dailog Call" + gameObject);
                    Stage_Manager.instance.Dialog_Call(dailog_IndexData[0], Dialog_Manager.DialogType.TypeB, gameObject);
                }

                // Remove List
                dailog_EnemyKillCount.Remove(dailog_EnemyKillCount[i]);
                dailog_IndexData.Remove(dailog_IndexData[0]);
            }
        }

        // End Check
        if (enemys.Count == 0)
        {
            enemyOver = true;
            CancelInvoke(nameof(Enemy_Check));
        }
    }

    private void Boss_Check() // Check Boss Hp
    {
        for (int i = 0; i < bossHp_IndexData.Count; i++)
        {
            if (boss.curHp <= bossHp_IndexData[i])
            {
                // Dailog Call
                Stage_Manager.instance.Dialog_Call(bossDailog_IndexData[i], Dialog_Manager.DialogType.TypeB, gameObject);

                // Remove List
                bossHp_IndexData.Remove(bossHp_IndexData[i]);
                bossDailog_IndexData.Remove(bossDailog_IndexData[i]);
            }
        }

        // End Check
        if (enemys.Count == 0)
        {
            bossOver = true;
            CancelInvoke(nameof(Boss_Check));
        }
    }

    private void Area_Check() // Check Area Over
    {
        if(enemyOver && bossOver)
        {
            areaOver = true;
            if(haveEndDialog)
            {
                // Area End Dailog
                StartCoroutine(nameof(End));
            }

            CancelInvoke(nameof(Area_Check));
        }
    }

    private IEnumerator End()
    {
        Debug.Log("Dailog Call" + gameObject);
        Stage_Manager.instance.Dialog_Call(endDialog_IndexData, Dialog_Manager.DialogType.TypeB, gameObject);

        // Delay
        while (Stage_Manager.instance.isDailog)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isSpawn)
        {
            Spawn();
        }
    }
}
