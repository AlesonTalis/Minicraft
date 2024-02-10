using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using System;
using UnityEngine;

namespace Assets.Scripts.CE
{
    internal static class CubeDataCE
    {
        const int TEXTURE_WIDTH = 512;
        const int TEXTURE_HEIGHT = 512;
        const int BLOCK_SIZE = 16;

        private static readonly int HORIZONTAL_BLOCKS_COUNT = (TEXTURE_WIDTH / BLOCK_SIZE);
        private static readonly int VERTICAL_BLOCKS_COUNT = (TEXTURE_WIDTH / BLOCK_SIZE);

        private static readonly float HORIZONTAL_OFFSET = 1 / ((float)TEXTURE_WIDTH / (float)BLOCK_SIZE);
        private static readonly float VERTICAL_OFFSET = 1 / ((float)TEXTURE_HEIGHT / (float)BLOCK_SIZE);

        private static readonly float OFF_X = 1f / TEXTURE_WIDTH;
        private static readonly float OFF_Y = 1f / TEXTURE_HEIGHT;

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

        [Obsolete]
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
        }
        
        internal static void Add(this BaseData cube, BaseData face, Vector3 offset = default, int faceId = 0)
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
        }

        [Obsolete]
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
            }

            return newUvs;
        }
        
        internal static Vector2[] SetUvPosition(Vector2[] uvBase, int pos)
        {
            var newUvs = new Vector2[uvBase.Length];
            int x = pos % HORIZONTAL_BLOCKS_COUNT;
            int y = pos / HORIZONTAL_BLOCKS_COUNT;

            for (int i = 0; i < uvBase.Length; i++)
            {
                newUvs[i].y = 1;
                if (uvBase[i].x == 1)
                {
                    newUvs[i].x = HORIZONTAL_OFFSET;
                }
                if (uvBase[i].y == 1)
                {
                    newUvs[i].y -= VERTICAL_OFFSET;
                }

                newUvs[i].x += HORIZONTAL_OFFSET * x;
                newUvs[i].y -= VERTICAL_OFFSET * y;
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
