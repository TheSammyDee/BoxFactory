using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCamRotator : MonoBehaviour
{
    private float maxRotY = 155;
    private float maxRotX = 50;

    private float rotX = 0;
    private float rotY = -20;
    private Vector3 defaultRot;

    private void Start()
    {
        defaultRot = new Vector3(0, rotY, 0);
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
    }
}
