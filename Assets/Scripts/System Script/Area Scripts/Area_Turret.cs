using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Turret : MonoBehaviour
{
    [Header("---Turret Setting---")]
    [SerializeField] private int damage;
    [SerializeField] private int armorPenetration;
    [SerializeField] private float cooldown;
    [SerializeField] private float timer;

    [SerializeField] private Transform shotPos;
    [SerializeField] private GameObject bullet;


    private void Start()
    {
        StartCoroutine(nameof(Shot));
    }

    private IEnumerator Shot()
    {
        // Cooldown
        while(timer < cooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Bullet Shot
        GameObject obj = Instantiate(bullet, shotPos.position, shotPos.rotation);
        obj.GetComponent<Enemy_Bullet>().Shot_Setting(Enemy_Bullet.BulletType.BulletA, gameObject, shotPos.forward, 30, damage, armorPenetration);
        Destroy(obj, 3f);
        // Reset
        timer = 0;
        StartCoroutine(nameof(Shot));
    }

    public void Stop()
    {
        Destroy(gameObject);
    }
}
