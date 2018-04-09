using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class VisualBox : MonoBehaviour {

    public enum Command { Left90Y, Left90Z, Stamp };

    public abstract void ApplyBox(Box box);

    public abstract void AnimateBox(List<VisualBox.Command> commands, UnityAction onFinishAnimation);

    public abstract void Clear();
}
