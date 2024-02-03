using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class BaseData
    {
        // MESH ARRAY
        public int[] Triangles { get; set; } = new int[0];
        public Vector3[] Vertices { get; set; } = new Vector3[0];
        public Vector2[] Uvs { get; set; } = new Vector2[0];

        // LISTS
        public List<int> TrianglesList { get; set; } = new List<int>();
        public List<Vector3> VerticesList { get; set; } = new List<Vector3>();
        public List<Vector2> UvsList { get; set; } = new List<Vector2>();

        // POSITION
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }

        // BLOCK DATA
        public Dictionary<Vector3Int,string> BlockData { get; set; }

        // OUTRAS CONFIGS
    }
}
