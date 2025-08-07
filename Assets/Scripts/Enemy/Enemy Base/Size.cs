using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Size : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private float maxSize;
    [SerializeField] private float speed;

    public void SizeUp()
    {
        StartCoroutine(nameof(Sizez));
    }

    private IEnumerator Sizez()
    {
        float timer = 1;
        while(timer < maxSize)
        {
            obj.transform.localScale = new Vector3(timer, timer, timer);
            timer += Time.deltaTime * speed;
            yield return null;
        }
    }
}
