using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandList : MonoBehaviour {

    [SerializeField]
    private Image stampImage;

    [SerializeField]
    private Image zLeftImage;

    [SerializeField]
    private Image yLeftImage;

    private ImageScroller imageScroller;

    private void Start()
    {
        imageScroller = gameObject.GetComponent<ImageScroller>();
    }

    public void Add(Box.Command command)
    {
        Image newImage;

        switch (command)
        {
            case Box.Command.Stamp:
                newImage = GameObject.Instantiate(stampImage);
                break;
            case Box.Command.Left90Y:
                newImage = GameObject.Instantiate(yLeftImage);
                break;
            case Box.Command.Left90Z:
                newImage = GameObject.Instantiate(zLeftImage);
                break;
            default:
                newImage = new GameObject().AddComponent<Image>();
                break;
        }

        imageScroller.Add(newImage);
    }

    public void Clear()
    {
        imageScroller.Clear();
    }
}
