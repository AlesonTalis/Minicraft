using Assets.Scripts.CE;
using Assets.Scripts.Enums;
using Assets.Scripts.Gen;
using Assets.Scripts.Utils;
using Assets.Scripts.Map;
using Assets.Scripts.Noise;
using Assets.Scripts.Scriptables;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Model;
using Assets.Scripts.Concurents;
using UnityEditor.Rendering;

namespace Assets.Scripts
{
    public class Preview : MonoBehaviour
    {
        [Header("Enums")]
        public GenType genType;
        public PreviewType previewType;
        public ModeType mode;
        public ushort blockId = 3;


        [Header("Settings")]
        public string globalSeed = "26041996";
        public MapSettings heightMapSettings = new ()
        {
            octaves = 4,
        };
        public MapSettings heatMapSettings = new ()
        {
            octaves = 4,
        };
        public MapSettings humidityMapSettings = new ()
        {
            octaves = 4,
        };
        [ReadOnly]
        public BiomeSetting[] biomes;
        [ReadOnly]
        public ItemSettings[] items;


        [Header("Preview")]
        public int mapPreviewSize = 128;
        public Vector2 mapPreviewOffset;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public GameObject chunkPreview;
        public Material defaultMaterial;
        public Gradient mapPreviewHeatGradient;
        public Gradient mapPreviewHumidityGradient;


        public void Generate()
        {
            Debug.Log("Generating...");
            FillItemSettingsDictionary();

            if (mode == ModeType.Mesh)
            {

                switch (genType)
                {
                    case GenType.Cube: GenerateCube(); break;
                    case GenType.SubChunk: GenerateSubChunk(); break;
                    case GenType.Chunk: GenerateChunk(); break;
                }

            }
            else if (mode == ModeType.Map)
            {

                switch (previewType)
                {
                    case PreviewType.HeightMap: GenerateHeightMap(); break;
                    case PreviewType.HeatMap: GenerateHeatMap(); break;
                    case PreviewType.HumidityMap: GenerateHumidityMap(); break;
                    case PreviewType.BiomeMap: GenerateBiomeMap(); break;
                }

            }
        }

        //public void LoadBiomesFolder(string[] files)
        //{
        //    //List<BiomeSettingSC> biomes = new List<BiomeSettingSC>();
        //    //
        //    //foreach (var file in files)
        //    //{
        //    //    try
        //    //    {
        //    //        var biome = (BiomeSettingSC)AssetDatabase.LoadAssetAtPath<BiomeSettingSC>(file);
        //    //
        //    //        biomes.Add(biome);
        //    //    }
        //    //    catch (System.Exception ex) {
        //    //        Debug.LogError($"LOAD_BIOME_ERROR: \n\"{file}\"\n\n{ex.Message}");
        //    //    }
        //    //}
        //    //
        //    //this.biomes = biomes.ToArray();
        //}
        
        public void LoadBiomesFolder(string path)
        {
            Debug.Log(path);
            var file = File.ReadAllText(path);
            this.biomes = JsonConvert.DeserializeObject<BiomeSetting[]>(file);
        }
        
        public void LoadBlocksFolder(string path)
        {
            Debug.Log(path);
            var file = File.ReadAllText(path);
            this.items = JsonConvert.DeserializeObject<ItemSettings[]>(file);
        }

        void FillItemSettingsDictionary()
        {
            ItemsSettingsDictionary.items.Clear();

            for (int i = 0; i < items.Length; i++)
            {
                ItemsSettingsDictionary.items.TryAdd(items[i].itemId.ToString(), items[i]);
            }
        }

        #region <MESH>

        void GenerateCube()
        {
            var cube = Cube.GenerateCube(63,blockId);
            var mesh = Cube.GenerateCubeMesh(cube);

            meshFilter.sharedMesh = mesh;
        }

        void GenerateSubChunk()
        {
            var subchunkBlockArray = SubChunk.SetFilledSubChunk(1,true);
            var subchunk = SubChunk.GetSubChunkData(subchunkBlockArray);
            var mesh = subchunk.GetMeshData();

            meshFilter.sharedMesh = mesh;
        }

        void GenerateChunk()
        {
            ChunkData chunk = new ChunkData();
            var heightMap = HeightmapNoise.GenerateHeightMap();
            chunk.SetChunkDataHeightMap(heightMap);
            chunk.GetChunkData();

            ClearObjects(chunkPreview.transform);

            for (int i = 0; i < chunk.subChunks.Length; i++)
            {
                var pos = chunk.subChunks[i].GetPos();
                GameObject go = new GameObject($"CHUNK {pos}");

                go.transform.localPosition = pos;
                go.transform.SetParent(chunkPreview.transform);

                var renderer = go.AddComponent<MeshRenderer>();
                var filter = go.AddComponent<MeshFilter>();

                var mesh = chunk.subChunks[i].GetMeshData();

                filter.sharedMesh = mesh;
                renderer.sharedMaterial = defaultMaterial;
            }
        }

        void ClearObjects(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
#if UNITY_EDITOR
                DestroyImmediate(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }
        }

        #endregion

        #region <MAP>

        SeedValues GetSeeds() => MapGen.GenerateSeeds(globalSeed.GetHashCode());

        void GenerateHeightMap()
        {
            var heightMap = MapGen.GenerateHeightMap(mapPreviewSize, mapPreviewOffset, heightMapSettings.ApplySeed(GetSeeds().heightMapSeed));

            var texture = MapPreview.GenerateTextureFromHeightMap(heightMap);

            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        void GenerateHeatMap()
        {
            var heatMap = MapGen.GenerateHeatMap(mapPreviewSize, mapPreviewOffset, heatMapSettings.ApplySeed(GetSeeds().heatMapSeed));

            var texture = MapPreview.GenerateTextureForGradientColors(heatMap, mapPreviewHeatGradient);

            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        void GenerateHumidityMap()
        {
            var humidity = MapGen.GenerateHumidityMap(mapPreviewSize, mapPreviewOffset, humidityMapSettings.ApplySeed(GetSeeds().humiditySeed));

            var texture = MapPreview.GenerateTextureForGradientColors(humidity, mapPreviewHumidityGradient);

            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        void GenerateBiomeMap()
        {
            var seeds = GetSeeds();

            var heightMap = MapGen.GenerateHeightMap(mapPreviewSize, mapPreviewOffset, heightMapSettings.ApplySeed(seeds.heightMapSeed));
            var heatMap = MapGen.GenerateHeatMap(mapPreviewSize, mapPreviewOffset, heatMapSettings.ApplySeed(seeds.heatMapSeed));
            var humidity = MapGen.GenerateHumidityMap(mapPreviewSize, mapPreviewOffset, humidityMapSettings.ApplySeed(seeds.humiditySeed));

            var biomeMap = MapGen.GenerateBiomeMap(heightMap, heatMap, humidity, biomes);

            var texture = MapPreview.GenerateTextureFromBiomeMap(biomeMap, biomes.GetColorDictionary());

            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        #endregion
    }
}