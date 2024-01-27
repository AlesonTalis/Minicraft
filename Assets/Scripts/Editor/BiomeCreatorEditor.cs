using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BiomeCreator))]
public class BiomeCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var biomeCreator = (BiomeCreator)target;

        if (GUILayout.Button("Save"))
        {
            biomeCreator.Save();
        }

        if (GUILayout.Button("Save As"))
        {
            biomeCreator.SaveAs();
        }

        if (GUILayout.Button("Load file"))
        {
            biomeCreator.Load();
        }
    }
}

