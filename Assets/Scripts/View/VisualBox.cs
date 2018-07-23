using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class VisualBox : MonoBehaviour
{

    public abstract void ApplyBox(Box box, bool isTemplate = false);

    public abstract void Clear();
}
