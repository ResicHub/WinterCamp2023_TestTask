using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 cameraMenuPosition = new Vector3(0.29f, 0.04f, 0.24f);
    private Quaternion cameraMenuRotation = Quaternion.Euler(0f, 215f, 0f);

    private Vector3 cameraGamePosition = new Vector3(0f, 0.35f, 0.45f);
    private Quaternion cameraGameRotation = Quaternion.Euler(40f, 180f, 0f);

    private float wTimer = 0;

    private bool isCameraCanRotate = false;
    private float movementSpeed = 50f;

    private void FixedUpdate()
    {
        if (isCameraCanRotate)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.RotateAround(Vector3.zero, Vector3.up, -movementSpeed * Time.fixedDeltaTime);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.RotateAround(Vector3.zero, Vector3.up, movementSpeed * Time.fixedDeltaTime);
            }
        }
    }

    public void MoveToGameField()
    {
        isCameraCanRotate = true;
        wTimer = 0;
        StartCoroutine(MovingCoroutine(transform.position, cameraGamePosition));
        StartCoroutine(RotatingCoroutine(transform.rotation, cameraGameRotation));
    }

    public void MoveToMenu()
    {
        isCameraCanRotate = false;
        wTimer = 0;
        StartCoroutine(MovingCoroutine(transform.position, cameraMenuPosition));
        StartCoroutine(RotatingCoroutine(transform.rotation, cameraMenuRotation));
    }

    private IEnumerator MovingCoroutine(Vector3 startPosition, Vector3 goalPosition)
    {
        while (wTimer <= 0 || transform.position != goalPosition)
        {
            yield return transform.position = Vector3.Lerp(startPosition, goalPosition, wTimer);
            wTimer += Time.deltaTime;
        }
    }

    private IEnumerator RotatingCoroutine(Quaternion startRotation, Quaternion goalRotation)
    {
        while (wTimer <= 0 || transform.rotation != goalRotation)
        {
            yield return transform.rotation = Quaternion.Lerp(startRotation, goalRotation, wTimer);
            wTimer += Time.deltaTime;
        }
    }
}
