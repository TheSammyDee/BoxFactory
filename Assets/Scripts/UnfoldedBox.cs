using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnfoldedBox : VisualBox {

    public Image front;
    public Image back;
    public Image top;
    public Image bottom;
    public Image left;
    public Image right;

    public Image stampImage;

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

    public override void ApplyBox(Box box)
    {
        Clear();
        foreach (Face face in box.faces)
        {
            if (face.stamps.Count > 0)
            {
                Image currentFace = faces[face.side];

                foreach (Stamp stamp in face.stamps)
                {
                    Image s = GameObject.Instantiate<Image>(stampImage);
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
