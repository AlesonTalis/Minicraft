using Assets.Scripts.Scriptables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    public class BiomeCreator : EditorWindow
    {
        [MenuItem("Window/CreateBiome")]
        public static void Init()
        {
            var window = EditorWindow.CreateWindow<BiomeCreator>("BiomeCreator");
        }



        private string saveLocation = "/Biomes/";
        private BiomeSetting? loadedBiomeSetting = null;


        private void OnGUI()
        {
            EditorGUI.LabelField(new Rect(10, 10, 500, 20), "BiomeCreator");

            SetSaveLocation(new Vector2(10, 40));
            OpenSaveButtons(new Vector2(10, 70));

            HorizontalLine(new Vector2(10,100));

            BiomeEditor(new Vector2(10, 110));
        }

        private void SetSaveLocation(Vector2 pos)
        {
            GUI.BeginGroup(new Rect(pos, new Vector2(300, 20)));


            GUI.Label(new Rect(0, 0, 70, 20), "SaveLoc...");
            GUI.Label(new Rect(70, 0, 120, 20), saveLocation);

            if (GUI.Button(new Rect(200,0,100,20), "Change"))
            {
                ChangeSaveLocation();
            }


            GUI.EndGroup();
        }

        private void OpenSaveButtons(Vector2 pos)
        {
            GUI.BeginGroup(new Rect(pos, new Vector2(300, 20)));

            if (GUI.Button(new Rect(0, 0, 135, 20), "Open Biome File"))
            {
                OpenBiomeFile();
            }

            GUI.enabled = loadedBiomeSetting != null;
            if (GUI.Button(new Rect(135, 0, 135, 20), "Save Biome File"))
            {
                SaveBiomeFile();
            }
            GUI.enabled = true;

            if (GUI.Button(new Rect(270, 0, 30, 20), "+"))
            {
                CreateBiomeFile();
            }


            GUI.EndGroup();
        }

        private void HorizontalLine(Vector2 pos)
        {
            GUI.Box(new Rect(pos, new Vector2(300, 2)), "");
        }

        private void BiomeEditor(Vector2 pos)
        {
            if (loadedBiomeSetting.HasValue == false) return;

            GUI.BeginGroup(new Rect(pos, new Vector2(300, 1000)));

            var settings = loadedBiomeSetting.Value;

            settings.biomeName = Label(new Vector2(0, 0), "FileName", settings.biomeName);
            settings.biomeTitle = Label(new Vector2(0, 30), "BiomeTitle", settings.biomeTitle);

            // save

            loadedBiomeSetting = settings;

            GUI.EndGroup();
        }

        private string Label(Vector2 pos, string title, string value, bool multLine = false)
        {
            GUI.BeginGroup(new Rect(pos, new Vector2(300, multLine ? 60 : 20)));

            GUI.Label(new Rect(0, 0, 100, 20), title);

            value = GUI.TextField(new Rect(100, 0, 200, 20), value);

            GUI.EndGroup();

            return value;
        }

        void ChangeSaveLocation()
        {
            var path = EditorUtility.OpenFolderPanel("Change Biomes Save Location", "", "");
            var assets = Application.dataPath;

            saveLocation = path.Replace(assets, "") + "/";
        }
        void OpenBiomeFile()
        {
            var file = EditorUtility.OpenFilePanel("Open Biome File", "", "biome.json");

            var fileData = File.ReadAllText(file);

            loadedBiomeSetting = JsonConvert.DeserializeObject<BiomeSetting>(fileData);
        }
        void SaveBiomeFile()
        {
            string fileData = JsonConvert.SerializeObject(loadedBiomeSetting);
            string path = Application.dataPath + saveLocation + loadedBiomeSetting.Value.biomeName + ".biome.json";

            File.WriteAllText(path, fileData);

            AssetDatabase.Refresh();
        }
        void CreateBiomeFile()
        {
            loadedBiomeSetting = new BiomeSetting();
        }
    }
}
