using Assets.Scripts.Scriptables;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class BiomeCreator : MonoBehaviour
    {
        [SerializeField]
        private float oceanHeight = 0.33f;

        [SerializeField]
        private string fileName;

        [SerializeField]
        private List<BiomeSetting> biomesSettings;

        private void OnValidate()
        {
            biomesSettings.ForEach(e => e.ApplyOceanHeight(oceanHeight));
        }

        public void Save()
        {
            var (_fileName, _fileExtension, _, json) = jsonContent();

            var path = fileName is "" ? 
                EditorUtility.SaveFilePanel("Save Biomes File", "/Assets/Biomes", _fileName, _fileExtension) : fileName;

            Debug.Log(path);

            File.WriteAllText(path, json);

            AssetDatabase.Refresh();
        }
        
        public void SaveAs()
        {
            var (_fileName, _fileExtension, _, json) = jsonContent();

            var path = EditorUtility.SaveFilePanel("Save Biomes File", "/Assets/Biomes", _fileName, _fileExtension);

            File.WriteAllText(path, json);

            AssetDatabase.Refresh();
        }

        (string _fileName, string _fileExtension, string _filePath, string _fileJson) jsonContent()
        {
            var _fileName = fileName is not "" ? Path.GetFileName(fileName).Replace(".biomes.json", "") : "default_biomes";
            var _fileExtension = "biomes.json";
            var _filePath = fileName is not "" ? Path.GetDirectoryName(fileName) : "";

            //Debug.Log($"{_fileName} {_fileExtension} {_filePath}");

            List<int> ids = new List<int>();

            biomesSettings.ForEach(e => {
                e.SetFileName(_fileName);

                Debug.Log(e.Id);

                if (ids.Contains(e.Id))
                {
                    e.ApplyId();

                }

                ids.Add(e.Id);
            });

            var json = JsonConvert.SerializeObject(biomesSettings);

            return (_fileName, _fileExtension, _filePath, json);
        }

        public void Load()
        {
            var path = EditorUtility.OpenFilePanel("Load Biomes File", "", "biomes.json");

            var fileContent = File.ReadAllText(path);

            biomesSettings = JsonConvert.DeserializeObject<List<BiomeSetting>>(fileContent);

            fileName = path;
        }
    }
}