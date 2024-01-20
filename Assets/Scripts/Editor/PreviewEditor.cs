using Assets.Scripts;
using Assets.Scripts.Scriptables;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        if (GUILayout.Button("Load Biomes Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Load Biomes Folder", "", "");
            string assets = Application.dataPath;

            Debug.Log(path);
            Debug.Log(assets);

            //preview.LoadBiomesFolder(files.Where(f => f.Contains(".meta") == false).ToArray());
            preview.LoadBiomesFolder(path.Replace(assets,"") + "/");
        }
    }
}
