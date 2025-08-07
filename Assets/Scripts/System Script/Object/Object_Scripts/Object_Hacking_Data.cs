using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_Hacking_Data : Object_Base
{
    [SerializeField] private Collider collider;
    [SerializeField] private float hackingSpeed;

    // Look Setting
    private GameObject lookTarget;
    private Vector3 lookDir;

    [Header("---UI---")]
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private GameObject uiSet; // 캔버스

    // Check UI
    [SerializeField] private GameObject check_Set;
    [SerializeField] private Image check_Image;
    [SerializeField] private Text check_Text;

    // Loading
    [SerializeField] private Slider slider; // 로딩 슬라이더
    [SerializeField] private Text loading_Text; // 로딩 텍스트

    // Data
    [SerializeField] private Image data_Image; // 데이터 이미지
    [SerializeField] private Text data_Text; // 데이터 텍스트

    // New
    [SerializeField] private Player_UI playerUI;

    private void Start()
    {
        worldCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        slider.minValue = 0;
        slider.maxValue = 100;
    }

    public override void Use()
    {
        Stage_Manager.instance.MarkOff();

        isUse = true;
        collider.enabled = false;
        StartCoroutine(nameof(IsHacking));
    }

    private IEnumerator IsHacking()
    {
        // UI On
        uiSet.SetActive(true);
        slider.gameObject.SetActive(true);
        loading_Text.gameObject.SetActive(true);

        // Player Pause Off
        playerUI.PauseOff();
        playerUI.canPause = false;

        // Hacking Bar
        float timer = slider.minValue;
        while(timer < slider.maxValue)
        {
            timer += hackingSpeed * Time.deltaTime;
            loading_Text.text = "Hacking..." + (int)slider.value + " %";
            slider.value = timer;
            yield return null;
        }

        // UI Setting & Delay
        slider.value = slider.maxValue;
        loading_Text.text = "Hacking Complete";
        yield return new WaitForSeconds(0.25f);

        // UI OnOff
        slider.gameObject.SetActive(false);
        loading_Text.gameObject.SetActive(false);
        data_Image.gameObject.SetActive(true);
        data_Text.gameObject.SetActive(true);

        // Off Wait
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;
        }

        playerUI.canPause = true;

        // UI Off
        uiSet.SetActive(false);
        isUse = false;

        // Dialog Call
        if (haveDialog)
        {
            Dialog_Setting();
        }

        // delay
        yield return new WaitForSeconds(0.5f);

        // Mark Update
        if (haveMark)
        {
            StartCoroutine(nameof(MarkDelay));
        }
    }

    private void LookAt()
    {
        // LookDir Setting
        lookTarget = GameObject.Find("Main Camera");
        lookDir = check_Set.transform.position - lookTarget.transform.position;
        lookDir.y = 0;

        // Lookat
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        check_Set.transform.rotation = targetRotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            check_Set.SetActive(true);
            LookAt();

            // 테스트용
            if (!isUse && Input.GetKeyDown(KeyCode.F))
            {
                Use();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            check_Set.SetActive(false);
        }
    }
}
