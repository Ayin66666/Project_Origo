using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SwordAura : MonoBehaviour
{
    private Rigidbody rigid;

    [Header("---Damage---")]
    [SerializeField] private int damage;
    [SerializeField] private int armorPenetration;

    [Header("---Movement---")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private Vector3 moveDir;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    public void Setting(int damage, int penetration, Vector3 dir)
    {
        this.damage = damage;
        armorPenetration = penetration;
        moveDir = dir;
        transform.rotation = Quaternion.LookRotation(moveDir);

        StartCoroutine(nameof(Move));
    }

    private IEnumerator Move()
    {
        float timer = 5f;
        while(timer > 0)
        {
            if (speed < 50)
            {
                speed += acceleration * Time.deltaTime;
            }

            rigid.velocity = moveDir * speed;
            timer -= Time.deltaTime;
            yield return null;
        }

        rigid.velocity = Vector3.zero;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            {
                // 생각해보니 투사체는 보스 오브젝트를 가져올 방법이 없으니 일단 여기서 데미지 자체가 완결되야 하네?

                other.GetComponent<Player_Status>().TakeDamage(damage, armorPenetration);
                //Debug.Log("isHit! / Damage : " + damage + " / ArmorPenetration : " + armorPenetration);
            }
        }
    }
}
