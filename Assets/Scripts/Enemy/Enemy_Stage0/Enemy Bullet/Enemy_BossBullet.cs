using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossBullet : MonoBehaviour
{
    private Rigidbody rigid;

    [Header("---Bullet VFX---")]
    [SerializeField] private GameObject vfx_Prefabs;

    [Header("---Bullet Type A Status---")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private Vector3 moveDir;

    [Header("---Bullet Type B Status---")]
    Vector3[] points = new Vector3[4];
    [SerializeField] private float maxTimer = 0;
    [SerializeField] private float curTimer = 0;

    public enum BulletType { TypeA, TypeB }
    public BulletType bulletType;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Shot_TypeA(BulletType type, Vector3 dir, float speed_Value, float acceleration_Value)
    {
        bulletType = type;
        speed = speed_Value;
        acceleration = acceleration_Value;
        moveDir = dir;
        StartCoroutine(nameof(Move_TypeA));
        // StartCoroutine(Timer());
    }

    private IEnumerator Move_TypeA() // ���� ���� + ����
    {
        float timer = 5f;
        while(timer > 0)
        {
            if(speed < 25)
            {
                speed += acceleration * Time.deltaTime;
            }

            rigid.velocity = moveDir * speed;
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Move_TypeB() // � ���� + ����
    {
        while(curTimer < maxTimer)
        {
            // ��� �ð� ���.
            curTimer += Time.deltaTime * speed;

            // ������ ����� X,Y,Z ��ǥ ���.
            transform.position = new Vector3(
                CubicBezierCurve(points[0].x, points[1].x, points[2].x, points[3].x),
                CubicBezierCurve(points[0].y, points[1].y, points[2].y, points[3].y),
                CubicBezierCurve(points[0].z, points[1].z, points[2].z, points[3].z)
            );

            yield return null;
        }
        // StartCoroutine(Timer());
    }
    public void Shot_TypeB(Transform startPos, Vector3 endPos, float bulletSpeed, float startPointDir, float endPointDir)
    {
        speed = bulletSpeed;

        // ���� �ð�
        maxTimer = Random.Range(0.8f, 1.0f);

        // ���� ����
        points[0] = transform.position;

        // ���� ���� ���� ���� ����Ʈ
        points[1] = startPos.position +
            (startPointDir * Random.Range(-1.0f, 1.0f) * startPos.right) + // X (��, �� ��ü)
            (startPointDir * Random.Range(-0.15f, 1.0f) * startPos.up) + // Y (�Ʒ��� ����, ���� ��ü)
            (startPointDir * Random.Range(-1.0f, -0.8f) * startPos.forward); // Z (�� �ʸ�)

        // ���� ���� ���� ���� ����Ʈ
        points[2] = endPos +
            (endPointDir * Random.Range(-1.0f, 1.0f) * new Vector3(endPos.x, 0, 0)) + // X (��, �� ��ü)
            (endPointDir * Random.Range(-1.0f, 1.0f) * new Vector3(0, endPos.y, 0)) + // Y (��, �Ʒ� ��ü)
            (endPointDir * Random.Range(0.8f, 1.0f) * new Vector3(0, 0, endPos.z)); // Z (�� �ʸ�)

        // ���� ����
        points[3] = endPos;

        // transform.position = startPos.position;
        StartCoroutine(nameof(Move_TypeB));
    }

    private IEnumerator Timer()
    {
        float timer = 5f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    /// 3�� ������ �
    /// <param name="a">���� ��ġ</param>
    /// <param name="b">���� ��ġ���� �󸶳� ���� �� ���ϴ� ��ġ</param>
    /// <param name="c">���� ��ġ���� �󸶳� ���� �� ���ϴ� ��ġ</param>
    /// <param name="d">���� ��ġ</param>
    private float CubicBezierCurve(float a, float b, float c, float d)
    {
        // ������ ���� �ð�
        float t = curTimer / maxTimer; // (���� ��� �ð� / �ִ� �ð�)

        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    public void Bullet_MoveUp()
    {
        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp()
    {
        // Move Up
        float speed = 10;
        while(speed > 0)
        {
            rigid.velocity = Vector3.up * speed;
            speed -= 10 * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator BulletHit()
    {
        // Stop Bullet Move
        rigid.velocity = Vector3.zero;

        // Hit Effect
        Instantiate(vfx_Prefabs, transform.position, Quaternion.identity);

        // Destroy
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player Hit
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Status>().TakeDamage(150, 30);
            StopAllCoroutines();
            StartCoroutine(nameof(BulletHit));
        }
        // Ground, Wall, Object Hit
        else if (other.CompareTag("Ground") || other.CompareTag("Object"))
        {
            StopAllCoroutines();
            StartCoroutine(nameof(BulletHit));
        }
    }
}
