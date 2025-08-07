using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI_Manager : MonoBehaviour
{
    [Header("---Start---")]
    // Stage Start
    [SerializeField] private GameObject startUI_Set;
    [SerializeField] private Image startUI_Image;
    [SerializeField] private Text startUI_Text;

    [Header("---End---")]
    // Stage End
    [SerializeField] private GameObject endUI_Set;
    [SerializeField] private Image endUI_Image;
    [SerializeField] private Text endUI_Text;

    [Header("---Boss---")]
    // Boss
    [SerializeField] private GameObject bossUI_Set;
    [SerializeField] private Text boss_Text;
    [SerializeField] private Slider bossHp_Image;
    [SerializeField] private Slider bossGroggy_Image;

    [Header("---Result---")]
    // Stage Result
    [SerializeField] private GameObject result_Set;
    [SerializeField] private Image result_Image;

    [SerializeField] private Text result_clearTime_Text;
    [SerializeField] private Text result_HitCount_Text;
    [SerializeField] private Text result_HitDamage_Text;
    [SerializeField] private Text result_remuneration_Text;

    [Header("---State---")]
    public bool isUI;

    public void Start_UI()
    {
        StartCoroutine(StartUI());
    }

    private IEnumerator StartUI()
    {
        yield return null;
    }

    public void End_UI()
    {
        StartCoroutine(EndUI());
    }

    private IEnumerator EndUI()
    {
        yield return null;
    }
}
