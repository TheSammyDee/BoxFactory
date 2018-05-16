using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxViewer : MonoBehaviour
{
    public BoxCamRotator boxCamRotator;
    public VisualBox box;

    private float rotationRatioX = 0.3f;
    private float rotationRatioY = 0.1f;

    private bool moving;
    private Vector3 lastPos;
    

    public void Update()
    {
        
    }
    
    public void OnDragBegin()
    {
        lastPos = Input.mousePosition;
    }

    public void OnDrag()
    {
        Vector2 delta = Input.mousePosition - lastPos;
        delta.x = -delta.x;
        boxCamRotator.RotateCam(new Vector2(delta.x * rotationRatioX, delta.y * rotationRatioY));
        lastPos = Input.mousePosition;
    }

    public void OnDragEnd()
    {

    }

    public void ResetView()
    {
        boxCamRotator.ResetView();
    }

    public void ShowBox(bool value)
    {
        box.gameObject.SetActive(value);
    }
}
