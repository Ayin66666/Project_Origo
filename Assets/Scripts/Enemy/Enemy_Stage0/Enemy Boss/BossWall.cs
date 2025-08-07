using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour
{
    [SerializeField] private float timer;
    float tt;
    void Start()
    {
        StartCoroutine(Timer());
    }

    void Update()
    {
        tt += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(tt);
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
