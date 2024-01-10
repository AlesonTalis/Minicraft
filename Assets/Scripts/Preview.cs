using Assets.Scripts.Gen;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Preview : MonoBehaviour
    {
        public MeshFilter meshFilter;


        public void Generate()
        {
            Debug.Log("Generating...");

            var cube = Cube.GenerateCube();
            var mesh = Cube.GenerateCubeMesh(cube);

            meshFilter.sharedMesh = mesh;

            Debug.Log(string.Join(", ", cube.Triangles));
        }
    }
}