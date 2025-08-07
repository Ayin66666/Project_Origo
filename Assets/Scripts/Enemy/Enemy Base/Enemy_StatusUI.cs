using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_StatusUI : MonoBehaviour
{
    [Header("---UI---")]
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private GameObject uiSet;
    [SerializeField] private Slider hpBarF;
    [SerializeField] private Slider hpBarB;
    [SerializeField] private Slider groggyBarF;
    [SerializeField] private Slider groggyBarB;
    [SerializeField] private Text nameText;

    [Header("---Setting---")]
    [SerializeField] private Enemy_Base enemy;
    [SerializeField] private float timer;
    [SerializeField] private bool isOn;
    private enum Type { Normal, Boss }
    [SerializeField] private Type type;

    [Header("---Look Setting---")]
    [SerializeField] private Camera mainCam;
    private Vector3 lookDir;

    void Start()
    {
        switch (type)
        {
            case Type.Normal:
                // Cam Setting
                mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
                worldCanvas.worldCamera = mainCam;
                break;

            case Type.Boss:
                nameText.text = enemy.status.Name;
                break;
        }

        // UI Setting
        hpBarF.maxValue = enemy.maxHp;
        hpBarF.value = enemy.maxHp;

        hpBarB.maxValue = enemy.maxHp;
        hpBarB.value = enemy.maxHp;

        groggyBarF.maxValue = enemy.maxSuperArmor;
        groggyBarF.value = enemy.maxSuperArmor;

        groggyBarB.maxValue = enemy.maxSuperArmor;
        groggyBarB.value = enemy.maxSuperArmor;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            enemy.TakeDamage(50, 30, Enemy_Base.HitType.None);
            TimerSetting();
        }
    }

    private void LateUpdate()
    {
        if (type == Type.Normal)
        {
            LookAt();
        }

        UI_Update();
    }

    public void TimerSetting()
    {
        timer = 1.5f;
    }

    private void LookAt()
    {
        // LookDir Setting
        lookDir = mainCam.transform.position - uiSet.transform.position;
        lookDir.y = 0;

        // Lookat
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        uiSet.transform.rotation = targetRotation;
    }

    void UI_Update()
    {
        hpBarF.value = enemy.curHp;
        groggyBarF.value = enemy.curSuperArmor;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            // Hp
            hpBarB.value = Mathf.Lerp(hpBarB.value, enemy.curHp, 10 * Time.deltaTime);

            // Groggy
            if (enemy.state == Enemy_Base.State.Groggy)
            {
                groggyBarB.value = enemy.curSuperArmor;
            }
            else
            {
                groggyBarB.value = Mathf.Lerp(groggyBarB.value, enemy.curSuperArmor, 10 * Time.deltaTime);
            }
        }

        if(enemy.curHp <= 0)
        {
            // Hp
            hpBarB.value = Mathf.Lerp(hpBarB.value, 0, 10 * Time.deltaTime);

            // Groggy
            if(enemy.state == Enemy_Base.State.Groggy)
            {
                groggyBarB.value = enemy.curSuperArmor;
            }
            else
            {
                groggyBarB.value = Mathf.Lerp(groggyBarB.value, 0, 10 * Time.deltaTime);
            }
        }
    }
}
