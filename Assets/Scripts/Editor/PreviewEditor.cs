using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Preview))]
public class PreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Preview preview = (Preview)target;

        if (GUILayout.Button("Generate"))
        {
            preview.Generate();
        }
    }
}
