using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPush3D : Button
{
    public int textOffset;

    private Text text;
    private Vector3 upPosition;
    private Vector3 downPosition;
    private bool pressing;

    protected override void Start()
    {
        base.Start();

        text = GetComponentInChildren<Text>();
        upPosition = text.transform.position;
        downPosition = upPosition - new Vector3(0, textOffset, 0);
        pressing = false;
    }

    protected void Update()
    {
        if (IsPressed() && !pressing && interactable)
        {
            MoveTextDown();
            pressing = true;
        }
        else if (!IsPressed() && pressing)
        {
            MoveTextUp();
            pressing = false;
        }
    }

    private void MoveTextDown()
    {
        text.transform.position = downPosition;
    }

    private void MoveTextUp()
    {
        text.transform.position = upPosition;
    }
}
