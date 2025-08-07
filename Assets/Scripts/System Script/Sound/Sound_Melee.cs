using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Melee : Sound_Base
{
    public enum SoundType { Move, NormalAttack, RushAttack, Groggy, Die }
    [SerializeField] private AudioClip[] clips;

    public void SoundCall_OneShot(SoundType type)
    {
        // Play Check
        if (!canPlay)
        {
            return;
        }

        // Sound Play
        switch (type)
        {
            case SoundType.NormalAttack:
                slot.SoundPlay(clips[1]);
                break;

            case SoundType.RushAttack:
                slot.SoundPlay(clips[2]);
                break;

            case SoundType.Groggy:
                slot.SoundPlay(clips[3]);
                break;

            case SoundType.Die:
                slot.SoundPlay(clips[4]);
                break;
        }
    }

    public void SoundCall_Loop(SoundType type, bool isOn)
    {
        if (isOn)
        {
            slot_Loop.SoundPlay(clips[0]);
        }
        else
        {
            slot_Loop.EndPlay();
        }
    }
}
