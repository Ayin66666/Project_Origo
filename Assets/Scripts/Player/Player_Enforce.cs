using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Enforce : MonoBehaviour
{
    [SerializeField] Player_Movement playerMovement;
    [SerializeField] float enforceTime;
    private Animator anim;
    [SerializeField] private float enforceCooltime;
    [SerializeField] private float enforceCooltime_Max;
    public Image enforceDisable;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("AnimationSpeed", 1f);
        enforceCooltime_Max = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Player_Status.instance.canEnforce)
        {
            StartCoroutine(Enforce());
        }
    }

    IEnumerator Enforce()
    {
        Debug.Log("±¤ÆøÈ­!");
        anim.SetTrigger("Enforce");
        Player_Status.instance.isEnforce = true;
        Player_Status.instance.chargingEnforce = true;
        Player_Status.instance.canMove = false;
        enforceCooltime = enforceCooltime_Max;
        enforceTime = 15f;
        StartCoroutine(Cooltime());

        playerMovement.speed *= 1.5f;
        Player_Status.instance.damage = (int)(Player_Status.instance.damage * (double)1.3);

        while (enforceTime > 0)
        {
            enforceTime -= Time.deltaTime;
            yield return null;
        }

        Player_Status.instance.isEnforce = false;
        playerMovement.speed /= 1.5f;
        Player_Status.instance.damage = (int)(Player_Status.instance.damage / (double)1.3);
    }

    IEnumerator Cooltime()
    {
        /*
        while (enforceCooltime > 0)
        {
            Player_Status.instance.canEnforce = false;
            enforceCooltime -= Time.deltaTime;
            enforceDisable.fillAmount = enforceCooltime / enforceCooltime_Max;
            yield return null;
        }
        while (enforceCooltime < 0)
        {
            Player_Status.instance.canEnforce = true;
            yield return null;
        }
        */
        Player_Status.instance.canEnforce = false;

        while (enforceCooltime > 0)
        {
            enforceCooltime -= Time.deltaTime;
            enforceDisable.fillAmount = enforceCooltime / enforceCooltime_Max;
            yield return null;
        }

        Player_Status.instance.canEnforce = true;
    }
}
