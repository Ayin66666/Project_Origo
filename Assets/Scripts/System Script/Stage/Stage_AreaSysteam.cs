using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSY.Tweening;

public class Stage_AreaSysteam : MonoBehaviour
{
    [Header("---Enemy Normal---")]
    [SerializeField] private int enemyCount;
    [SerializeField] private int curKillCount;
    [SerializeField] private List<int> killCount;
    [SerializeField] private List<Enemy_Base> enemy_List;
    [SerializeField] private List<GameObject> enemy_Prefab;

    [Header("---Enemy Boss---")]
    [SerializeField] private List<int> bossHp;
    [SerializeField] private List<int> bossDialog;
    [SerializeField] private GameObject boss_Prefab;
    [SerializeField] private Enemy_Base boss;

    [Header("---Dialog---")]
    [SerializeField] private int startDialog;
    [SerializeField] private int endDialog;
    [SerializeField] private List<int> dialog;

    [Header("---State---")]
    [SerializeField] private bool have_StartDialog;
    [SerializeField] private bool have_EndDialog;

    // Area End Check
    [SerializeField] private bool isSpawn;
    [SerializeField] private bool isNormalOver;
    [SerializeField] private bool isBossOver;
    [SerializeField] private bool isOver;
    [SerializeField] private bool haveMark;
    [SerializeField] private int markIndex;
    private enum AreaType { Normal, Boss }
    [SerializeField] private AreaType areaType;
    [SerializeField] private Stage_End end;
    [SerializeField] private bool isEnd;


    [Header("---Wall---")]
    [SerializeField] private GameObject wall;
    [SerializeField] private Door[] doors;

    void Start()
    {
        switch (areaType)
        {
            case AreaType.Normal:
                isNormalOver = false;
                isBossOver = true;
                break;

            case AreaType.Boss:
                // isNormalOver = false;
                isBossOver = false;
                break;
        }
    }

    public void BossSpawnCall()
    {
        if (areaType == AreaType.Boss)
        {
            Spwan();
        }
    }

    private void Spwan()
    {
        isSpawn = true;
        Stage_Manager.instance.MarkOff();

        wall.SetActive(true);
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Use(true);
        }
        
        // Start Dialog Call
        if(have_StartDialog)
        {
            Stage_Manager.instance.Dialog_Call(startDialog, Dialog_Manager.DialogType.TypeB, gameObject);
        }

        // Enemy Spawn
        curKillCount = 0;
        for (int i = 0; i < enemy_Prefab.Count; i++)
        {
            enemyCount++;
            enemy_Prefab[i].SetActive(true);
        }

        // Boss Spawn
        if(areaType == AreaType.Boss)
        {
            boss_Prefab.SetActive(true);
        }

        // Check Area
        switch (areaType)
        {
            case AreaType.Normal:
                InvokeRepeating(nameof(CheckArea_Normal), 1, 1);
                InvokeRepeating(nameof(CheckArea_End), 1, 1);
                break;

            case AreaType.Boss:
                InvokeRepeating(nameof(CheckArea_Normal), 1, 1);
                InvokeRepeating(nameof(CheckArea_Boss), 1, 1);
                InvokeRepeating(nameof(CheckArea_End), 1, 1);
                break;
        }
    }

    private void CheckArea_Normal()
    {

        Debug.Log("Check Normal");
        /*
        // Area Enemy Check
        for (int i = enemy_List.Count-1; i > 0; i--)
        {
            Debug.Log("CallA");

            if (enemy_List[i].curHp <= 0)
            {
                Debug.Log("CallB");
                enemyCount--;
                enemy_List.Remove(enemy_List[i]);
            }
        }
        */

        // List Check
        enemy_List.RemoveAll(enemy => enemy == null);
        enemy_Prefab.RemoveAll(enemy => enemy == null);
        curKillCount = enemyCount - enemy_List.Count;

        // Area Dialog Check
        if (killCount.Count > 0)
        {
            for (int i = 0; i < killCount.Count; i++)
            {
                Debug.Log(killCount[i] + "aaaa");
                if (curKillCount >= killCount[i])
                {
                    // Dialog Call
                    Stage_Manager.instance.Dialog_Call(dialog[0], Dialog_Manager.DialogType.TypeB, gameObject);
                    dialog.Remove(dialog[0]);
                    killCount.Remove(killCount[i]);
                    break;
                }
            }
        }

        // End Check
        if(curKillCount == enemyCount)
        {
            isNormalOver = true;
            CancelInvoke(nameof(CheckArea_Normal));
        }
    }

    private void CheckArea_Boss()
    {
        // Area Boss Hp Check
        for (int i = 0; i < bossHp.Count; i++)
        {
            if(boss.curHp <= bossHp[i])
            {
                // Dialog Call
                Stage_Manager.instance.Dialog_Call(bossDialog[0], Dialog_Manager.DialogType.TypeB, gameObject);
                bossDialog.Remove(bossDialog[i]);
                bossHp.Remove(bossHp[i]);
            }
        }

        if(boss.curHp <= 0)
        {
            isBossOver = true;
            CancelInvoke(nameof(CheckArea_Boss));
        }
    }

    private void CheckArea_End()
    {
        if(isNormalOver && isBossOver)
        {
            StartCoroutine(nameof(EndArea));
            CancelInvoke(nameof(CheckArea_End));
        }
    }

    private IEnumerator EndArea()
    {
        // End Dialog Call
        if (have_EndDialog)
        {
            Stage_Manager.instance.Dialog_Call(endDialog, Dialog_Manager.DialogType.TypeB, gameObject);
        }

        // End Area
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Use(false);
        }

        // Dialog Delay
        while (Dialog_Manager.instance.isDialogPrint)
        {
            yield return null;
        }

        // Mark On
        if(haveMark)
        {
            Stage_Manager.instance.MarkOn(markIndex);
        }

        isOver = true;
        if(isEnd)
        {
            end.Call_End(Stage_End.SceneType.NextStage);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isSpawn)
        {
            Spwan();
        }
    }
}
