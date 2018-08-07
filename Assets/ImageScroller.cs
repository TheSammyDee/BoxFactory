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

    public void AddImage(Image image)
    {
        Vector3 pos;
        float xPos;
        if (images.Count == 0)
        {
            pos = content.transform.position;
            xPos = -(content.rect.width / 2f) + (image.rectTransform.rect.width / 2f) + spacing;
            pos.x = xPos;
            image.transform.SetParent(content);
            image.transform.position = pos;
        }
        else
        {
            Image lastImage = images[images.Count - 1];
            pos = lastImage.transform.position;
        }
    }
}
