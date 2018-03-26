using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualBox : MonoBehaviour {

    public abstract void ApplyBox(Box box);

    public abstract void Clear();
}
