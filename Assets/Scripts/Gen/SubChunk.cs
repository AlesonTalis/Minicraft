using Assets.Scripts.CE;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class SubChunk
    {
        const int CHUNK_SIZE = 16;

        /** 
         * 8bits
         * ^8 tem ou não bloco (para checar blocos vizinhos)
         * ^7 e ^6 tipo do bloco (transparente, semibloco, especial)
         * ^1 - ^5 id do bloco
         */

        /// <summary>
        /// 1111 1111, onde ^8 = transparencia
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        public static ushort[,,] SetFilledSubChunk(ushort blockId, bool stripes = false)
        {
            int size = CHUNK_SIZE + 1;
            var chunkArray = new ushort[size, size, size];

            chunkArray.Loop((i, x, y, z) =>
            {
                if ((x > 0 && y > 0 && z > 0) && (x < size - 1 && y < size - 1 && z < size - 1))
                {
                    chunkArray[x,y,z] = blockId;
                }
                else
                {
                    chunkArray[x,y,z] = (ushort)(blockId + 128);
                }
            }, "GENERATE SUBCHUNK");

            if (stripes)
            {
                chunkArray = StripeChunk(chunkArray);
            }

            return chunkArray;
        }

        public static ushort[,,] StripeChunk(ushort[,,] array)
        {
            array.Loop((i, x, y, z) => { 
                if (y % 2 == 0)
                {
                    array[x,y,z] = 129;
                }
            }, "Stripes");

            return array;
        }

        // cubos:
        public static SubChunkData GetSubChunkData(ushort[,,] blockArray)
        {
            var subChunkData = new SubChunkData();

            blockArray.Loop((i, x, y, z) =>
            {
                var blockid = blockArray[x,y,z];

                var vizinhos = blockArray.GetNeighbors(x, y, z);

                if ((blockid & 128) == 0)
                {
                    var cube = Cube.GenerateCube(vizinhos);

                    subChunkData = subChunkData.Add(cube, x - 1, y - 1, z - 1);// pode ser vazio
                }
            }, "ConvertToVerticeData");

            return subChunkData;
        }

        public static Mesh GetSubChunkMesh(SubChunkData subChunkData)
        {
            var mesh = new Mesh()
            {
                vertices = subChunkData.VerticesList.ToArray(),
                triangles = subChunkData.TrianglesList.ToArray(),
            };

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}