using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Boss_Laser : MonoBehaviour
{
    public Transform btarget;
    public Transform mTarget;
    public Transform fTarget;

    public void Forward_Setting()
    {
        // Attack
        Vector3 start = fTarget.position - transform.position;
        Quaternion startLook = Quaternion.LookRotation(start);
        transform.rotation = startLook;
    }

    public void Rotate_Start(float time)
    {
        StartCoroutine(Rotate(time));
    }

    private IEnumerator Rotate(float totalRotTime) // ���� ���� ���� -> ���� �����ϰ� 179�� �̻� ȸ�� �Ұ�
    {
        // Rotation Reset
        Vector3 start = mTarget.position - transform.position;
        Quaternion startLook = Quaternion.LookRotation(start);
        transform.rotation = startLook;

        // Rotation Setting
        Vector3 lookDir = btarget.position - transform.position;
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
        gameObject.SetActive(false);
    }
}
