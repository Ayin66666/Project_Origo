using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog_Data", menuName = "Scriptable Object/Dialog_Data", order = int.MaxValue)]
public class Dialog_Data_SO : ScriptableObject
{
    [SerializeField] private int dialog_Index;
    public int Dialog_Index { get { return dialog_Index; } }

    [SerializeField] private string[] nameText;
    public string[] Name_Text { get { return nameText; } }

    [TextArea]
    [SerializeField] private string[] dialog_Text;
    public string[] Dialog_Text { get { return dialog_Text; } }
}
