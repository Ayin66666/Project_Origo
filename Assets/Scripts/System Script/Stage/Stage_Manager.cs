using System.Collections.Generic;
using UnityEngine;


public class Stage_Manager : Stage_Base
{
    public static Stage_Manager instance;
    [SerializeField] private GameObject[] areaMark;

    [Header("---Enemey Data---")]
    [SerializeField] private List<Stage_AreaSysteam> areaData; // 스테이지 내의 에리어 몬스터 데이터

    [Header("---Cinemachine Data---")]
    [SerializeField] private GameObject cinemachine; // 나중에 동영상 관련 기능으로 바꿀 것!

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (haveStartMark)
        {
            //areaMark[0].SetActive(true);   웨이포인트 바꾸면서 지워도 될거같은데 일단 냅둠
            Waypoint_Manager.instance.Waypoint_Setting(0);
        }
    }

    public void MarkOn(int index)
    {
        Waypoint_Manager.instance.Waypoint_Setting(index);
    }

    public void MarkOff()
    {
        // Mark Off
        for (int i = 0; i < areaMark.Length; i++)
        {
            Waypoint_Manager.instance.Waypoint_Mark_OnOff(i, false);
        }
    }

    private void Start()
    {
        Debug.Log("Dailog Call" + gameObject);
        Stage_Start();
    }
}
