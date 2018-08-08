using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour{

    [SerializeField]
    RectTransform content;

    private List<Image> images;
    private int spacing = 16;

    private void Start()
    {
        images = new List<Image>();
    }

    public void Add(Image image)
    {
        float xPos;

        if (images.Count == 0)
        {
            xPos = -(content.rect.width / 2f) + (image.rectTransform.rect.width / 2f) + spacing;
        }
        else
        {
            Image lastImage = images[images.Count - 1];
            xPos = lastImage.rectTransform.anchoredPosition.x + (lastImage.rectTransform.rect.width / 2f) + (image.rectTransform.rect.width / 2f) + spacing;
        }
        
        image.transform.SetParent(content);
        image.rectTransform.anchoredPosition = new Vector2(xPos, 0);

        images.Add(image);
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
