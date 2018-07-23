using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VisualBox2D : VisualBox {

    public Image front;
    public Image back;
    public Image top;
    public Image bottom;
    public Image left;
    public Image right;

    public Image stampImage;
    public Image stampTemplateImage;

    private List<Image> stampImages;

    private Dictionary<Face.Side, Image> faces;

    public void Start()
    {
        faces = new Dictionary<Face.Side, Image>();
        faces.Add(Face.Side.Front, front);
        faces.Add(Face.Side.Back, back);
        faces.Add(Face.Side.Top, top);
        faces.Add(Face.Side.Bottom, bottom);
        faces.Add(Face.Side.Left, left);
        faces.Add(Face.Side.Right, right);

        stampImages = new List<Image>();
    }

    public override void ApplyBox(Box box, bool isTemplate = false)
    {
        Clear();
        foreach (Face face in box.faces)
        {
            if (face.stamps.Count > 0)
            {
                Image currentFace = faces[face.side];

                Image image = isTemplate ? stampTemplateImage : stampImage;

                foreach (Stamp stamp in face.stamps)
                {
                    Image s = GameObject.Instantiate<Image>(image);
                    s.transform.SetParent(currentFace.transform);
                    s.transform.position = currentFace.transform.position;
                    s.transform.Rotate(Vector3.forward, stamp.rotation);
                    stampImages.Add(s);
                }
            }
        }
    }

    public override void Clear()
    {
        foreach(Image stamp in stampImages)
        {
            GameObject.Destroy(stamp);
        }

        stampImages = new List<Image>();
    }
}
