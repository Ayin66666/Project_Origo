using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_PlayerSystem : MonoBehaviour
{
    public static Sound_PlayerSystem instance;

    [Header("---Sound Slot---")]
    public Sound_Slot oneShotSlot;
    public Sound_Slot loopSlot;

    [Header("---Attack Sound---")]
    [SerializeField] private AudioClip[] combo;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip[] skill;
    [SerializeField] private AudioClip[] guard;
    [SerializeField] private AudioClip[] ult;

    [Header("---Other Sound---")]
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip interaction;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip[] enforce;

    public enum AttackSound { Damage, Combo, Skill, Guard, Ult }
    public enum OtherSound { Move, Heal, Interaction, Dash, Hit, Die , Enforce}

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Sound_AttackOn(AttackSound type, int soundIndex)
    {
        Debug.Log(type + " " + soundIndex);

        switch (type)
        {
            case AttackSound.Damage:
                oneShotSlot.SoundPlay(damage);
                break;

            case AttackSound.Combo:
                oneShotSlot.SoundPlay(combo[soundIndex]);
                break;

            case AttackSound.Skill:
                oneShotSlot.SoundPlay(skill[soundIndex]);
                break;

            case AttackSound.Guard:
                oneShotSlot.SoundPlay(guard[soundIndex]);
                break;

            case AttackSound.Ult:
                oneShotSlot.SoundPlay(ult[soundIndex]);
                break;
        }
    }

    public void Sound_OhterOn(OtherSound type, int soundIndex)
    {
        switch (type)
        {
            case OtherSound.Move:
                loopSlot.SoundPlay(move);
                break;

            case OtherSound.Heal:
                oneShotSlot.SoundPlay(heal);
                break;

            case OtherSound.Interaction:
                oneShotSlot.SoundPlay(interaction);
                break;

            case OtherSound.Dash:
                oneShotSlot.SoundPlay(dash);
                break;

            case OtherSound.Hit:
                oneShotSlot.SoundPlay(hit);
                break;

            case OtherSound.Die:
                oneShotSlot.SoundPlay(die);
                break;

            case OtherSound.Enforce:
                oneShotSlot.SoundPlay(enforce[soundIndex]);
                break;
        }
    }

    public void Move_SoundOff()
    {
        loopSlot.EndPlay();
    }
}
