using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplsionEnd : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private GameObject explsionCollider;

    private void Awake()
    {
        explsionCollider = transform.Find("Attack Collider").gameObject;
    }

    private void OnEnable()
    {
        if (explsionCollider != null)
        {
            StartCoroutine(nameof(On));
        }
        else
        {
            Debug.Log("Collider Null / Object Name : " + gameObject.name);
        }
    }

    private IEnumerator On()
    {
        explsionCollider.SetActive(true);
        yield return new WaitForSeconds(timer);
        explsionCollider.SetActive(false);
    }
}
