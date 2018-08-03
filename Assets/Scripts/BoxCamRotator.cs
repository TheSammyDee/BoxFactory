using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCamRotator : MonoBehaviour
{
    [SerializeField]
    GameObject yRotationGimbal;

    private float rotX;
    private float rotY;
    private Vector3 defaultXRot;
    private Vector3 defaultYRot;

    private void Start()
    {
        defaultXRot = new Vector3(Config.Instance.BoxCamRotStartX, -Config.Instance.BoxCamRotStartY, 0);
        defaultYRot = new Vector3(0, Config.Instance.BoxCamRotStartY, 0);
        ResetView();
    }

    public void RotateCam(Vector2 delta)
    {
        rotY += delta.x;
        rotX += delta.y;

        if (Mathf.Abs(rotY) > Config.Instance.BoxCamMaxRotY)
        {
            rotY = rotY > 0 ? Config.Instance.BoxCamMaxRotY : -Config.Instance.BoxCamMaxRotY;
        }

        float maxRotX = Config.Instance.BoxCamMaxRotX + Config.Instance.BoxCamMaxRotXOffset;
        float minRotX = -Config.Instance.BoxCamMaxRotX + Config.Instance.BoxCamMaxRotXOffset;
        if (rotX > maxRotX)
        {
            rotX = maxRotX;
        }
        else if (rotX < minRotX)
        {
            rotX = minRotX;
        }

        transform.rotation = Quaternion.Euler(new Vector3(rotX, -Config.Instance.BoxCamRotStartY, 0));
        yRotationGimbal.transform.localRotation = Quaternion.Euler(new Vector3(0, rotY, 0));
    }

    public void ResetView()
    {
        transform.rotation = Quaternion.Euler(defaultXRot);
        yRotationGimbal.transform.localRotation = Quaternion.Euler(defaultYRot);
        rotX = Config.Instance.BoxCamRotStartX;
        rotY = Config.Instance.BoxCamRotStartY;
    }
}
