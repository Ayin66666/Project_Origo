using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_DialogData : MonoBehaviour
{
    [SerializeField] private Dialog_Data_SO[] data;

    public Dialog_Data_SO SendData(int index)
    {
        return data[index];
    }
}
