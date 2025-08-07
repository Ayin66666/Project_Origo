using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using LSY.Tweening;

public class StartScene : MonoBehaviour
{
    [Header("---UI---")]
    public Image blackScreen;
    public Image option;
    public Transform[] optionTargetPos;
    public Text gameName;
    public Text[] buttonsText;
    public Button[] buttons;

    [Header("---State---")]
    public bool isBlackScreenFadeOut;
    public bool isGameNameFadeIn;
    public bool isButtonsFadeIn;
    public bool isClickOption;
    public bool isClickQuitOption;
    private float moveTimer;

    void Awake()
    {
        isBlackScreenFadeOut = true;  

        option.transform.position = optionTargetPos[0].transform.position; // 옵션창 안 보이는 곳으로

        // 안 보이게
        gameName.color = new Color(255, 255, 255, 0);
        for (int i = 0; i < buttonsText.Length; i++)
        {
            buttonsText[i].color = new Color(255, 255, 255, 0);
        }

        // 버튼 작동 off
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = buttons[i].GetComponent<Button>();
            buttons[i].interactable = false;
        }

        // 검은화면에서 메인화면 페이드 인
        blackScreen.color = new Color(0, 0, 0, 1);
        StartCoroutine(BlackScreenFadeOut());
    }

    void Update()
    {
        moveTimer += Time.deltaTime;

        // option 눌렀으면 option 버튼 비활성화
        if (isClickOption)
        {
            buttons[2].interactable = false;
        }

        // 옵션창 닫는 버튼 눌렀으면 닫기 버튼 비활성화
        if (isClickQuitOption)
        {
            buttons[4].interactable = false;
        }
    }

    IEnumerator BlackScreenFadeOut()
    {
        float fadeCount = 1;
        isBlackScreenFadeOut = true;

        yield return new WaitForSeconds(0.5f);

        while(fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            blackScreen.color = new Color(0, 0, 0, fadeCount);
        }

        // 메인화면 페이드 인 됐으면 게임 이름 나타나기
        isBlackScreenFadeOut = false;
        StartCoroutine(GameNameFadeIn());
    }

    IEnumerator GameNameFadeIn()
    {
        float fadeCount = 0;
        isGameNameFadeIn = true;

        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            gameName.color = new Color(255, 255, 255, fadeCount);
        }

        // 게임 이름 나타났으면 버튼들 나타나기
        isGameNameFadeIn = false;
        StartCoroutine(ButtonsTextFadeIn());
    }

    IEnumerator ButtonsTextFadeIn()
    {
        float fadeCount = 0;
        isButtonsFadeIn = true;
        
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            for (int i = 0; i < buttonsText.Length; i++)
            {
                buttonsText[i].color = new Color(255, 255, 255, fadeCount);
            }
        }

        isButtonsFadeIn = false;

        // 버튼들 다 나타난 후 클릭 활성화
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void ClickGameStart()
    {
        Debug.Log("게임 시작");
        //SceneLorder.LoadScene("Stage0_Tutorial");
        SceneLorder.LoadScene("Stage0_Tutorial");
        // 씬 전환
    }

    public void ClickBossRoom()
    {
        Debug.Log("보스룸");
        SceneLorder.LoadScene("Stage0_Urban_RedLine_BossArea");

        // 씬 전환
    }

    public void ClickOption()
    {
        isClickOption = true;

        StopCoroutine(nameof(OffOption)); // 중복방지
        Debug.Log("옵션창 켜기");
        moveTimer = 0;
        StartCoroutine(nameof(OnOption));
    }

    public void ClickOptionQuit()
    {
        isClickQuitOption = true;

        StopCoroutine(nameof(OnOption)); // 중복방지
        Debug.Log("옵션창 닫기");
        moveTimer = 0;
        StartCoroutine(nameof(OffOption));
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator OnOption()
    {
        while (moveTimer <= 2f)
        {
            option.transform.position = Vector3.Lerp(optionTargetPos[0].position, optionTargetPos[1].position, EasingFunctions.OutExpo(moveTimer));
            yield return null;
        }

        isClickOption = false;
        buttons[4].interactable = true;
    }

    IEnumerator OffOption()
    {
        while (moveTimer <= 2f)
        {
            option.transform.position = Vector3.Lerp(optionTargetPos[1].position, optionTargetPos[0].position, EasingFunctions.InBack(moveTimer));
            yield return null;
        }

        isClickQuitOption = false;
        buttons[2].interactable = true;
    }
}
