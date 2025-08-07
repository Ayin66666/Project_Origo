using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Player : MonoBehaviour
{
    [Header("---Sound Slot---")]
    public Sound_Slot uiSlot;
    public Sound_Slot attackSlot;
    public Sound_Slot otherSlot;
    public Sound_Slot loopSlot;

    [Header("---Attack Sound---")]
    [SerializeField] private AudioClip[] combo;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip[] skill;
    [SerializeField] private AudioClip[] guard;
    [SerializeField] private AudioClip[] special;

    [Header("---Other Sound---")]
    [SerializeField] private AudioClip idle;
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip interaction;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip die;

    [Header("---UISound---")]
    [SerializeField] private AudioClip select;
    [SerializeField] private AudioClip check;
    [SerializeField] private AudioClip loading;

    public enum AttackSound { Damage, Combo, Skill, Guard, Special }
    public enum OtherSound { Idle, Move, Heal, Interaction, Dash, Hit, Die }
    public enum UISound { Select, Check, Loading }

    public void Sound_AttackOn(AttackSound type, int soundIndex)
    {
        switch (type)
        {
            case AttackSound.Damage:
                
                break;

            case AttackSound.Combo:
                break;

            case AttackSound.Skill:
                break;

            case AttackSound.Guard:
                break;

            case AttackSound.Special:
                break;
        }
    }

    public void Sound_OhterOn(OtherSound type, int soundIndex)
    {
        switch (type)
        {
            case OtherSound.Idle:
                break;

            case OtherSound.Move:
                break;

            case OtherSound.Heal:
                break;

            case OtherSound.Interaction:
                break;

            case OtherSound.Dash:
                break;

            case OtherSound.Hit:
                break;

            case OtherSound.Die:
                break;
        }
    }

    public void Sound_UIOn(UISound type, int soundIndex)
    {
        switch (type)
        {
            case UISound.Select:
                break;

            case UISound.Check:
                break;

            case UISound.Loading:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SoundSystem"))
        {
            other.GetComponent<Sound_Base>().SoundOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SoundSystem"))
        {
            other.GetComponent<Sound_Base>().SoundOff();
        }
    }
}
