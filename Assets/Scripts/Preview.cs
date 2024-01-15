using Assets.Scripts.CE;
using Assets.Scripts.Enums;
using Assets.Scripts.Gen;
using Assets.Scripts.Map;
using Assets.Scripts.Noise;
using System.Collections;
using Unity.VisualScripting;
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


        [Header("Preview")]
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public GameObject chunkPreview;
        public Material defaultMaterial;


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
                }

            }
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

        void GenerateHeightMap()
        {
            var seeds = MapGen.GenerateSeeds(globalSeed.GetHashCode());
            var heightMap = MapGen.GenerateHeightMap(128, Vector2.zero, heightMapSettings.ApplySeed(seeds.heightMapSeed));

            var texture = MapPreview.GenerateTextureFromHeightMap(heightMap);

            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        #endregion
    }
}