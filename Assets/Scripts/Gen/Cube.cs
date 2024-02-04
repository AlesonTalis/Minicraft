
using UnityEngine;
using Assets.Scripts.CE;
using Assets.Scripts.Model;

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
                new Vector2(1,0), new Vector2(1,1),
                new Vector2(0,0), new Vector2(0,1),
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
                new Vector2(1,0), new Vector2(1,1),
                new Vector2(0,0), new Vector2(0,1),
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
        public static CubeData GenerateCube(int faces = 63, ushort blockId = 1)
        {
            var cube = new CubeData();
            var faceId = new Vector2Int(11, 31);

            if ((faces & 1) != 0) cube.Add(CUBE_FACE_TOP, default, faceId);
            if ((faces & 2) != 0) cube.Add(CUBE_FACE_BOTTOM, default, faceId);
            if ((faces & 4) != 0) cube.Add(CUBE_FACE_RIGHT, default, faceId);
            if ((faces & 8) != 0) cube.Add(CUBE_FACE_LEFT, default, faceId);
            if ((faces & 16) != 0) cube.Add(CUBE_FACE_FORWARD, default, faceId);
            if ((faces & 32) != 0) cube.Add(CUBE_FACE_BACK, default, faceId);

            return cube;
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