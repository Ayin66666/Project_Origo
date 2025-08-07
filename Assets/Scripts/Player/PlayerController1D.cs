using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1D : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Player_Effect_Manager playerEffectManager;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        StartCoroutine(StopAnim());
    }

    private IEnumerator StopAnim()
    {
        float timer = 0;
        while (timer < 1 && !Input.anyKey)
        {
            Player_Status.instance.isStop = true;
            Player_Status.instance.isMove = false;
            anim.SetBool("IsStop", true);
            anim.SetBool("IsMove", false);
            anim.SetTrigger("Stop");
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void StopToIdle()
    {
        anim.SetBool("IsStop", false);
    }

    public void HitToIdle()
    {
        anim.SetBool("IsHit", false);
        Player_Status.instance.canMove = true;
    }

    public void CanMove()
    {
        Player_Status.instance.canMove = true;
        if(!Player_Status.instance.isEnforce)
        {
            Player_Status.instance.canEnforce = true;
        }
    }
    public void AnimationSlow()
    {
        anim.SetFloat("AnimationSpeed", 0.3f);
        Player_Status.instance.canNextAttack = true;
    }

    public void AnimationSlowFallback()
    {
        anim.SetFloat("AnimationSpeed", 1f);

        if (Player_Status.instance.isEnforce)
        {
            anim.SetFloat("AnimationSpeed", 1.3f);
        }
    }

    public void ReturnEnforceBool()
    {
        Player_Status.instance.chargingEnforce = false;
    }
}
