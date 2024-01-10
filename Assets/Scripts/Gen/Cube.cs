
using UnityEngine;

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
                0, 3, 2,
                0, 1, 3
            },
            Vertices = new[]
            {
                new Vector3(0, 1, 0), new Vector3(1, 1, 0),
                new Vector3(0, 0, 0), new Vector3(1, 0, 0),
            },
        };
        static readonly CubeFaceData CUBE_FACE_BACK = new()
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

        #endregion

        /// <summary>
        /// FACES:
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        /// ^1 TOP, ^2 BOTTOM, ^3 RIGHT, ^4 LEFT, ^5 FORWARD, ^6 BACK
        public static CubeData GenerateCube(int faces = 63)
        {
            var cube = new CubeData().Init();

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


        #region Private

        private static CubeData Init(this CubeData cube)
        {
            cube = new CubeData
            {
                Triangles = new int[0],
                Vertices = new Vector3[0],
                Uvs = new Vector2[0]
            };

            return cube;
        }

        private static CubeData Add(this CubeData cube, CubeFaceData face)
        {
            var triangles = new int[cube.Triangles.Length + face.Triangles.Length];
            var vertices = new Vector3[cube.Vertices.Length + face.Vertices.Length];
            //var uvs = new Vector2[cube.Uvs.Length + face.Uvs.Length];

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
                vertices[v] = face.Vertices[v - cube.Vertices.Length];
            }

            cube.Triangles = triangles;
            cube.Vertices = vertices;

            return cube;
        }

        #endregion
    }

    public struct CubeData
    {
        public int[] Triangles;
        public Vector3[] Vertices;
        public Vector2[] Uvs;
    }

    public struct CubeFaceData
    {
        public int[] Triangles;
        public Vector3[] Vertices;
        public Vector2[] Uvs;
    }
}