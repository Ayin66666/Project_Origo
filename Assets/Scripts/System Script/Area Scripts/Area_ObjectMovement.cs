using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_ObjectMovement : MonoBehaviour
{
    [Header("---Setting---")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stopTime;

    [SerializeField] private Vector3 movePos1; // DownPos
    [SerializeField] private Vector3 movePos2; // UpPos
    private Vector3 originalPos;

    private void Start()
    {
        moveSpeed = Random.Range(0.05f, 0.25f);
        //stopTime = Random.Range(1.5f, 5f);

        // Pos Setting
        originalPos = transform.position;
        movePos1 = transform.position - new Vector3(0, Random.Range(-3f, -5f), 0);
        movePos2 = transform.position - new Vector3(0, Random.Range(+3f, +5f), 0);

        // Move Start
        StartCoroutine(nameof(Movemnet));
    }

    private IEnumerator Movemnet()
    {
        // First Movement
        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(originalPos, movePos1, timer);
            yield return null;
        }

        // Movement(Loop)
        while (true)
        {
            timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(movePos1, movePos2, InOutCubic(timer));
                yield return null;
            }
            transform.position = movePos2;

            // Delay
            yield return new WaitForSeconds(stopTime);

            timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(movePos2, movePos1, InOutCubic(timer));
                yield return null;
            }
            transform.position = movePos1;

            // Delay
            yield return new WaitForSeconds(stopTime);
        }
    }

    public static float InCubic(float t) => t * t * t;
    public static float OutCubic(float t) => 1 - InCubic(1 - t);
    public static float InOutCubic(float t)
    {
        if (t < 0.5) return InCubic(t * 2) / 2;
        return 1 - InCubic((1 - t) * 2) / 2;
    }

    public static float InCirc(float t) => -((float)Mathf.Sqrt(1 - t * t) - 1);
    public static float OutCirc(float t) => 1 - InCirc(1 - t);
    public static float InOutCirc(float t)
    {
        if (t < 0.5) return InCirc(t * 2) / 2;
        return 1 - InCirc((1 - t) * 2) / 2;
    }
}
