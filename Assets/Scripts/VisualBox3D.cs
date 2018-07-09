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

    private const float PAUSE_TIME = 0.8f;
    private const int ROTATION_SPEED = 90;

    private List<GameObject> stampObjects;
    private Dictionary<Face.Side, GameObject> faces; 
    private float pauseTimer;
    private bool paused;
    private bool animating;
    private List<VisualBox.Command> animationCommands;
    private int commandNum;
    private float rotationCounter;
    private float rotationGoal;
    private Vector3 rotationAxis;

    private UnityAction FinishAnimation;

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

    private void Update()
    {
        if (animating)
        {
            if (paused)
            {
                if (pauseTimer >= PAUSE_TIME)
                {
                    paused = false;
                    pauseTimer = 0;
                    if (commandNum == animationCommands.Count)
                    {
                        animating = false;
                        transform.localRotation = Quaternion.identity;
                        FinishAnimation();
                    }
                    else
                    {
                        PrepAnimation(animationCommands[commandNum]);
                    }
                }
                else
                {
                    pauseTimer += Time.deltaTime;
                }
            }
            else
            {
                if (animationCommands[commandNum] == VisualBox.Command.Stamp)
                {
                    AnimateStamp();
                }
                else
                {
                    AnimateRotation();
                }
            }
        }
    }

    private void PrepAnimation(VisualBox.Command command)
    {
        rotationCounter = 0f;
        switch (command)
        {
            case VisualBox.Command.Left90Y:
                rotationGoal = 90f;
                rotationAxis = transform.InverseTransformDirection(Vector3.up);
                break;
            case VisualBox.Command.Left90Z:
                rotationGoal = 90f;
                rotationAxis = transform.InverseTransformDirection(Vector3.forward);
                break;
        }
    }

    private void AnimateStamp()
    {
        GameObject go = GameObject.Instantiate<GameObject>(stampPrefab);
        go.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.back);

        float boxRadius = (transform.localPosition.z - front.transform.localPosition.z);
        float worldBoxRadius = boxRadius * transform.lossyScale.z;
        go.transform.position = transform.position - (Vector3.forward * worldBoxRadius);
        Vector3 scale = go.transform.localScale;
        go.transform.SetParent(transform, true);
        go.transform.localScale = scale;
        stampObjects.Add(go);

        commandNum++;
        paused = true;
    }

    private void AnimateRotation()
    {
        float rotationAmount = ROTATION_SPEED * Time.deltaTime;
        transform.Rotate(rotationAxis, rotationAmount);
        rotationCounter += rotationAmount;

        if (rotationCounter >= rotationGoal)
        {
            if (rotationCounter > rotationGoal)
            {
                // round euler values to nearest multiple of 90 to prevent angle drifting
                Vector3 angles = transform.rotation.eulerAngles;
                angles.x = 90 * (int)Mathf.Round(angles.x / 90.0f);
                angles.y = 90 * (int)Mathf.Round(angles.y / 90.0f);
                angles.z = 90 * (int)Mathf.Round(angles.z / 90.0f);
                transform.rotation = Quaternion.Euler(angles);
            }
            commandNum++;
            paused = true;
        }
    }

    public override void ApplyBox(Box box)
    {
        Clear();
        foreach (Face face in box.faces)
        {
            if (face.stamps.Count > 0)
            {
                GameObject currentFace = faces[face.side];

                foreach (Stamp stamp in face.stamps)
                {
                    GameObject go = GameObject.Instantiate<GameObject>(stampPrefab);
                    go.transform.SetParent(currentFace.transform, false);
                    go.transform.Rotate(Vector3.down, stamp.rotation);
                    stampObjects.Add(go);
                }
            }
        }
    }

    public override void AnimateBox(List<VisualBox.Command> commands, UnityAction onFinishAnimation)
    {
        transform.localRotation = Quaternion.identity;
        Clear();
        FinishAnimation = onFinishAnimation;
        animationCommands = commands;
        commandNum = 0;
        pauseTimer = 0f;
        paused = true;
        animating = true;
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
