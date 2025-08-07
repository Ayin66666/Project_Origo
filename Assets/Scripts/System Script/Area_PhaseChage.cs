using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Area_PhaseChage : MonoBehaviour
{
    [Header("---Stage---")]
    [SerializeField] private MeshRenderer ground;
    [SerializeField] private Material phase2Material;
    [SerializeField] private Material phase2Skybox;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip clip;

    [Header("---Boss---")]
    [SerializeField] private Enemy_Boss_TypeRed boss;
    private bool isChage;
    private bool isPlay;

    [Header("---Player UI---")]
    [SerializeField] private GameObject playerUI;

    private void Start()
    {
        InvokeRepeating(nameof(MapChage), 1, 1);
        InvokeRepeating(nameof(SpawnSound), 1, 1);
    }

    public void UIOnOff(bool isOn)
    {
        if(isOn)
        {
            playerUI.SetActive(true);
        }
        else
        {
            playerUI.SetActive(false);
        }
    }

    public void SpawnSound()
    {
        if(boss.gameObject.activeSelf == false)
        {
            return;
        }

        if(boss.isSoundOn && !isPlay)
        {
            isPlay = true;
            audio.Play();
            CancelInvoke(nameof(SpawnSound));
        }
    }

    public void MapChage()
    {
        if(boss.phase == Enemy_Boss_TypeRed.Phase.Phase2 && !isChage)
        {
            // Sound Change
            isChage = true;
            audio.clip = clip;
            audio.Play();

            // Stage Change
            ground.material = phase2Material;
            RenderSettings.skybox = phase2Skybox;

            // Stop Check
            CancelInvoke(nameof(MapChage));
        }
    }
}
