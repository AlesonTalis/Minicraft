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
            chunk.SetChunkSettings(chunkPosition, settings);
            
            chunk.GenerateHeightMap();
            chunk.GenerateHeatMap();
            chunk.GenerateHumidityMap();
            chunk.GenerateBiomeMap();
            chunk.GenerateDetailsMap();
            
            chunk.SetChunkDataHeightMap();

            chunk.FillBiomeBlocks();

            if (debug) chunk.debugData = JsonConvert.SerializeObject(chunk);
            
            chunk.GetChunkData();
        }

        public static void SetBlock(this ChunkData chunk, int x, int y, int z, ushort blockId)
        {
            int subChunkId = y / SubChunk.CHUNK_SIZE;
            int subChunkY = y % SubChunk.CHUNK_SIZE + 1;// skip primeira linha?

            try
            {
                // TODO: observar a utilidade desta funcao
                if (subChunkY == SubChunk.CHUNK_SIZE && subChunkId < Chunk.CHUNK_SUBCHUNKS_STACK_HEIGHT)// borda superior
                    chunk.subChunks[subChunkId+1].BlockArray[x, 0, z] = blockId;
                if (subChunkY == 1 && subChunkId > 1)// borda inferior
                    chunk.subChunks[subChunkId-1].BlockArray[x, subChunkY + 1, z] = blockId;

                chunk.subChunks[subChunkId].BlockArray[x, subChunkY, z] = blockId;
            }
            catch
            {
                Debug.Log($"{x}|{y}|{z}|{subChunkId}|{subChunkY}");
            }
        }

        static void SetChunkSettings(this ChunkData chunk, Vector2Int offset, WorldSettings settings)
        {
            chunk.generateSize = SubChunk.CHUNK_SIZE + 2;
            chunk.finalSize = SubChunk.CHUNK_SIZE;
            chunk.worldOffset = offset;
            chunk.worldSettings = settings;

            chunk.chunkUID = Guid.NewGuid().ToString();
            chunk.SetPosition(offset);

            chunk.chunkSeed = ($"{offset.x}_{offset.y}_{settings.globalSeed}").GetHashCode();
        }

        static void GenerateHeightMap(this ChunkData chunk)
        {
            chunk.heightMapData = MapGen.GenerateHeightMap(chunk.generateSize, chunk.worldOffset, chunk.worldSettings.heightMapSettings);
        }

        static void GenerateHeatMap(this ChunkData chunk)
        {
            chunk.heatMapData = MapGen.GenerateHeatMap(chunk.generateSize, chunk.worldOffset, chunk.worldSettings.heatMapSettings);
        }

        static void GenerateHumidityMap(this ChunkData chunk)
        {
            chunk.humidityMapData = MapGen.GenerateHumidityMap(chunk.generateSize, chunk.worldOffset, chunk.worldSettings.humidityMapSettings);
        }

        static void GenerateBiomeMap(this ChunkData chunk)
        {
            chunk.biomeMapData = MapGen.GenerateBiomeMap(chunk.heightMapData, chunk.heatMapData, chunk.humidityMapData);
        }

        static void GenerateDetailsMap(this ChunkData chunk)
        {
            chunk.detailsMapData = MapGen.GenerateDetailsMap(chunk.heightMapData, chunk.biomeMapData, chunk.worldOffset, chunk.worldSettings.detailsMapSettings);
        }
    }
}
