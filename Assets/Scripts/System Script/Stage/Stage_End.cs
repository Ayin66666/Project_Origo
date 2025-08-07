using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_End : MonoBehaviour
{
    private enum SystemType { Collider, Destory, Call }
    [SerializeField] SystemType systemType;

    public enum SceneType { ReturnMain, NextStage }
    [SerializeField] private SceneType sceneType;
    [SerializeField] private string nextSceneName;

    [SerializeField] private Image fadeIamge;
    private bool isFade;

    public void Call_End(SceneType type)
    {
        switch (type)
        {
            case SceneType.ReturnMain:
                Stage_Manager.instance.Stage_End(Stage_Base.EndType.Next, nextSceneName);
                break;

            case SceneType.NextStage:
                Stage_Manager.instance.Stage_End(Stage_Base.EndType.Next, nextSceneName);
                break;
        }
    }

    private void OnDestroy()
    {   
        if(systemType == SystemType.Destory && !isFade)
        {
            StartCoroutine(Fade());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(systemType == SystemType.Collider && other.CompareTag("Player") && !isFade)
        {
            StartCoroutine(Fade());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && systemType == SystemType.Collider && !isFade)
        {
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        isFade = true;
        fadeIamge.gameObject.SetActive(true);

        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime;
            fadeIamge.color = new Color(fadeIamge.color.r, fadeIamge.color.g, fadeIamge.color.b, a);
            yield return null;
        }

        //Delay
        yield return new WaitForSeconds(0.25f);
        Stage_Manager.instance.Stage_End(Stage_Base.EndType.Next, nextSceneName);

    }
}
