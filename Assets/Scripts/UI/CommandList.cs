using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
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
    private ScrollRect scrollRect;


    private void Start()
    {
        images = new List<Image>();
        scrollRect = this.GetComponent<ScrollRect>();
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
                Debug.LogError("Unknown command passed to CommandList");
                break;
        }

        newImage.transform.SetParent(content.transform);
        images.Add(newImage);
        StartCoroutine(ScrollToEnd());
    }

    public void Clear()
    {
        foreach (Image i in images)
        {
            GameObject.Destroy(i.gameObject);
        }

        images.Clear();
    }

    private IEnumerator ScrollToEnd()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.horizontalNormalizedPosition = 1;
    }
}
