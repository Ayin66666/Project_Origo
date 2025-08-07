using System.Collections.Generic;
using UnityEngine;


public class Stage_Manager : Stage_Base
{
    public static Stage_Manager instance;
    [SerializeField] private GameObject[] areaMark;

    [Header("---Enemey Data---")]
    [SerializeField] private List<Stage_AreaSysteam> areaData; // �������� ���� ������ ���� ������

    [Header("---Cinemachine Data---")]
    [SerializeField] private GameObject cinemachine; // ���߿� ������ ���� ������� �ٲ� ��!

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
            //areaMark[0].SetActive(true);   ��������Ʈ �ٲٸ鼭 ������ �ɰŰ����� �ϴ� ����
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
