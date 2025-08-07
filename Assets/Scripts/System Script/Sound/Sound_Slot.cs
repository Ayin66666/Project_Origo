using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound_Slot : MonoBehaviour
{
    [Header("---Setting---")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayType playType;
    private enum PlayType { OneShot, Loop }
    public bool isPlay;

    public void SoundPlay(AudioClip clip)
    {
        // Sound
        audioSource.clip = clip;
        switch (playType)
        {
            case PlayType.OneShot:
                audioSource.PlayOneShot(clip);
                StartCoroutine(nameof(Delay));
                break;

            case PlayType.Loop:
                audioSource.Play();
                break;
        }
    }

    public void EndPlay()
    {
        if (audioSource.clip != null)
        {
            switch (playType)
            {
                case PlayType.OneShot:
                    audioSource.Stop();
                    audioSource.clip = null;
                    break;

                case PlayType.Loop:
                    audioSource.Stop();
                    audioSource.clip = null;
                    break;
            }
        }
    }

    private IEnumerator Delay()
    {
        isPlay = true;
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        isPlay = false;
        audioSource.clip = null;
    }
}
