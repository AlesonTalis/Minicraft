
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
        };

        #endregion

        /// <summary>
        /// FACES:
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        /// ^1 TOP, ^2 BOTTOM, ^3 RIGHT, ^4 LEFT, ^5 FORWARD, ^6 BACK
        public static CubeData GenerateCube(int faces = 63)
        {
            var cube = new CubeData();

            if ((faces & 1) != 0) cube = cube.Add(CUBE_FACE_TOP);
            if ((faces & 2) != 0) cube = cube.Add(CUBE_FACE_BOTTOM);
            if ((faces & 4) != 0) cube = cube.Add(CUBE_FACE_RIGHT);
            if ((faces & 8) != 0) cube = cube.Add(CUBE_FACE_LEFT);
            if ((faces & 16) != 0) cube = cube.Add(CUBE_FACE_FORWARD);
            if ((faces & 32) != 0) cube = cube.Add(CUBE_FACE_BACK);

            return cube;
        }

        public static Mesh GenerateCubeMesh(CubeData cube)
        {
            var mesh = new Mesh
            {
                vertices = cube.Vertices,
                triangles = cube.Triangles
            };

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }

    }
}