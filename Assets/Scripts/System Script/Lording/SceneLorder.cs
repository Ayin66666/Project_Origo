using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLorder : MonoBehaviour
{
    public static SceneLorder instance;
    [SerializeField] static string nextScene;

    [Header("---Loading Image---")]
    [SerializeField] private Slider progressbar;
    [SerializeField] private Text loadText;
    [SerializeField] private Text tipText;
    [SerializeField] private string[] texts;
    public static int curStage;

    AudioSource audio;

    private bool isLoading;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
        instance = this;
    }

    void Start()
    {
        StartCoroutine(TTT());
    }

    public static void LoadScene(string sceneName)
    {
        // Next Scene Name Setting 
        curStage++;
        nextScene = sceneName;
        SceneManager.LoadScene("Loading_Scene");
    }

    public static void ReturnMain()
    {
        Time.timeScale = 1;
        curStage = 0;
        nextScene = "Start_Scene";
        SceneManager.LoadScene("Loading_Scene");
    }

    IEnumerator Tip()
    {
        // Tip Text Fade Setting
        tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, 0);
        while (isLoading)
        {
            tipText.text = texts[Random.Range(0, texts.Length)];

            float a = 0;
            while (a < 1)
            {
                tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, a);
                a += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            a = 1;
            while (a > 0)
            {
                tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, a);
                a -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator TTT()
    {
        audio.Play();
        Cursor.lockState = CursorLockMode.None;

        Debug.Log(nextScene);
        isLoading = true;
        // StartCoroutine(nameof(Tip));
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;


        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            Debug.Log(operation.isDone);
            Debug.Log(progressbar.value);

            yield return null;
            if (progressbar.value < 0.9f)
            {
                Debug.Log("call Vel up");
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                Debug.Log("call Vel up2");
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            if (progressbar.value >= 1f)
            {
                Debug.Log("call Vel up3");
                loadText.text = "Press SpaceBar to Start.";
            }

            if (Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                // Scene Move
                isLoading = false;
                operation.allowSceneActivation = true;
            }
        }
    }
}
