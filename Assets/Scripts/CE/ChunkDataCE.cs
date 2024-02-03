using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using Assets.Scripts.Noise;
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
            float[,] heightMap = MapGen.GenerateHeightMap(SubChunk.CHUNK_SIZE + 2, chunkPosition, settings.heightMapSettings);
            
            chunk.chunkUID = Guid.NewGuid().ToString();
            chunk.SetPosition(chunkPosition);
            chunk.SetChunkDataHeightMap(heightMap);
            chunk.GetChunkData();
        }
    }
}
