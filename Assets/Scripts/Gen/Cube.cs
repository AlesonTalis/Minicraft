
using UnityEngine;
using Assets.Scripts.CE;
using Assets.Scripts.Model;
using System.Collections.Concurrent;
using Assets.Scripts.Concurents;

namespace Assets.Scripts.Gen
{
    public static class Cube
    {
        #region LOOKUP TABLE FACES

        // LOOKUP TABLE CUBE
        static readonly CubeFaceData CUBE_FACE_TOP = new()
        {
            Triangles = new[]
            {
                0, 3, 2,
                0, 1, 3
            },
            Vertices = new[]
            {
                new Vector3(0, 1, 0), new Vector3(0, 1, 1),
                new Vector3(1, 1, 0), new Vector3(1, 1, 1),
            },
            Uvs = new[]
            {
                new Vector2(0,0), new Vector2(0,1),
                new Vector2(1,0), new Vector2(1,1),
            },
        };
        static readonly CubeFaceData CUBE_FACE_BOTTOM = new()
        {
            Triangles = new[]
            {
                0, 2, 3,
                0, 3, 1
            },
            Vertices = new[]
            {
                new Vector3(0, 0, 0), new Vector3(0, 0, 1),
                new Vector3(1, 0, 0), new Vector3(1, 0, 1),
            },
            Uvs = new[]
            {
                new Vector2(0,0), new Vector2(0,1),
                new Vector2(1,0), new Vector2(1,1),
            },
        };
        static readonly CubeFaceData CUBE_FACE_RIGHT = new()
        {
            Triangles = new[]
            {
                0, 3, 2,
                0, 1, 3
            },
            Vertices = new[]
            {
                new Vector3(1, 1, 0), new Vector3(1, 1, 1),
                new Vector3(1, 0, 0), new Vector3(1, 0, 1),
            },
            Uvs = new[]
            {
                new Vector2(1,1), new Vector2(0,1),// 0 1 // 10 11
                new Vector2(1,0), new Vector2(0,0),// 2 3 // 00 01
            },
        };
        static readonly CubeFaceData CUBE_FACE_LEFT = new()
        {
            Triangles = new[]
            {
                0, 2, 3,
                0, 3, 1
            },
            Vertices = new[]
            {
                new Vector3(0, 1, 0), new Vector3(0, 1, 1),
                new Vector3(0, 0, 0), new Vector3(0, 0, 1),
            },
            Uvs = new[]
            {
                new Vector2(1,1), new Vector2(0,1),// 0 1 // 10 11
                new Vector2(1,0), new Vector2(0,0),// 2 3 // 00 01
            },
        };
        static readonly CubeFaceData CUBE_FACE_FORWARD = new()
        {
            Triangles = new[]
            {
                0, 2, 3,
                0, 3, 1
            },
            Vertices = new[]
            {
                new Vector3(0, 1, 1), new Vector3(1, 1, 1),
                new Vector3(0, 0, 1), new Vector3(1, 0, 1),
            },
            Uvs = new[]
            {
                new Vector2(0,1), new Vector2(1,1),
                new Vector2(0,0), new Vector2(1,0),
            },
        };
        static readonly CubeFaceData CUBE_FACE_BACK = new()
        {
            Triangles = new[]
            {
                0, 3, 2,
                0, 1, 3
            },
            Vertices = new[]
            {
                new Vector3(0, 1, 0), new Vector3(1, 1, 0),
                new Vector3(0, 0, 0), new Vector3(1, 0, 0),
            },
            Uvs = new[]
            {
                new Vector2(0,1), new Vector2(1,1),
                new Vector2(0,0), new Vector2(1,0),
            },
        };

        #endregion

        /// <summary>
        /// FACES:
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        /// ^1 TOP, ^2 BOTTOM, ^3 RIGHT, ^4 LEFT, ^5 FORWARD, ^6 BACK
        public static CubeData GenerateCube(int faces = 63, ushort blockId = 1, ushort biome = 0)
        {
            var cube = new CubeData();
            var faceId = new Vector2Int(3, 6);

            if ((faces & 1) != 0) cube.Add(CUBE_FACE_TOP, default, GetFaceId(0,blockId,biome));
            if ((faces & 2) != 0) cube.Add(CUBE_FACE_BOTTOM, default, GetFaceId(1, blockId, biome));
            if ((faces & 4) != 0) cube.Add(CUBE_FACE_RIGHT, default, GetFaceId(2,blockId,biome));
            if ((faces & 8) != 0) cube.Add(CUBE_FACE_LEFT, default, GetFaceId(3,blockId,biome));
            if ((faces & 16) != 0) cube.Add(CUBE_FACE_FORWARD, default, GetFaceId(4,blockId,biome));
            if ((faces & 32) != 0) cube.Add(CUBE_FACE_BACK, default, GetFaceId(5,blockId,biome));

            return cube;
        }

        static Vector2Int GetFaceId(ushort faceId, ushort blockId, ushort biomeId)
        {
            if (ItemsSettingsDictionary.items.TryGetValue($"{blockId}", out ItemSettings item) is false) return new Vector2Int(26, 6);

            return item.itemImageFaces[faceId].GetVector2Int();
        }

        public static Mesh GenerateCubeMesh(CubeData cube)
        {
            var mesh = new Mesh
            {
                vertices = cube.Vertices,
                uv = cube.Uvs,
                triangles = cube.Triangles,
            };

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

    }
}