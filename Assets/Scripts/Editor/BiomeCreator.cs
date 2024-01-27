using Assets.Scripts.Scriptables;
using Assets.Scripts.Utils;
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
        private BiomeSetting? loadedBiomeSetting = new BiomeSetting();


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
            settings.biomeColor = ColorPicker(new Vector2(0, 60), "BiomeColor", settings.biomeColor);
            settings.biomeHeightRange = RangePicker(new Vector2(0, 90), "HeightLimits", settings.biomeHeightRange);
            settings.biomeTemperatureRange = RangePicker(new Vector2(0, 120), "HeatLimits", settings.biomeTemperatureRange);
            settings.biomeHumidityRange = RangePicker(new Vector2(0, 150), "HumidLimits", settings.biomeHumidityRange);

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

        private Color32 ColorPicker(Vector2 pos, string title, Color32 color)
        {
            GUI.BeginGroup(new Rect(pos, new Vector2(300, 20)));

            GUI.Label(new Rect(0, 0, 100, 20), title);

            GUI.BeginGroup(new Rect(100, 0, 200, 20));
            color = EditorGUILayout.ColorField(color);

            GUI.EndGroup();
            GUI.EndGroup();

            return color;
        }

        private Vector2 RangePicker(Vector2 pos, string title, Vector2 value)
        {
            GUI.BeginGroup(new Rect(pos, new Vector2(300, 20)));

            GUI.Label(new Rect(0, 0, 100, 20), title);

            GUI.BeginClip(new Rect(100, 0, 200, 20));

            float min = value.x, max = value.y;

            GUI.Label(new Rect(0, 0, 40, 20), min.ToString("F2"));
            float _min = GUI.HorizontalSlider(new Rect(50, 0, 50, 20), min, 0, 1);
            
            float _max = GUI.HorizontalSlider(new Rect(110, 0, 50, 20), max, 0, 1);
            GUI.Label(new Rect(170, 0, 40, 20), max.ToString("F2"));
            
            GUI.EndClip();
            GUI.EndGroup();

            value = new Vector2()
            {
                x = Mathf.Clamp(_min, 0f, max),
                y = Mathf.Clamp(_max, min, 1f),
            };

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

            if (file is null) return;

            var fileData = File.ReadAllText(file);

            loadedBiomeSetting = JsonConvert.DeserializeObject<BiomeSetting>(fileData);
        }
        void SaveBiomeFile()
        {
            string fileData = JsonConvert.SerializeObject(loadedBiomeSetting);
            string fileName = loadedBiomeSetting.GetFileName();
            string path = Application.dataPath + saveLocation + fileName;

            File.WriteAllText(path, fileData);

            AssetDatabase.Refresh();
        }
        void CreateBiomeFile()
        {
            loadedBiomeSetting = new BiomeSetting()
            {
                biomeHeightRange = new Vector2(0.25f, 0.75f)
            };
        }
    }
}
