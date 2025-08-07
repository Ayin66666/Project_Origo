using System.Collections;
using UnityEngine;

public abstract class Object_Base : MonoBehaviour
{
    [Header("---Object Setting---")]
    [SerializeField] protected Type type;
    [SerializeField] protected bool isUse;
    [SerializeField] protected bool haveDialog;
    [SerializeField] protected int dialogIndex;
    [SerializeField] protected bool haveMark;
    [SerializeField] protected int markIndex;
    protected enum Type { None, hacking, CheckPoint, Supply }

    public abstract void Use();

    protected void Dialog_Setting()
    {
        Stage_Manager.instance.Dialog_Call(dialogIndex, Dialog_Manager.DialogType.TypeB, gameObject);
    }

    protected IEnumerator MarkDelay()
    {
        while (Dialog_Manager.instance.isDialogPrint)
        {
            yield return null;
        }

        Stage_Manager.instance.MarkOn(markIndex);
        gameObject.SetActive(false);
    }
}
