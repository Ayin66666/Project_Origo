using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_LongLaser_Vertical : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    Vector3 lookDir;

    private void OnDisable()
    {
        // Reset Target
        target.transform.position = start.position;

        // Reset Laser
        lookDir = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = targetRotation;
    }

    public void Setting(float time)
    {
        StartCoroutine(Rot(time));
    }

    private IEnumerator Rot(float time)
    {
        StartCoroutine(TargetMove(time));
        float timer = 0;
        while(timer < 1)
        {
            lookDir = (target.transform.position - transform.position).normalized;

            // Lookat -> ���� �Ĵٺ��� ������� �ӵ��� �ʹ� ������ 2��� �ø���
            timer += Time.deltaTime / time;
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = targetRotation;
            yield return null;
        }
    }

    private IEnumerator TargetMove(float time)
    {
        float timer = 0;
        float rotSpeed = 1f / time; // �ʱ� ȸ�� �ӵ� (1�ʿ� �󸶳� ȸ���� ���ΰ�)
        while (timer < 1)
        {
            timer += Time.deltaTime * rotSpeed;
            target.transform.position = Vector3.Lerp(start.position, end.position, timer);

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
