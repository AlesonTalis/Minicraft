using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.CE
{
    internal static class CubeDataCE
    {
        private static readonly float HORIZONTAL_OFFSET = 1 / (1024f / 16f);
        private static readonly float VERTICAL_OFFSET = 1 / (512f / 16f);
        private static readonly float OFF_X = 1f / 1024f;
        private static readonly float OFF_Y = 1f / 512f;

        internal static BaseData Init(this BaseData cube)
        {
            cube = new BaseData
            {
                Triangles = new int[0],
                Vertices = new Vector3[0],
                Uvs = new Vector2[0]
            };

            return cube;
        }

        internal static void Add(this BaseData cube, BaseData face, Vector3 offset = default, Vector2Int faceId = default)
        {
            var triangles = new int[cube.Triangles.Length + face.Triangles.Length];
            var vertices = new Vector3[cube.Vertices.Length + face.Vertices.Length];
            var uvs = new Vector2[cube.Uvs.Length + face.Uvs.Length];

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

            if (faceId != default)
            {
                var faceUvs = SetUvPosition(face.Uvs, faceId);

                for (int v = 0; v < cube.Uvs.Length; v++)
                {
                    uvs[v] = cube.Uvs[v];
                }

                for (int v = cube.Uvs.Length; v < face.Uvs.Length + cube.Uvs.Length; v++)
                {
                    uvs[v] = faceUvs[v - cube.Uvs.Length];
                }
            }

            cube.Triangles = triangles;
            cube.Vertices = vertices;
            cube.Uvs = uvs;

            //return cube;
        }

        internal static Vector2[] SetUvPosition(Vector2[] uvBase, Vector2Int pos)
        {
            int y = pos.y;
            int x = pos.x;
            var newUvs = new Vector2[uvBase.Length];

            for (int i = 0; i < uvBase.Length; i++)
            {
                if (uvBase[i].x == 1)
                {
                    newUvs[i].x = HORIZONTAL_OFFSET;
                }
                if (uvBase[i].y == 1)
                {
                    newUvs[i].y = VERTICAL_OFFSET;
                }

                newUvs[i].x += HORIZONTAL_OFFSET * x;
                newUvs[i].y += VERTICAL_OFFSET * y;

                //uvBase[i].x += OFF_X * 0.5f;
                //uvBase[i].y += OFF_Y * 0.5f;
            }

            return newUvs;
        }

        internal static BaseData AddList(this BaseData cube, BaseData face, Vector3 offset = default)
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

            for (int i = 0; i < face.UvsList.Count; i++)
            {
                cube.UvsList.Add(face.UvsList[i]);
            }

            return cube;
        }

        internal static BaseData AddListFromArray(this BaseData cube, BaseData face, Vector3 offset = default)
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

            for (int i = 0; i < face.Uvs.Length; i++)
            {
                cube.UvsList.Add(face.Uvs[i]);
            }

            return cube;
        }
    }
}
