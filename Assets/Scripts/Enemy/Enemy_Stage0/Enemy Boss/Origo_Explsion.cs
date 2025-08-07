using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Origo_Explsion : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float speed;
    [SerializeField] private float weight;
    [SerializeField] private Vector3 moveDir;

    private MeshFilter mesh;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>();
    }
    void Start()
    {
        for (int i = 0; i < mesh.mesh.vertices.Length; i++)
        {
            GameObject obj = Instantiate(bullet);
            obj.transform.position = mesh.mesh.vertices[i] + transform.position;
            obj.transform.forward = mesh.mesh.vertices[i];
        }
    }
}
