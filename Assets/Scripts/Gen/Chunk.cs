using Assets.Scripts.CE;
using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class Chunk
    {
        public const int CHUNK_SUBCHUNKS_STACK_HEIGHT = 4;


        public static ushort[][,,] SetFilledChunk(ushort blockId, bool striped = false)
        {
            ushort[][,,] chunk = new ushort[CHUNK_SUBCHUNKS_STACK_HEIGHT][,,];

            for (int i = 0; i < chunk.Length; i++)
            {
                chunk[i] = SubChunk.SetFilledSubChunk(blockId, striped);
            }

            return chunk;
        }

        public static ChunkData GetChunkData(ushort[][,,] chunk)
        {
            ChunkData chunkData = new ChunkData();

            for (int i = 0; i < chunk.Length; i++)
            {
                var subChunkData = SubChunk.GetSubChunkData(chunk[i]);

                chunkData.AddList(subChunkData, new Vector3(0, i * SubChunk.CHUNK_SIZE, 0));
            }

            return chunkData;
        }

        public static Mesh GetChunkMesh(ChunkData chunk)
        {
            Mesh mesh = new Mesh
            {
                vertices = chunk.VerticesList.ToArray(),
                triangles = chunk.TrianglesList.ToArray(),
            };

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
