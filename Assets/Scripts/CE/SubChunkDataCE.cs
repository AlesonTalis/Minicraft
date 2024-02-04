using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.CE
{
    public static class SubChunkDataCE
    {
        public static void Add(this SubChunkData data, BaseData cube, int x, int y, int z)
        {
            Vector3 offset = new (x, y, z);
            data.AddListFromArray(cube, offset);
        }

        public static void FillColumn(this SubChunkData subChunk, int x, int z, int heightLimit, ushort blockTop, ushort blockBottom)
        {
            int height = subChunk.BlockArray.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                if (i <= heightLimit)
                {
                    subChunk.BlockArray[x, i, z] = blockBottom;
                }
                else
                {
                    subChunk.BlockArray[x, i, z] = blockTop;
                }
            }
        }

        public static Vector3 GetPos(this SubChunkData subchunk) => new (subchunk.PosX, subchunk.PosY, subchunk.PosZ);
    }
}
