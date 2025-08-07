using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Player_UltSkill : MonoBehaviour
{
    [Header("---�Ҵ� ���� �͵�---")]
    private Animator anim;
    public GameObject player;
    public Image ultDisable;
    [SerializeField] Player_Attack playerAttack;
    [SerializeField] private RawImage ultRawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private ParticleSystem[] ultEffect;


    public int ultDamage;
    public int ultGroggyDamage;
    [SerializeField] private float ultCooltime;
    private float ultCooltime_Max;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        ultDamage = 1000;
        ultGroggyDamage = 1;
        ultCooltime_Max = 10f;
        ultRawImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(nameof(Ult_Skill));
        }
    }

    IEnumerator Ult_Skill()
    {
        if(Player_Status.instance.isUlt || !Player_Status.instance.canUlt || Player_Status.instance.isDead || Player_Status.instance.allStop)
        {
            yield break;
        }

        if (Player_Status.instance.curSp >= 800)
        {
            Player_Status.instance.canMove = false;
            Player_Status.instance.canEnforce = false;
            Player_Status.instance.isUlt = true;
            Player_Status.instance.isIvincible = true;

            playerAttack.AttackColliderOff(); // ����, ��ų �ݶ��̴� off
            playerAttack.OnUltGroggyCollider(); // �ñر� �׷α� �ݶ��̴� on
            Player_Status.instance.curSp -= 800;
            Debug.Log("�ñر� �ߵ�!");
            //�ñر� �ִϸ��̼� (�̵� �Ұ� �� ���� ����)
            anim.SetBool("IsUlt", true);
            ultCooltime = ultCooltime_Max;
            StartCoroutine(Cooltime());
            //�ƾ� ���
            videoPlayer.Play();
            ultRawImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.1f);

            anim.SetTrigger("Ult_Skill");
            yield return new WaitForSeconds(0.2f);

            ultRawImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            player.SetActive(false);
            yield return new WaitForSeconds(0.3f);

            transform.forward = -transform.forward;
            ultEffect[0].gameObject.SetActive(true);
            Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Ult, 0);
            yield return new WaitForSeconds(0.3f);

            ultEffect[1].gameObject.SetActive(true);
            Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Ult, 1);
            yield return new WaitForSeconds(0.2f);
            Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Ult, 2);
            yield return new WaitForSeconds(0.2f);
            Sound_PlayerSystem.instance.Sound_AttackOn(Sound_PlayerSystem.AttackSound.Ult, 3);
            yield return new WaitForSeconds(0.3f);

            player.SetActive(true);
            anim.SetBool("Ult_SkillFinished", true);
            anim.SetBool("IsUlt", false);
        }
        else
        {
            Debug.Log("Sp ����!");
        }
    }
    IEnumerator Cooltime()
    {
        Player_Status.instance.canUlt = false;
        while (ultCooltime > 0)
        {
            ultCooltime -= Time.deltaTime;
            ultDisable.fillAmount = ultCooltime / ultCooltime_Max;
            yield return null;
        }
        Player_Status.instance.canUlt = true;
    }

}
