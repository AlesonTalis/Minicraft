
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class CubeFaceData
    {
        public int[] Triangles {  get; set; } = new int[0];
        public Vector3[] Vertices { get; set; } = new Vector3[0];
        public Vector2[] Uvs { get; set; } = new Vector2[0];

        public List<int> TrianglesList { get; set; } = new List<int>();
        public List<Vector3> VerticesList { get; set; } = new List<Vector3>();
        public List<Vector2> UvsList { get; set; } = new List<Vector2>();
    }
}