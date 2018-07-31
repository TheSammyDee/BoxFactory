using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ButtonPush3D))]
public class ButtonPush3DEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        ButtonPush3D target3DButton = (ButtonPush3D)target;

        target3DButton.textOffset = EditorGUILayout.IntField("Text offset", target3DButton.textOffset);

        base.OnInspectorGUI();
    }
}
