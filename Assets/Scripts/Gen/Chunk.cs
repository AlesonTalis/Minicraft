using Assets.Scripts.CE;
using Assets.Scripts.Concurents;
using Assets.Scripts.Model;
using Assets.Scripts.Scriptables;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class Chunk
    {
        public const int CHUNK_SUBCHUNKS_STACK_HEIGHT = 8;
        public const ushort AIR_BLOCK = 1024 + 32768 + 1;


        public static SubChunkData[] SetFilledChunk(ushort blockId, bool striped = false)
        {
            SubChunkData[] chunk = new SubChunkData[CHUNK_SUBCHUNKS_STACK_HEIGHT];

            for (int i = 0; i < chunk.Length; i++)
            {
                chunk[i] = SubChunk.SetFilledSubChunk(blockId, striped);
            }

            return chunk;
        }

        public static void SetMapData(this ChunkData chunkData, float[,] heightMapData, float[,] heatMapData, float[,] humidityMapData, int[,] biomeMapData)
        {
            chunkData.heightMapData = heightMapData;
            chunkData.heatMapData = heatMapData;
            chunkData.humidityMapData = humidityMapData;

            chunkData.biomeMapData = biomeMapData;
        }

        public static void SetChunkDataHeightMap(this ChunkData chunkData, float[,] heightMap = default)
        {
            chunkData.subChunks = SetFilledChunk(1);
            BiomeSetting oldBiome = null;
            ushort biomeBlock = 2;

            chunkData.heightMapData.Loop((l) =>
            {
                int height = (int)Mathf.Floor((chunkData.heightMapData[l.x, l.y]) * (SubChunk.CHUNK_SIZE * CHUNK_SUBCHUNKS_STACK_HEIGHT));
                var biomeId = chunkData.biomeMapData[l.x, l.y];

                if (oldBiome is not null && oldBiome.index != biomeId && BiomeSettingsDictionary.biomes.TryGetValue(biomeId, out oldBiome) is not false)
                {
                    biomeBlock = oldBiome.UnderSurfaceFiller.ToList()[0].Key;
                }

                for (int i = 0; i < CHUNK_SUBCHUNKS_STACK_HEIGHT; i++)
                {
                    int chunkHieght = height - (i * SubChunk.CHUNK_SIZE);

                    chunkData.subChunks[i].FillColumn(l.x, l.y, chunkHieght, AIR_BLOCK, biomeBlock);// biome block rule
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
