using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class MapGen
    {
        const float SMALL_NUMBER = 0.001f;
        const int LARGE_NUMBER = 100000;

        public static float[,] GenerateHeightMap(int size, Vector2 offset, MapSettings settings)
        {
            float[,] map = new float[size,size];

            FastNoise noise = new FastNoise(settings.seed);

            Vector2[] octavesOffset = new Vector2[settings.octaves];
            System.Random rand = new System.Random(settings.seed);

            for (int i = 0; i < settings.octaves; i++)
            {
                int x = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);
                int y = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);

                octavesOffset[i] = new Vector2(x, y);
            }

            Debug.Log(string.Join(", ", octavesOffset));

            map.Loop((l) => {
                float value = 0;
                float amplitude = 1;
                float frequency = 1;

                for (int o = 0; o < octavesOffset.Length; o++)
                {
                    (float x, float y) = GetPosition(offset + octavesOffset[o], l.x, l.y, settings, frequency);

                    value += noise.GetSimplex(x, y) * amplitude;

                    frequency *= settings.lacunarity;
                    amplitude *= settings.persistance;
                }
                return Mathf.Pow(value, settings.power);
            });


            return map;
        }

        static (float x, float y) GetPosition(Vector2 offset, int xI, int yI, MapSettings settings, float frequency)
        {
            float x = ((float)((xI + offset.x) * SMALL_NUMBER) * settings.scale) * frequency;
            float y = ((float)((yI + offset.y) * SMALL_NUMBER) * settings.scale) * frequency;

            return (x, y);
        }
    }

    public struct MapSettings
    {
        public float scale;
        public int octaves;

        public float persistance;
        public float lacunarity;

        public int seed;

        public float power;
    }
}
