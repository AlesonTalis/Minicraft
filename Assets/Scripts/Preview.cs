using Assets.Scripts.CE;
using Assets.Scripts.Enums;
using Assets.Scripts.Gen;
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

namespace Assets.Scripts
{
    public class Preview : MonoBehaviour
    {
        [Header("Enums")]
        public GenType genType;
        public PreviewType previewType;
        public ModeType mode;


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

        void GenerateCube()
        {
            var cube = Cube.GenerateCube();
            var mesh = Cube.GenerateCubeMesh(cube);

            meshFilter.sharedMesh = mesh;

            //Debug.Log(string.Join(", ", cube.Triangles));
        }

        #region <MESH>

        void GenerateSubChunk()
        {
            var subchunkBlockArray = SubChunk.SetFilledSubChunk(1);
            var subchunk = SubChunk.GetSubChunkData(subchunkBlockArray);
            var mesh = SubChunk.GetSubChunkMesh(subchunk);

            meshFilter.sharedMesh = mesh;

            //Debug.Log(string.Join(", ", subchunk.TrianglesList));
            //Debug.Log(string.Join(", ", subchunk.VerticesList));
        }

        void GenerateChunk()
        {
            var heightMap = HeightmapNoise.GenerateHeightMap();
            var chunk = Chunk.SetChunkDataHeightMap(heightMap);

            ClearObjects(chunkPreview.transform);

            for (int i = 0; i < chunk.subChunks.Length; i++)
            {
                var pos = chunk.subChunks[i].GetPos();
                GameObject go = new GameObject($"CHUNK {pos}");

                go.transform.localPosition = pos;
                go.transform.SetParent(chunkPreview.transform);

                var renderer = go.AddComponent<MeshRenderer>();
                var filter = go.AddComponent<MeshFilter>();

                var mesh = SubChunk.GetSubChunkMesh(chunk.subChunks[i]);

                filter.sharedMesh = mesh;
                renderer.sharedMaterial = defaultMaterial;

                Debug.Log(string.Join(", ", chunk.subChunks[i].TrianglesList));
                Debug.Log(string.Join(", ", chunk.subChunks[i].VerticesList));
            }

            //Debug.Log(string.Join(", ", chunk.TrianglesList));
            //Debug.Log(string.Join(", ", chunk.VerticesList));
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

        }

        #endregion
    }
}