using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_TypeRed : Sound_Base
{
    public enum NormalSound { Move, Groggy, Die }

    public enum ShortSound { ComboA, ComboB, Stomping, Strike, Backstepshooting, Guard, Upward, Forward }

    public enum MediumSound { SwordAura, DJ, Continuous, Charging, Laser }

    public enum LongSound { LongCharging, LongLaser }

    public enum DroneSound { Move, Attack }

    [Header("---Normal Sound---")] 
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip groggySound;
    [SerializeField] private AudioClip dieSound;

    [Header("---Short Attack Sound---")] // 사운드 14개
    [SerializeField] private AudioClip[] comboASound;
    [SerializeField] private AudioClip[] comboBSound;
    [SerializeField] private AudioClip stompingSound;
    [SerializeField] private AudioClip[] strikeSound;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private AudioClip[] guardSound;
    [SerializeField] private AudioClip[] upwardSound;
    [SerializeField] private AudioClip[] forwardSound;

    [Header("---Medium Attack Sound---")] // 사운드 10개
    [SerializeField] private AudioClip[] swordAuraSound;
    [SerializeField] private AudioClip[] djSound;
    [SerializeField] private AudioClip[] continuousSound;
    [SerializeField] private AudioClip[] chargingSound;
    [SerializeField] private AudioClip[] laserSound;

    [Header("---Long Attack Sound---")]
    [SerializeField] private AudioClip[] longLaserSound;
    [SerializeField] private AudioClip[] longChargingSound;

    [Header("---Drone Sound---")]
    [SerializeField] private AudioClip droneMoveSound;
    [SerializeField] private AudioClip droneAttackSound;

    public void NormalLoopSound_Call(bool isOn)
    {
        if(isOn)
        {
            slot_Loop.SoundPlay(moveSound);
        }
        else
        {
            slot_Loop.EndPlay();
        }
    }

    public void NormalSound_Call(NormalSound type)
    {
        switch (type)
        {
            case NormalSound.Groggy:
                slot.SoundPlay(groggySound);
                break;

            case NormalSound.Die:
                slot.SoundPlay(dieSound);
                break;
        }
    }

    public void ShortSound_Call(ShortSound type, int soundIndex)
    {
        // Sound Type Check
        switch (type)
        {
            // Combo A
            case ShortSound.ComboA:
                switch (soundIndex)
                {
                    case 0:
                        slot.SoundPlay(comboASound[soundIndex]);
                        break;

                    case 1:
                        slot.SoundPlay(comboASound[soundIndex]);
                        break;

                    case 2:
                        slot.SoundPlay(comboASound[soundIndex]);
                        break;
                }
                break;

            // Combo B
            case ShortSound.ComboB:
                switch (soundIndex)
                {
                    case 0:
                        slot.SoundPlay(comboBSound[soundIndex]);
                        break;

                    case 1:
                        slot.SoundPlay(comboBSound[soundIndex]);
                        break;

                    case 2:
                        slot.SoundPlay(comboBSound[soundIndex]);
                        break;

                    case 3:
                        slot.SoundPlay(comboBSound[soundIndex]);
                        break;
                }
                break;

            // Stomping
            case ShortSound.Stomping:
                slot.SoundPlay(stompingSound);
                break;

            case ShortSound.Strike:
                switch (soundIndex)
                {
                    case 0: // Charge
                        slot.SoundPlay(strikeSound[soundIndex]);
                        break;

                    case 1: // Move
                        slot.SoundPlay(strikeSound[soundIndex]);
                        break;

                    case 2: // Attack
                        slot.SoundPlay(strikeSound[soundIndex]);
                        break;
                }
                break;

            case ShortSound.Backstepshooting:
                slot.SoundPlay(shootingSound);
                break;

            // Gurad
            case ShortSound.Guard:
                switch (soundIndex)
                {
                    case 0: // Gurad
                        slot.SoundPlay(guardSound[soundIndex]);
                        break;

                    case 1: // Gurad Attack
                        slot.SoundPlay(guardSound[soundIndex]);
                        break;
                }
                break;

            // Upward
            case ShortSound.Upward:
                switch (soundIndex)
                {
                    case 0: // Charge
                        slot.SoundPlay(upwardSound[soundIndex]);
                        break;

                    case 1: // Slash
                        slot.SoundPlay(upwardSound[soundIndex]);
                        break;

                    case 2: // Explison
                        slot.SoundPlay(upwardSound[soundIndex]);
                        break;
                }
                break;

            // Forward
            case ShortSound.Forward:
                switch (soundIndex)
                {
                    case 0: // Move
                        slot.SoundPlay(forwardSound[soundIndex]);
                        break;

                    case 1: // Slash
                        slot.SoundPlay(forwardSound[soundIndex]);
                        break;
                }
                break;
        }
    }

    public void MediumSound_Call(MediumSound type, int soundIndex)
    {
        switch (type)
        {
            case MediumSound.SwordAura:
                switch (soundIndex)
                {
                    case 0: // Charge
                        slot.SoundPlay(swordAuraSound[soundIndex]);
                        break;

                    case 1: // Slash
                        slot.SoundPlay(swordAuraSound[soundIndex]);
                        break;
                }
                break;

            case MediumSound.DJ:
                switch (soundIndex)
                {
                    case 0: // Move
                        slot.SoundPlay(djSound[soundIndex]);
                        break;

                    case 1: // Slash
                        slot.SoundPlay(djSound[soundIndex]);
                        break;
                }
                break;

            case MediumSound.Continuous:
                switch (soundIndex)
                {
                    case 0: // Move
                        slot.SoundPlay(continuousSound[soundIndex]);
                        break;

                    case 1: // Explison
                        slot.SoundPlay(continuousSound[soundIndex]);
                        break;
                }
                break;

            case MediumSound.Charging:
                switch (soundIndex)
                {
                    case 0: // Move
                        slot.SoundPlay(chargingSound[soundIndex]);
                        break;

                    case 1: // Back Slash
                        slot.SoundPlay(chargingSound[soundIndex]);
                        break;
                }
                break;

            case MediumSound.Laser:
                switch (soundIndex)
                {
                    case 0: // Forward Attack
                        slot.SoundPlay(laserSound[soundIndex]);
                        break;

                    case 1: // 180 Attack
                        slot.SoundPlay(laserSound[soundIndex]);
                        break;
                }
                break;
        }
    }

    public void LongSound_Call(LongSound type, int soundIndex)
    {
        switch (type)
        {
            case LongSound.LongCharging:
                switch (soundIndex)
                {
                    case 0: // Attack
                        slot.SoundPlay(longChargingSound[soundIndex]);
                        break;

                    case 1: // Back Slash
                        slot.SoundPlay(longChargingSound[soundIndex]);
                        break;
                }
                break;

            case LongSound.LongLaser:
                switch (soundIndex)
                {
                    case 0: // Stomping
                        slot.SoundPlay(longLaserSound[soundIndex]);
                        break;

                    case 1: // Attack
                        slot.SoundPlay(longLaserSound[soundIndex]);
                        break;

                    case 2: // Explsion
                        slot.SoundPlay(longLaserSound[soundIndex]);
                        break;
                }
                break;
        }
    }

    public void DroneSound_Call(DroneSound type)
    {
        switch (type)
        {
            case DroneSound.Move:
                slot.SoundPlay(droneMoveSound);
                break;

            case DroneSound.Attack:
                slot.SoundPlay(droneAttackSound);
                break;
        }
    }
}
