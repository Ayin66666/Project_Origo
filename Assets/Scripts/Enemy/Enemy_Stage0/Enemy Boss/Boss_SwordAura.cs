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
                // �����غ��� ����ü�� ���� ������Ʈ�� ������ ����� ������ �ϴ� ���⼭ ������ ��ü�� �ϰ�Ǿ� �ϳ�?

                other.GetComponent<Player_Status>().TakeDamage(damage, armorPenetration);
                //Debug.Log("isHit! / Damage : " + damage + " / ArmorPenetration : " + armorPenetration);
            }
        }
    }
}
