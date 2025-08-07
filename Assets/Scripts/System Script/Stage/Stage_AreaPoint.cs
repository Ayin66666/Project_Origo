using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_AreaPoint : MonoBehaviour
{
    [SerializeField] private Stage_Base stage;
    [SerializeField] private Collider collider;
    [SerializeField] private int dailog_Index;

    [SerializeField] private bool haveMark;
    [SerializeField] private int markIndex;

    [SerializeField] private bool isCall;

    private IEnumerator MarkOn()
    {
        while(Dialog_Manager.instance.isDialogPrint)
        {
            yield return null;
        }
        Stage_Manager.instance.MarkOn(markIndex);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isCall)
        {
            isCall = true;
            stage.Dialog_Call(dailog_Index, Dialog_Manager.DialogType.TypeB, gameObject);

            // Mark Check
            if(haveMark)
            {
                StartCoroutine(nameof(MarkOn));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
