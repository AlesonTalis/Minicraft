using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.CE
{
    internal static class CubeDataCE
    {
        internal static CubeData Init(this CubeData cube)
        {
            cube = new CubeData
            {
                Triangles = new int[0],
                Vertices = new Vector3[0],
                Uvs = new Vector2[0]
            };

            return cube;
        }

        internal static CubeData Add(this CubeData cube, CubeFaceData face, Vector3 offset = default)
        {
            var triangles = new int[cube.Triangles.Length + face.Triangles.Length];
            var vertices = new Vector3[cube.Vertices.Length + face.Vertices.Length];
            //var uvs = new Vector2[cube.Uvs.Length + face.Uvs.Length];

            if (offset == default)
                offset = Vector3.zero;

            // triangles
            for (int i = 0; i < cube.Triangles.Length; i++)
            {
                triangles[i] = cube.Triangles[i];
            }
            for (int i = cube.Triangles.Length; i < face.Triangles.Length + cube.Triangles.Length; i++)
            {
                triangles[i] = face.Triangles[i - cube.Triangles.Length] + cube.Vertices.Length;
            }

            // vertices
            for (int v = 0; v < cube.Vertices.Length; v++)
            {
                vertices[v] = cube.Vertices[v];
            }
            for (int v = cube.Vertices.Length; v < face.Vertices.Length + cube.Vertices.Length; v++)
            {
                vertices[v] = face.Vertices[v - cube.Vertices.Length] + offset;
            }

            cube.Triangles = triangles;
            cube.Vertices = vertices;

            return cube;
        }

        internal static CubeData AddList(this CubeData cube, CubeFaceData face, Vector3 offset = default)
        {
            int vertices = cube.VerticesList.Count;

            for (int i = 0; i < face.TrianglesList.Count; i++)
            {
                cube.TrianglesList.Add(face.TrianglesList[i] + vertices);
            }

            for (int i = 0; i < face.VerticesList.Count; i++)
            {
                cube.VerticesList.Add(face.VerticesList[i] + offset);
            }

            return cube;
        }

        internal static CubeData AddListFromArray(this CubeData cube, CubeFaceData face, Vector3 offset = default)
        {
            int vertices = cube.VerticesList.Count;

            for (int i = 0; i < face.Triangles.Length; i++)
            {
                cube.TrianglesList.Add(face.Triangles[i] + vertices);
            }

            for (int i = 0; i < face.Vertices.Length; i++)
            {
                cube.VerticesList.Add(face.Vertices[i] + offset);
            }

            return cube;
        }
    }
}
