using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Manager : MonoBehaviour
{
    public static Dialog_Manager instance;
    private Coroutine curCoroutine;

    [Header("---UI---")]
    [SerializeField] private GameObject dialog;
    [SerializeField] private Text dialog_Text;
    [SerializeField] private Text name_Text;
    [SerializeField] private Image dialog_Line;

    [Header("---Dialog Setting---")]
    [SerializeField] private float typingSpeed;
    [SerializeField] private float textSpeed;
    public bool isDialogPrint;
    public enum DialogType { TypeA, TypeB }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DialogOn(Dialog_Data_SO dialogData, DialogType type)
    {
        // Dailog Reset
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }

        dialog.SetActive(false);

        // Text Reset
        name_Text.text = "";
        dialog_Text.text = "";
        name_Text.color = new Color(name_Text.color.r, name_Text.color.g, name_Text.color.b, 0);
        dialog_Text.color = new Color(dialog_Text.color.r, dialog_Text.color.g, dialog_Text.color.b, 0);
        dialog_Line.color = new Color(dialog_Line.color.r, dialog_Line.color.g, dialog_Line.color.b, 0);

        switch (type)
        {
            case DialogType.TypeA:
                //StartCoroutine(Dialog_TypeA(dialogData));
                curCoroutine = StartCoroutine(Dialog_TypeA(dialogData));
                break;

            case DialogType.TypeB:
                //StartCoroutine(Dialog_TypeB(dialogData));
                curCoroutine = StartCoroutine(Dialog_TypeB(dialogData));
                break;
        }
    }

    private IEnumerator Dialog_TypeA(Dialog_Data_SO dialog_Data)
    {
        // Dialog On
        dialog.SetActive(true);
        isDialogPrint = true;

        for (int i = 0; i < dialog_Data.Dialog_Text.Length; i++)
        {
            // Reset Text
            name_Text.text = "";
            dialog_Text.text = "";

            // Text Setting
            name_Text.text = dialog_Data.Name_Text[i];
            foreach(char t in dialog_Data.Dialog_Text[i])
            {
                dialog_Text.text += t;
                yield return new WaitForSeconds(typingSpeed);
            }

            // Text Delay
            yield return new WaitForSeconds(1f);
        }

        // Dialog Off
        dialog.SetActive(false);
        curCoroutine = null;
        isDialogPrint = false;
    } // 글자가 한 글자씩 나오는 방식

    private IEnumerator Dialog_TypeB(Dialog_Data_SO dialog_Data) // 글자의 투명도 조절로 나오는 방식
    {
        // Dialog On
        name_Text.color = new Color(name_Text.color.r, name_Text.color.g, name_Text.color.b, 0);
        dialog_Text.color = new Color(dialog_Text.color.r, dialog_Text.color.g, dialog_Text.color.b, 0);
        dialog_Line.color = new Color(dialog_Line.color.r, dialog_Line.color.g, dialog_Line.color.b, 0);
        dialog.SetActive(true);
        isDialogPrint = true;

        for (int i = 0; i < dialog_Data.Dialog_Text.Length; i++)
        {
            // Text Setting
            name_Text.text = dialog_Data.Name_Text[i];
            dialog_Text.text = dialog_Data.Dialog_Text[i];

            if(i == 0)
            {
                // On
                float a = 0;
                while (a < 1)
                {
                    a += Time.deltaTime;
                    name_Text.color = new Color(name_Text.color.r, name_Text.color.g, name_Text.color.b, a);
                    dialog_Text.color = new Color(dialog_Text.color.r, dialog_Text.color.g, dialog_Text.color.b, a);
                    dialog_Line.color = new Color(dialog_Line.color.r, dialog_Line.color.g, dialog_Line.color.b, a);
                    yield return null;
                }
            }
            else
            {
                name_Text.color = new Color(name_Text.color.r, name_Text.color.g, name_Text.color.b, 1);
                dialog_Text.color = new Color(dialog_Text.color.r, dialog_Text.color.g, dialog_Text.color.b, 1);
            }
  

            // Delay
            yield return new WaitForSeconds(textSpeed);

            // Off
            if(i == dialog_Data.Dialog_Text.Length-1)
            {
                float a = 1;
                while (a > 0)
                {
                    a -= Time.deltaTime;
                    name_Text.color = new Color(name_Text.color.r, name_Text.color.g, name_Text.color.b, a);
                    dialog_Text.color = new Color(dialog_Text.color.r, dialog_Text.color.g, dialog_Text.color.b, a);
                    dialog_Line.color = new Color(dialog_Line.color.r, dialog_Line.color.g, dialog_Line.color.b, a);
                    yield return null;
                }
            }
            else
            {
                // Next Delay
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Dialog Off
        dialog.SetActive(false);
        curCoroutine = null;
        isDialogPrint = false;
    }
}
