using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Veiwer : MonoBehaviour
{
    [Header("---UI---")]
    [SerializeField] private GameObject ui_object;
    [SerializeField] private Image ui_Boder;
    [SerializeField] private Text ui_Text;

    // UI Look Setting
    [SerializeField] private GameObject lookTarget;
    [SerializeField] private bool isOn;
    private Vector3 lookDir;

    // OnOff Speed
    [SerializeField] private float speed;

    void Start()
    {
        lookTarget = GameObject.Find("Main Camera");
    }

    void Update()
    {
        if(isOn)
        {
            // LookDir Setting
            lookDir = ui_object.transform.position - lookTarget.transform.position;
            lookDir.y = 0;
            // Lookat
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            ui_object.transform.rotation = Quaternion.Lerp(ui_object.transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    private void LookAt()
    {
        if(isOn)
        {
            StartCoroutine(nameof(On));
        }
        else
        {
            StartCoroutine(nameof(Off));
        }
    }

    private IEnumerator On()
    {
        StopCoroutine(nameof(Off));

        // UI Setting
        float timer = ui_Boder.color.a;
        while (timer < 1)
        {
            ui_Boder.color = new Color(ui_Boder.color.r, ui_Boder.color.g, ui_Boder.color.b, timer);
            //ui_Text.color = new Color(ui_Text.color.r, ui_Text.color.g, ui_Text.color.b, timer);
            timer += Time.deltaTime * speed;
            yield return null;
        }
    }

    private IEnumerator Off()
    {
        StopCoroutine(nameof(On));

        // UI Setting
        float timer = ui_Boder.color.a;
        while (timer > 0)
        {
            ui_Boder.color = new Color(ui_Boder.color.r, ui_Boder.color.g, ui_Boder.color.b, timer);
            //ui_Text.color = new Color(ui_Text.color.r, ui_Text.color.g, ui_Text.color.b, timer);
            timer -= Time.deltaTime * speed;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOn = true;
            LookAt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOn = false;
            LookAt();
        }
    }
}
