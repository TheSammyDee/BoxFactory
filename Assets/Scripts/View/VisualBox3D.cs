using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisualBox3D : VisualBox {

    public GameObject front;
    public GameObject back;
    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;

    public GameObject stampPrefab;
    public GameObject stampTemplatePrefab;

    private List<GameObject> stampObjects;
    private Dictionary<Face.Side, GameObject> faces; 

    private void Start()
    {
        faces = new Dictionary<Face.Side, GameObject>();
        faces.Add(Face.Side.Front, front);
        faces.Add(Face.Side.Back, back);
        faces.Add(Face.Side.Top, top);
        faces.Add(Face.Side.Bottom, bottom);
        faces.Add(Face.Side.Left, left);
        faces.Add(Face.Side.Right, right);

        stampObjects = new List<GameObject>();
    }

    public override void ApplyBox(Box box, bool isTemplate = false)
    {
        Clear();
        foreach (Face face in box.faces)
        {
            if (face.stamps.Count > 0)
            {
                GameObject currentFace = faces[face.side];

                GameObject prefab = isTemplate ? stampTemplatePrefab : stampPrefab;

                foreach (Stamp stamp in face.stamps)
                {
                    GameObject go = GameObject.Instantiate<GameObject>(prefab);
                    go.transform.SetParent(currentFace.transform, false);
                    go.transform.Rotate(Vector3.down, stamp.rotation);
                    stampObjects.Add(go);
                }
            }
        }
    }

    public override void Clear()
    {
        foreach (GameObject stamp in stampObjects)
        {
            GameObject.Destroy(stamp);
        }

        stampObjects = new List<GameObject>();
    }
}
