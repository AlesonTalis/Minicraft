﻿using Assets.Scripts.CE;
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
        public const int CHUNK_SIZE = 16;

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
        public static SubChunkData SetFilledSubChunk(ushort blockId, bool stripes = false)
        {
            int size = CHUNK_SIZE + 2;
            var chunkArray = new ushort[size, size, size];

            chunkArray.Loop((l) =>
            {
                chunkArray[l.x, l.y, l.z] = blockId;

                return null;
            }, "GENERATE SUBCHUNK");

            if (stripes)
            {
                chunkArray = StripeChunk(chunkArray);
            }

            SubChunkData subChunkData = new SubChunkData()
            {
                BlockArray = chunkArray,
            };

            return subChunkData;
        }

        public static ushort[,,] StripeChunk(ushort[,,] array)
        {
            array.Loop((l) => 
            { 
                if (l.y % 2 == 0)
                {
                    array[l.x,l.y,l.z] = 129;
                }

                return null;
            }, "Stripes");

            return array;
        }

        // cubos:
        public static BaseData GetSubChunkData(SubChunkData subChunkData)
        {
            subChunkData.BlockArray.Loop((l) =>
            {
                ushort blockid = subChunkData.BlockArray[l.x,l.y,l.z];

                var vizinhos = subChunkData.BlockArray.GetNeighbors(l.x, l.y, l.z);

                var cube = Cube.GenerateCube(vizinhos, blockid);

                subChunkData.Add(cube, l.x - 1, l.y - 1, l.z - 1);// pode ser vazio

                return null;
            }, "ConvertToVerticeData", new LoopSettings { ignoreBorders = true });

            return subChunkData;
        }

        public static bool CheckHasNonFullBlock(SubChunkData subChunkData)
        {
            bool hasNonFullBlock = false;
            ushort last = subChunkData.BlockArray[0,0,0];

            subChunkData.BlockArray.Loop((l) =>
            { 
                if (last != subChunkData.BlockArray[l.x,l.y,l.z])
                {
                    hasNonFullBlock = true;

                    return new LoopResult { callBreak = true };
                }

                return null;
            }, "CheckNonFullBlock", new () { ignoreBorders = true });

            return hasNonFullBlock;
        }

        public static Mesh GetMeshData(this BaseData subChunkData)
        {
            var mesh = new Mesh()
            {
                vertices = subChunkData.VerticesList.ToArray(),
                triangles = subChunkData.TrianglesList.ToArray(),
                uv = subChunkData.UvsList.ToArray(),
            };

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}