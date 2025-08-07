using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    private Rigidbody rigid;

    [Header("---Prefab---")]
    [SerializeField] private GameObject[] hitVFX_Prefabs;

    [Header("---Bullet Status---")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private int damage;
    [SerializeField] private int armorPenetration;

    public enum BulletType { BulletA, BulletB }
    [SerializeField] private BulletType bulletType;
    [SerializeField] private GameObject target;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Shot_Setting(BulletType type, GameObject targetPos, Vector3 dir, float speed ,int bulletDamage, int armorPenetration)
    {
        bulletSpeed = speed;
        moveDir = dir;
        damage = bulletDamage;
        target = targetPos;
        Debug.Log(target);

        switch (type)
        {
            case BulletType.BulletA:
                StartCoroutine(nameof(Move_TypeA));
                break;

            case BulletType.BulletB:
                StartCoroutine(nameof(Move_TypeB));
                break;
        }
    }

    private IEnumerator Move_TypeA() // 직선 이동
    {
        while(true)
        {
            rigid.velocity = moveDir * bulletSpeed;
            yield return null;
        }
    }

    private IEnumerator Move_TypeB() // 곡선 이동
    {
        float timer = 0f;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, target.transform.position, timer);
            yield return null;
        }
    }

    private IEnumerator BulletHit()
    {
        // Stop Bullet Move
        StopAllCoroutines();
        rigid.velocity = Vector3.zero;

        // Hit Effect
        Instantiate(hitVFX_Prefabs[(int)bulletType], transform.position, Quaternion.identity);

        // Destroy
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player Hit
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player_Status>().TakeDamage(damage, armorPenetration);
            StartCoroutine(nameof(BulletHit));
        }

        // Ground, Wall, Object Hit
        if(other.CompareTag("Ground") || other.CompareTag("Object"))
        {
            StartCoroutine(nameof(BulletHit));
        }
    }
}
