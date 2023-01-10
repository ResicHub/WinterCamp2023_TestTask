using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform mainCamera;

    private float mainCameraSpeed = 50f;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mainCamera.RotateAround(Vector3.zero, Vector3.up, - mainCameraSpeed * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            mainCamera.RotateAround(Vector3.zero, Vector3.up, mainCameraSpeed * Time.fixedDeltaTime);
        }
    }
}
