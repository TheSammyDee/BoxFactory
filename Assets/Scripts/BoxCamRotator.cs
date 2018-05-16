using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCamRotator : MonoBehaviour
{
    [SerializeField]
    GameObject yRotationGimbal;

    private float maxRotY = 155;
    private float maxRotX = 50;

    private const float ROT_X_START = 0;
    private const float ROT_Y_START = 20;
    private float rotX;
    private float rotY;
    private Vector3 defaultXRot;
    private Vector3 defaultYRot;

    private void Start()
    {
        defaultXRot = new Vector3(ROT_X_START, -ROT_Y_START, 0);
        defaultYRot = new Vector3(0, ROT_Y_START, 0);
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

        transform.rotation = Quaternion.Euler(new Vector3(rotX, -ROT_Y_START, 0));
        yRotationGimbal.transform.localRotation = Quaternion.Euler(new Vector3(0, rotY, 0));
    }

    public void ResetView()
    {
        transform.rotation = Quaternion.Euler(defaultXRot);
        yRotationGimbal.transform.localRotation = Quaternion.Euler(defaultYRot);
        rotX = ROT_X_START;
        rotY = ROT_Y_START;
    }
}
