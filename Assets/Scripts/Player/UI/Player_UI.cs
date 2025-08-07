using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    [Header("---UI---")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider damageSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider spSlider;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Text youDie;
    [SerializeField] private Text pressText;

    [Header("---New---")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject pause;
    private bool isPause;
    public bool canPause;

    void Awake()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        youDie.color = new Color(255, 255, 255, 0);
        pressText.color = new Color(255, 255, 255, 0);

        blackScreen.gameObject.SetActive(false);
        youDie.gameObject.SetActive(false);
        pressText.gameObject.SetActive(false);
        canPause = true;
        isPause = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        UI_Update();

        if(Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            switch (isPause)
            {
                case true:
                    PauseOff();
                    break;

                case false:
                    PauseOn();
                    break;
            }
        }
    }

    private void UI_Update()
    {
        // Hp
        hpSlider.value = (float)(Player_Status.instance.curHp / (float)Player_Status.instance.maxHp);

        // Damage
        damageSlider.value = Mathf.Lerp(damageSlider.value, Player_Status.instance.getDamaged / Player_Status.instance.maxHp, Time.deltaTime * 5f);
        
        // Stamina
        staminaSlider.value = Player_Status.instance.curStamina / Player_Status.instance.maxStamina;
       
        // Sp
        spSlider.value = Player_Status.instance.curSp / Player_Status.instance.maxSp;
    }

    #region Pause


    public void PauseOn()
    {
        Cursor.lockState = CursorLockMode.None;
        isPause = true;
        Time.timeScale = 0;

        background.SetActive(true);
        pause.SetActive(true);
    }

    public void PauseOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isPause = false;
        Time.timeScale = 1;

        background.SetActive(false);
        pause.SetActive(false);
    }

    public void ReturnMain()
    {
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 1f;
        SceneLorder.LoadScene("Start_Scene");
    }

    #endregion

    public void DieCall()
    {
        StartCoroutine(DieScreenFadeIn());
    }

    public IEnumerator DieScreenFadeIn()
    {
        yield return new WaitForSeconds(1.5f);

        blackScreen.gameObject.SetActive(true);
        youDie.gameObject.SetActive(true);
        pressText.gameObject.SetActive(true);

        float fadeCount = 0;
        while(fadeCount <= 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            blackScreen.color = new Color(0, 0, 0, fadeCount);
            youDie.color = new Color(255, 255, 255, fadeCount);
        }

        // Delay
        yield return new WaitForSeconds(0.15f);

        fadeCount = 0;
        while(fadeCount <= 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            pressText.color = new Color(255, 255, 255, fadeCount);
        }

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            // 시작화면으로 전환
            yield return null;
        }
        SceneLorder.LoadScene("Start_Scene");
    }
}
