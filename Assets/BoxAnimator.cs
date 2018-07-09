using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxAnimator : MonoBehaviour
{
    private const float PAUSE_TIME = 0.8f;
    private const int ROTATION_SPEED = 90;

    private Transform boxTransform;
    private VisualBox visualBox;
    private Box box;

    private float pauseTimer;
    private bool paused;
    private bool animating;
    private List<VisualBox.Command> animationCommands;
    private int commandNum;
    private float rotationCounter;
    private float rotationGoal;
    private Vector3 rotationAxis;

    private UnityAction FinishAnimation;

    // Use this for initialization
    void Start () {
		
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
                        boxTransform.localRotation = Quaternion.identity;
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
                    visualBox.ApplyBox(box);
                    commandNum++;
                    paused = true;
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
                rotationAxis = boxTransform.InverseTransformDirection(Vector3.up);
                box.RotateYLeft();
                break;
            case VisualBox.Command.Left90Z:
                rotationGoal = 90f;
                rotationAxis = boxTransform.InverseTransformDirection(Vector3.forward);
                box.RotateZLeft();
                break;
            case VisualBox.Command.Stamp:
                box.Stamp();
                break;
        }
    }

    private void AnimateRotation()
    {
        float rotationAmount = ROTATION_SPEED * Time.deltaTime;
        boxTransform.Rotate(rotationAxis, rotationAmount);
        rotationCounter += rotationAmount;

        if (rotationCounter >= rotationGoal)
        {
            if (rotationCounter > rotationGoal)
            {
                // round euler values to nearest multiple of 90 to prevent angle drifting
                Vector3 angles = boxTransform.rotation.eulerAngles;
                angles.x = 90 * (int)Mathf.Round(angles.x / 90.0f);
                angles.y = 90 * (int)Mathf.Round(angles.y / 90.0f);
                angles.z = 90 * (int)Mathf.Round(angles.z / 90.0f);
                boxTransform.rotation = Quaternion.Euler(angles);
            }

            commandNum++;
            paused = true;
        }
    }

    public void AnimateBox(Transform boxTransform, VisualBox visualBox, List<VisualBox.Command> commands, UnityAction onFinishAnimation)
    {
        this.boxTransform = boxTransform;
        this.visualBox = visualBox;

        this.boxTransform.localRotation = Quaternion.identity;
        FinishAnimation = onFinishAnimation;
        animationCommands = commands;
        commandNum = 0;
        pauseTimer = 0f;
        paused = true;
        animating = true;

        box = new Box();
        visualBox.Clear();
    }
}
