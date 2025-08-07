using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_LongLaser : MonoBehaviour
{
    public Transform target;
    public Transform startTarget;
    Quaternion test;

    void Awake()
    {
        test = transform.rotation;
    }
    void OnDisable()
    {
        Vector3 start = startTarget.position - transform.position;
        Quaternion startLook = Quaternion.LookRotation(start);
        transform.rotation = startLook;
    }

    public void Rotate_Start(float time)
    {
        Debug.Log(transform.rotation);
        StartCoroutine(Rotate(time));
    }

    private IEnumerator Rotate(float totalRotTime) // ���� ���� ���� -> ���� �����ϰ� 179�� �̻� ȸ�� �Ұ�
    {
        Vector3 lookDir = target.position - transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        float timer = 0;
        float rotSpeed = 1f / totalRotTime; // �ʱ� ȸ�� �ӵ� (1�ʿ� �󸶳� ȸ���� ���ΰ�)
        while (timer < 1)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, timer);
            timer += Time.deltaTime * rotSpeed;

            // Speed Up (���൵ 30% �̻�)
            rotSpeed = (timer > 0.3f) ? rotSpeed + Time.deltaTime * 3f : rotSpeed;

            // Speed Down (���൵ 70 �̻� + �ӵ� 0.25 �̻�)
            rotSpeed = (timer > 0.7f) && (rotSpeed > 0.25f) ? rotSpeed - Time.deltaTime * 15f : rotSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
        Debug.Log("End");
        gameObject.SetActive(false);
    }
}
