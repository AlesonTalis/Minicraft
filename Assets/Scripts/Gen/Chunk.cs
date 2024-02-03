using Assets.Scripts.CE;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class Chunk
    {
        public const int CHUNK_SUBCHUNKS_STACK_HEIGHT = 8;


        public static SubChunkData[] SetFilledChunk(ushort blockId, bool striped = false)
        {
            SubChunkData[] chunk = new SubChunkData[CHUNK_SUBCHUNKS_STACK_HEIGHT];

            for (int i = 0; i < chunk.Length; i++)
            {
                chunk[i] = SubChunk.SetFilledSubChunk(blockId, striped);
            }

            return chunk;
        }

        public static void SetChunkDataHeightMap(this ChunkData chunkData, float[,] heightMap)
        {
            chunkData.subChunks = SetFilledChunk(1,true);

            heightMap.Loop((l) =>
            {
                int height = (int)Mathf.Floor(heightMap[l.x, l.y] * (SubChunk.CHUNK_SIZE * CHUNK_SUBCHUNKS_STACK_HEIGHT));

                for (int i = 0; i < CHUNK_SUBCHUNKS_STACK_HEIGHT; i++)
                {
                    int chunkHieght = height - (i * SubChunk.CHUNK_SIZE);

                    chunkData.subChunks[i].FillColumn(l.x, l.y, chunkHieght, 129, 1);
                }

                return null;
            }, "ConvertHeightMapToChunkData");
        }

        public static ChunkData GetChunkData(SubChunkData[] chunk)
        {
            ChunkData chunkData = new ();

            for (int i = 0; i < chunk.Length; i++)
            {
                var subChunkData = SubChunk.GetSubChunkData(chunk[i]);

                chunkData.AddList(subChunkData, new Vector3(0, i * SubChunk.CHUNK_SIZE, 0));
            }

            return chunkData;
        }
        
        public static void GetChunkData(this ChunkData chunkData)
        {
            for (int i = 0; i < chunkData.subChunks.Length; i++)
            {
                var subChunkData = SubChunk.GetSubChunkData(chunkData.subChunks[i]);
                subChunkData.PosY = SubChunk.CHUNK_SIZE * i;

                chunkData.AddList(subChunkData, new Vector3(0, i * SubChunk.CHUNK_SIZE, 0));
            }
        }
        
        public static void GetChunkDataValues(this ChunkData chunkData)
        {
            for (int i = 0; i < chunkData.subChunks.Length; i++)
            {
                var subChunkData = SubChunk.GetSubChunkData(chunkData.subChunks[i]);
                subChunkData.PosY = SubChunk.CHUNK_SIZE * i;

                chunkData.AddList(subChunkData, new Vector3(0, i * SubChunk.CHUNK_SIZE, 0));
            }
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
