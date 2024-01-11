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
    }
}
