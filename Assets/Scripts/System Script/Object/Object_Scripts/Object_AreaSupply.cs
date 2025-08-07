using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_AreaSupply : Object_Base
{
    [SerializeField] private Collider colider;

    [Header("---UI---")]
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private GameObject check_Set;
    [SerializeField] private Image check_Image;
    [SerializeField] private Text check_Text;

    // LookAt
    private GameObject lookTarget;
    private Vector3 lookDir;

    public override void Use()
    {
        isUse = true;
        colider.enabled = false;

        Debug.Log("Use Area Supply !");
        Player_Status.instance.Supply();
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
        if(other.CompareTag("Player"))
        {
            check_Set.SetActive(true);
            LookAt();

            if (!isUse && Input.GetKeyDown(KeyCode.F))
            {
                Use();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            check_Set.SetActive(false);
        }
    }
}
