using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class MapGen
    {
        const float SMALL_NUMBER = 0.001f;
        const int LARGE_NUMBER = 100000;

        public static SeedValues GenerateSeeds(int globalSeed)
        {
            SeedValues seedValues = new SeedValues();
            System.Random random = new System.Random(globalSeed);

            seedValues.heightMapSeed = random.Next();
            seedValues.heatMapSeed = random.Next();

            return seedValues;
        }

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

                value *= 0.85f;

                value = Mathf.Clamp01((value + 1f) * 0.5f);

                value = settings.multiplier.Evaluate(value);

                return Mathf.Pow(value, settings.power);
            });


            return map;
        }

        public static float[,] GenerateHeatMap(int size, Vector2 offset, MapSettings settings)
        {
            return new float[0,0];
        }

        static (float x, float y) GetPosition(Vector2 offset, int xI, int yI, MapSettings settings, float frequency)
        {
            float x = ((float)((xI + offset.x) * SMALL_NUMBER) * settings.scale) * frequency;
            float y = ((float)((yI + offset.y) * SMALL_NUMBER) * settings.scale) * frequency;

            return (x, y);
        }
    }

    [System.Serializable]
    public struct MapSettings
    {
        [Min(0.001f)]
        public float scale;
        [Range(1,6)]
        public int octaves;

        [Range(0f,1f)]
        public float persistance;
        [Min(1f)]
        public float lacunarity;

        internal int seed;

        [Min(0.01f)]
        public float power;

        public AnimationCurve multiplier;
    }

    public struct SeedValues
    {
        public int heightMapSeed;
        public int heatMapSeed;
        // outras seeds
    }
}
