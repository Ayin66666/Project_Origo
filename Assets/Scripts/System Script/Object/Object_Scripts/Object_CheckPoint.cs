using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_CheckPoint : Object_Base
{
    [SerializeField] private Collider collider;

    [Header("---UI---")]
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private GameObject check_Set;
    [SerializeField] private Image check_Image;
    [SerializeField] private Text check_Text;

    // LookAt
    private GameObject lookTarget;
    private Vector3 lookDir;

    private void Start()
    {
        worldCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public override void Use()
    {
        Stage_Manager.instance.MarkOff();

        isUse = true;
        collider.enabled = false;
        Debug.Log("Call CheckPoint Call");
        Stage_Manager.instance.Dialog_Call(dialogIndex, Dialog_Manager.DialogType.TypeB, gameObject);

        if (haveMark)
        {
            StartCoroutine(nameof(MarkDelay));
        }
    }

    private void LookAt()
    {
        // LookDir Setting
        lookTarget = GameObject.Find("Main Camera");
        lookDir = check_Set.transform.position - lookTarget.transform.position;
        lookDir.y = 0;

        // Lookat
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        check_Set.transform.rotation = targetRotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            check_Set.SetActive(true);
            LookAt();

            // 테스트용
            if (!isUse && Input.GetKeyDown(KeyCode.F))
            {
                Use();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            check_Set.SetActive(false);
        }
    }
}
