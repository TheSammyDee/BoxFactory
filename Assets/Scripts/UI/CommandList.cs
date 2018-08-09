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

    [SerializeField]
    RectTransform content;

    private List<Image> images;


    private void Start()
    {
        images = new List<Image>();
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

        newImage.transform.SetParent(content.transform);
        images.Add(newImage);
    }

    public void Clear()
    {
        foreach (Image i in images)
        {
            GameObject.Destroy(i.gameObject);
        }

        images.Clear();
    }
}
