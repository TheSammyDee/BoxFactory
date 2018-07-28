using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxAnimator : MonoBehaviour
{
    private Transform boxTransform;
    private VisualBox visualBox;
    private VisualBox invisiBox;
    private Box box;

    private float pauseTimer;
    private bool paused;
    private bool animating;
    private List<Box.Command> animationCommands;
    private int commandNum;
    private float rotationCounter;
    private float rotationGoal;
    private Vector3 rotationAxis;

    private UnityAction OnFinishAnimation;

    public void Initialize(VisualBox invisiBoxPrefab)
    {
        invisiBox = GameObject.Instantiate(invisiBoxPrefab);
    }

    public void AnimateBox(Transform boxViewTransform, List<Box.Command> commands, UnityAction onFinishAnimation)
    {
        boxTransform = boxViewTransform;
        boxTransform.localRotation = Quaternion.identity;
        invisiBox.transform.SetParent(boxTransform, false);
        invisiBox.transform.localPosition = Vector3.zero;
        invisiBox.transform.localRotation = Quaternion.identity;
        invisiBox.transform.localScale = Vector3.one;
        OnFinishAnimation = onFinishAnimation;
        animationCommands = commands;
        commandNum = 0;
        pauseTimer = 0f;
        paused = true;
        animating = true;

        box = new Box();
    }

    private void Update()
    {
        if (animating)
        {
            if (paused)
            {
                if (pauseTimer >= Config.Instance.AnimationPauseTime)
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
                if (animationCommands[commandNum] == Box.Command.Stamp)
                {
                    invisiBox.ApplyBox(box);
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

    private void PrepAnimation(Box.Command command)
    {
        rotationCounter = 0f;
        switch (command)
        {
            case Box.Command.Left90Y:
                rotationGoal = 90f;
                rotationAxis = boxTransform.InverseTransformDirection(Vector3.up);
                box.RotateYLeft();
                break;
            case Box.Command.Left90Z:
                rotationGoal = 90f;
                rotationAxis = boxTransform.InverseTransformDirection(Vector3.forward);
                box.RotateZLeft();
                break;
            case Box.Command.Stamp:
                box.Stamp();
                break;
        }
    }

    private void AnimateRotation()
    {
        float rotationAmount = Config.Instance.AnimationRotationSpeed * Time.deltaTime;
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

    private void FinishAnimation()
    {
        invisiBox.Clear();
        invisiBox.transform.SetParent(null);

        if (OnFinishAnimation != null)
            OnFinishAnimation();
    }
}
