using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStop : MonoBehaviour
{
    [SerializeField] private bool isStop;

    void Start()
    {
        if(isStop)
        {
            StartCoroutine(nameof(Stop));
        }
    }

    private IEnumerator Stop()
    {
        Player_Status.instance.canMove = false;
        yield return new WaitForSeconds(3f);
        Player_Status.instance.canMove = true;

    }
}
