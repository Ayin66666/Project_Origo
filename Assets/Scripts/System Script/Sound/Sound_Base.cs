using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sound_Base : MonoBehaviour
{
    [Header("---Setting---")]
    [SerializeField] protected Sound_Slot slot;
    [SerializeField] protected Sound_Slot slot_Loop;
    [SerializeField] private Type type;
    private enum Type { Normal, Boss }
    public bool canPlay;

    public void SoundOn()
    {
        if (type == Type.Normal)
        {
            canPlay = true;
        }
    }

    public void SoundOff()
    {
        if (type == Type.Normal)
        {
            canPlay = false;
        }
    }
}
