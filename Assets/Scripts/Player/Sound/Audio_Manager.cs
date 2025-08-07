using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

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
}
