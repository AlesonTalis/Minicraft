using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.CE
{
    public static class SubChunkDataCE
    {
        public static SubChunkData Add(this SubChunkData data, CubeData cube, int x, int y, int z)
        {
            Vector3 offset = new (x, y, z);
            data = data.AddListFromArray(cube, offset) as SubChunkData;

            return data;
        }

        public static void FillColumn(this SubChunkData subChunk, int x, int z, int heightLimit, ushort blockTop, ushort blockBottom)
        {
            int height = subChunk.blockArray.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                if (i <= heightLimit)
                {
                    subChunk.blockArray[x, i, z] = blockBottom;
                }
                else
                {
                    subChunk.blockArray[x, i, z] = blockTop;
                }
            }
        }

        public static Vector3 GetPos(this SubChunkData subchunk) => new (subchunk.chunkPosX, subchunk.chunkPosY, subchunk.chunkPosZ);
    }
}
