using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCamRotator : MonoBehaviour
{
    private float maxRotY = 155;
    private float maxRotX = 50;

    private const float ROT_X_START = 0;
    private const float ROT_Y_START = -20;
    private float rotX;
    private float rotY;
    private Vector3 defaultRot;

    private void Start()
    {
        defaultRot = new Vector3(0, ROT_Y_START, 0);
        ResetView();
    }

    public void RotateCam(Vector2 delta)
    {
        rotY += delta.x;
        rotX += delta.y;

        if (Mathf.Abs(rotY) > maxRotY)
        {
            rotY = rotY > 0 ? maxRotY : -maxRotY;
        }
        if (Mathf.Abs(rotX) > maxRotX)
        {
            rotX = rotX > 0 ? maxRotX : -maxRotX;
        }

        transform.rotation = Quaternion.identity;
        transform.Rotate(Vector3.up, rotY);
        transform.Rotate(Vector3.right, rotX);
    }

    public void ResetView()
    {
        transform.rotation = Quaternion.Euler(defaultRot);
        rotX = ROT_X_START;
        rotY = ROT_Y_START;
    }
}
