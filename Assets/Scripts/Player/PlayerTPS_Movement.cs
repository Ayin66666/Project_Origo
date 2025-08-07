using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPS_Movement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform cam;

    [Header("---Movement---")]
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float moveSpeed;
    private float turnSmoothVelocity;
    private float smoothTime = 0.05f;
    private bool canMove = true;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(canMove)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(x, 0, z).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirs = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveSpeed * Time.deltaTime * moveDirs.normalized);
        }
    }

    private IEnumerator Dash()
    {
        canMove = false;
        float timer = 0.25f;
        while(timer > 0)
        {
            controller.Move(transform.forward * moveSpeed * 3f * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        canMove = true;
    }
}
