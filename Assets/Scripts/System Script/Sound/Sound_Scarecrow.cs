using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Scarecrow : Sound_Base
{
    public enum SoundType { Move, Attack, Groggy, Die }
    [SerializeField] private AudioClip[] clips;

    // 사운드 4개 필요!
    // 대기음, 공격, 그로기, 사망

    public void SoundCall_OneShot(SoundType type)
    {
        // Play Check
        if(!canPlay)
        {
            return;
        }

        switch (type)
        {
            case SoundType.Attack:
                slot.SoundPlay(clips[1]);
                break;

            case SoundType.Groggy:
                slot.SoundPlay(clips[2]);
                break;

            case SoundType.Die:
                slot.SoundPlay(clips[3]);
                break;
        }
    }

    public void SoundCall_Loop(SoundType type, bool isOn)
    {
        if(isOn)
        {
            slot_Loop.SoundPlay(clips[0]);
        }
        else
        {
            slot_Loop.EndPlay();
        }
    }
}
