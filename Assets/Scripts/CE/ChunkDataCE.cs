using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using Assets.Scripts.Noise;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CE
{
    public static class ChunkDataCE
    {
        public static void SetPosition(this ChunkData chunkData, Vector2Int pos)
        {
            chunkData.PosX = pos.x;
            chunkData.PosZ = pos.y;
        }

        public static Vector3Int GetPosition(this ChunkData chunkData) => new Vector3Int(chunkData.PosX, chunkData.PosY, chunkData.PosZ);

        public static void GenerateChunkData(this ChunkData chunk, Vector2Int chunkPosition, WorldSettings settings, bool debug = false)
        {
            int size = SubChunk.CHUNK_SIZE;
            chunk.bufferData = new ChunkBufferData[4];

            float[,] heightMapData = MapGen.GenerateHeightMap(size, chunkPosition, settings.heightMapSettings, ref chunk);
            float[,] heatMapData = MapGen.GenerateHeatMap(size, chunkPosition, settings.heatMapSettings, ref chunk);
            float[,] humidityMapData = MapGen.GenerateHumidityMap(size, chunkPosition, settings.humiditySettings, ref chunk);
            int[,] biomeMapData = MapGen.GenerateBiomeMap(size, heightMapData, heatMapData, humidityMapData, settings.biomeSettings, ref chunk);

            chunk.SetMapData(heightMapData, heatMapData, humidityMapData, biomeMapData);

            chunk.chunkUID = Guid.NewGuid().ToString();
            chunk.SetPosition(chunkPosition);

            chunk.chunkSeed = ($"{chunkPosition.x}_{chunkPosition.y}_{settings.globalSeed}_{size}").GetHashCode();

            chunk.SetChunkDataHeightMap();

            chunk.CreateDecoration();

            if (debug) chunk.debugData = JsonConvert.SerializeObject(chunk);
            
            chunk.GetChunkData();
        }

        public static void SetBlock(this ChunkData chunk, int x, int y, int z, ushort blockId)
        {
            int subChunkId = y / SubChunk.CHUNK_SIZE;
            int subChunkY = y % SubChunk.CHUNK_SIZE;

            try
            {
                chunk.subChunks[subChunkId].BlockArray[x, subChunkY, z] = blockId;
            }
            catch
            {
                Debug.Log($"{x}|{y}|{z}|{subChunkId}|{subChunkY}");
            }
        }
    }
}
