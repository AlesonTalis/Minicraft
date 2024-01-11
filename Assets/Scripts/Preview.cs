using Assets.Scripts.Enums;
using Assets.Scripts.Gen;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Preview : MonoBehaviour
    {
        public GenType genType;

        public MeshFilter meshFilter;


        public void Generate()
        {
            Debug.Log("Generating...");

            switch (genType)
            {
                case GenType.Cube: GenerateCube(); break;
                case GenType.SubChunk: GenerateSubChunk(); break;
                case GenType.Chunk: GenerateChunk(); break;
            }
        }

        void GenerateCube()
        {
            var cube = Cube.GenerateCube();
            var mesh = Cube.GenerateCubeMesh(cube);

            meshFilter.sharedMesh = mesh;

            //Debug.Log(string.Join(", ", cube.Triangles));
        }

        void GenerateSubChunk()
        {
            var subchunkBlockArray = SubChunk.SetFilledSubChunk(1, true);
            var subchunk = SubChunk.GetSubChunkData(subchunkBlockArray);
            var mesh = SubChunk.GetSubChunkMesh(subchunk);

            meshFilter.sharedMesh = mesh;

            //Debug.Log(string.Join(", ", subchunk.TrianglesList));
            //Debug.Log(string.Join(", ", subchunk.VerticesList));
        }

        void GenerateChunk()
        {
            var chunkBlockArray = Chunk.SetFilledChunk(1, true);
            var chunk = Chunk.GetChunkData(chunkBlockArray);
            var mesh = Chunk.GetChunkMesh(chunk);

            meshFilter.sharedMesh = mesh;

            //Debug.Log(string.Join(", ", chunk.TrianglesList));
            //Debug.Log(string.Join(", ", chunk.VerticesList));
        }
    }
}