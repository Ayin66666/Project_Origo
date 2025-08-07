using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Elite : Sound_Base
{
    public enum SoundType { Move, NormalAttack, Shot, DashAttack, Groggy, Die }
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
                slot_Loop.SoundPlay(clips[1]);
                break;

            case SoundType.Shot:
                slot_Loop.SoundPlay(clips[2]);
                break;

            case SoundType.DashAttack:
                slot_Loop.SoundPlay(clips[3]);
                break;

            case SoundType.Groggy:
                slot_Loop.SoundPlay(clips[4]);
                break;

            case SoundType.Die:
                slot_Loop.SoundPlay(clips[5]);
                break;
        }
    }

    public void SoundCall_Loop(bool isOn)
    {
        // Play Check
        if (!canPlay)
        {
            return;
        }

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
