using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit_TTT : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z-1);
        transform.rotation = target.transform.rotation;
    }
}
