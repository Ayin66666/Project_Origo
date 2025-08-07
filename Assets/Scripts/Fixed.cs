using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        ResetKey();
    }

    private void ResetKey()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            SceneLorder.LoadScene("Start_Scene");
        }
    }
}
