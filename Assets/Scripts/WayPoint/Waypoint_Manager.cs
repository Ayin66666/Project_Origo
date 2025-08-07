using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Waypoint_Manager : MonoBehaviour
{
    public static Waypoint_Manager instance;

    public Camera_WayPoint_Base camera_WayPoint_Base;

    [System.Serializable]
    struct Data
    {
        public int totalWayPoints;
        public GameObject waypointCanvas;
        public GameObject worldWaypoints;

        public List<WayPoint_Controller> wayPoint_Controller;
    }
    
    [SerializeField] Data data;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        camera_WayPoint_Base = GameObject.Find("Main Camera").GetComponentInChildren<Camera_WayPoint_Base>();
    }

    /// <summary>
    /// 지정한 인덱스의 웨이포인트를 활성화하고, 나머지 웨이포인트를 비활성화 하는 기능
    /// </summary>
    /// <param name="index">활성화 할 웨이포인트의 인덱스</param>
    public void Waypoint_Setting(int index)
    {
        camera_WayPoint_Base.data.wayPoints.Clear();
        camera_WayPoint_Base.data.wayPoints.Add(data.wayPoint_Controller[index]);

        for (int i = 0; i < data.wayPoint_Controller.Count; i++)
        {
            Waypoint_Mark_OnOff(i, false);

            data.wayPoint_Controller[i].wayPoint_UI.gameObject.transform.SetParent(data.wayPoint_Controller[i].gameObject.transform);
        }

        Waypoint_Mark_OnOff(index, true);
    }

    public void Waypoint_Mark_OnOff(int index, bool value)
    {
        data.wayPoint_Controller[index].gameObject.SetActive(value);
        data.wayPoint_Controller[index].wayPoint_UI.gameObject.SetActive(value);
    }
}
