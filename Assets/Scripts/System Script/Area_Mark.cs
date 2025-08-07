using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Area_Mark : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private GameObject WorldMark_Iamge;
    [SerializeField] private Text markRange_Text;

    [SerializeField] private GameObject cam;
    [SerializeField] private Transform targetPos;
    [SerializeField] private Transform playerPos;
    private Vector3 markPos;
    private Vector3 lookDir;
    private float dir;


    private void OnEnable()
    {
        playerPos = GameObject.Find("Player").transform;
        targetPos = transform;

        cam = GameObject.Find("Main Camera");
        worldCanvas.worldCamera = cam.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        // LookDir Setting
        lookDir = playerPos.position - cam.transform.position;
        lookDir.y = 0;
        // Lookat
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        worldCanvas.transform.rotation = Quaternion.Lerp(worldCanvas.transform.rotation, targetRotation, Time.deltaTime * 5);


        markPos = targetPos.position - playerPos.position;
        markPos.y = 0;
        dir = markPos.magnitude;
        markRange_Text.text = (int)dir + "M";
    }
}
