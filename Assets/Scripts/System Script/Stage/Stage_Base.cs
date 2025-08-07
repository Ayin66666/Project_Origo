using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public abstract class Stage_Base : MonoBehaviour
{
    [Header("---Stage Data---")]
    [SerializeField] protected Stage_DialogData dialogData;
    [SerializeField] protected Stage_AreaSysteam bossAreaSystem;

    [Header("---Stage Status---")]
    public bool isStartDialog;
    public bool haveStartMark;
    public bool isEndDialog;
    public bool haveEndVideo;
    public bool haveStartAudio;

    public bool isUIDelay;
    public bool isDailog;

    public enum EndType { End, Next }
    public EndType endType;

    [Header("---Start Video---")]
    [SerializeField] private int startVideoTime;
    [SerializeField] private bool haveStartVideo;
    [SerializeField] private VideoPlayer startVideoPlayer;
    [SerializeField] private GameObject startVideoObj;

    [Header("---Fade UI---")]
    [SerializeField] private Image fadeIamge;
    [SerializeField] private Image fadeIamge2;
    [SerializeField] private AudioSource audio;

    [Header("---End Video & Dialog---")]
    [SerializeField] private int endDialogIndex;
    [SerializeField] private int endVideotime;


    [SerializeField] private VideoPlayer endVideoPlayer;
    [SerializeField] private GameObject endVideoObj;

    public void Stage_Start()
    {
        StartCoroutine(nameof(StageStart));
    }

    private IEnumerator StageStart()
    {
        isUIDelay = true;
        if(haveStartVideo)
        {
            fadeIamge2.gameObject.SetActive(false);

            startVideoObj.SetActive(true);
            startVideoPlayer.Play();

            float timer = startVideoTime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            startVideoPlayer.Stop();
            
            float a = 0;
            while(a < 1)
            {
                a += Time.deltaTime;
                fadeIamge.color = new Color(fadeIamge.color.r, fadeIamge.color.g, fadeIamge.color.b, a);
                yield return null;
            }

            startVideoObj.SetActive(false);
        }
        else
        {
            // Start Fade
            float a = 0;
            while (a > 0)
            {
                a -= Time.deltaTime;
                fadeIamge2.color = new Color(fadeIamge2.color.r, fadeIamge2.color.g, fadeIamge2.color.b, a);
                yield return null;
            }

            fadeIamge2.gameObject.SetActive(false);
        }

        // Sound On
        if (haveStartAudio)
        {
            audio.Play();
        }

        // Delay
        yield return new WaitForSeconds(0.25f);
        isUIDelay = false;

        // Dialog
        if (isStartDialog)
        {
            // Delay
            yield return new WaitForSeconds(0.25f);
            Dialog_Call(0, Dialog_Manager.DialogType.TypeB, gameObject);
        }
    }

    public void Stage_End(EndType type, string nextScene)
    {
        audio.Stop();
        StartCoroutine(End(type, nextScene));
    }

    protected IEnumerator End(EndType type, string nextScene)
    {
        Debug.Log("Call End");
        isUIDelay = true;
        // Check EndDialog
        if (isEndDialog)
        {
            Dialog_Call(endDialogIndex, Dialog_Manager.DialogType.TypeB, gameObject);
        }

        // Dialog Wait
        while(isDailog)
        {
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(1.5f);

        // Check EndVideo
        if (haveEndVideo)
        {
            endVideoObj.SetActive(true);
            endVideoPlayer.Play();

            // Video Wait
            float timer = endVideotime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            // Video End
            endVideoPlayer.Stop();
            endVideoObj.SetActive(false);
        }

        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime;
            fadeIamge.color = new Color(fadeIamge.color.r, fadeIamge.color.g, fadeIamge.color.b, a);
            yield return null;
        }

        fadeIamge.color = new Color(fadeIamge.color.r, fadeIamge.color.g, fadeIamge.color.b, 1);

        // Delay
        yield return new WaitForSeconds(0.25f);
        isUIDelay = false;

        // Return To Main
        switch (type)
        {
            case EndType.End:
                // 결과창 -> 메인화면 이동
                SceneLorder.LoadScene(nextScene);
                break;

            case EndType.Next:
                // 다음 스테이지 이동
                SceneLorder.LoadScene(nextScene);
                break;
        }
    }

    public void Dialog_Call(int index, Dialog_Manager.DialogType type, GameObject obj)
    {
        // 다이얼로그 인덱스 데이터와 출력 타입
        Debug.Log(obj);
        Dialog_Manager.instance.DialogOn(dialogData.SendData(index), type);
    }

    public void Cinemachine_Call(int index)
    {
        // Cinemachine On
        // cinemachine.Play(); -> 예제
    }
}
