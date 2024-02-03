using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Noise
{
    public static class HeightmapNoise
    {
        public const float SMALL_NUMBER = 0.0001f;

        public static float[,] GenerateHeightMap(float scale = 10000f)
        {
            int width = SubChunk.CHUNK_SIZE + 2, height = SubChunk.CHUNK_SIZE + 2;
            float[,] heightMap = new float[width, height];

            var noiseGen = new FastNoise();

            heightMap.Loop((l) =>
            {
                float x = (float)l.x * SMALL_NUMBER * scale;
                float y = (float)l.y * SMALL_NUMBER * scale;

                // (h + 1) * 0.5
                float noise = noiseGen.GetPerlin(x,y);

                var value = (noise + 1) * 0.5f;

                return value;
            }, "GenerateHeightMap");

            return heightMap;
        }
    }
}
