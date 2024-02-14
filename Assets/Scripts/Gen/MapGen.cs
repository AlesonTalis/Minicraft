using Assets.Scripts.Concurents;
using Assets.Scripts.Scriptables;
using Assets.Scripts.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class MapGen
    {
        const float SMALL_NUMBER = 0.001f;
        const int LARGE_NUMBER = 100000;
        const float HOLE_PREVENTER_MULTIPLIER = 0.85f;
        const float HOLE_PREVENTER_MIN_VALUE = 0.15f;

        public static SeedValues GenerateSeeds(int globalSeed)
        {
            SeedValues seedValues = new SeedValues();
            System.Random random = new System.Random(globalSeed);

            seedValues.heightMapSeed = random.Next();
            seedValues.heatMapSeed = random.Next();
            seedValues.humiditySeed = random.Next();

            return seedValues;
        }

        public static float[,] GenerateHeightMap(int size, Vector2 offset, MapSettings settings)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value, _) =>
            {
                return value;
            });

            return heightMap;
        }

        public static float[,] GenerateHeatMap(int size, Vector2 offset, MapSettings settings)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value, _) =>
            {
                return value;
            });

            return heightMap;
        }
        
        public static float[,] GenerateHumidityMap(int size, Vector2 offset, MapSettings settings)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value, _) =>
            {
                return value;
            });

            return heightMap;
        }

        public static float[,] GenerateDetailsMap(float[,] heightMap, int[,] biomeMap, Vector2Int offset, MapSettings settings)
        {
            var size = heightMap.GetLength(0);
            BiomeSetting biome = null;

            var detailsMap = Generate2dFloat(size, offset, settings, (value, loop) =>
            {
                int x = loop.x, y = loop.y;

                var heightValue = heightMap[x, y];
                var biomeId = biomeMap[x, y];

                if (biome is null || biome.index != biomeId)
                {
                    biome = ConcurrentBiomeSettings.biomesDict[biomeId];
                }

                value = heightValue * value * biome.Intensity;


                return value;
            });

            return detailsMap;
        }

        public static int[,] GenerateBiomeMap(float[,] heightMap, float[,] heatMap, float[,] humidityMap)
        {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);

            var biomeMap = new int[width, height];

            biomeMap.Loop((l) =>
            {
                var value = -1;
                int x = l.x, y = l.y;

                var height = heightMap[x, y];
                var heat = heatMap[x, y];
                var humidity = humidityMap[x, y];

                var biome = ConcurrentBiomeSettings.biomesList.GetBiomeInRange(height, heat, humidity);

                // index para debug
                value = biome.index;// biome.Id;

                return value;
            });

            return biomeMap;
        }

        static float[,] Generate2dFloat(int size, Vector2 offset, MapSettings settings, Func<float, Loop, float> actionPerPoint)
        {
            float[,] map = new float[size, size];

            FastNoise noise = new FastNoise(settings.seed);

            Vector2[] octavesOffset = new Vector2[settings.octaves];
            System.Random rand = new System.Random(settings.seed);

            for (int i = 0; i < settings.octaves; i++)
            {
                int x = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);
                int y = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);

                octavesOffset[i] = new Vector2(x, y) + offset;
            }

            map.Loop((l) => {
                float value = 0;
                float amplitude = 1;
                float frequency = 1;

                for (int o = 0; o < octavesOffset.Length; o++)
                {
                    (float x, float y) = GetPosition(octavesOffset[o], l.x, l.y, settings, frequency);

                    value += noise.GetSimplex(x, y) * amplitude;

                    frequency *= settings.lacunarity;
                    amplitude *= settings.persistance;
                }

                value *= settings.intensity;

                value *= value * HOLE_PREVENTER_MULTIPLIER;
                value += HOLE_PREVENTER_MIN_VALUE;

                //if (value > 2f || value < -2f)
                //{
                //    throw new Exception($"VALUE TOO SMALL|LARGE - {value} - {offset}");
                //}

                value = actionPerPoint(value, l);

                value = Mathf.Pow(value, settings.power);

                return value;
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

        [Range(0f,1f)]
        public float intensity;

        public AnimationCurve multiplier;

    }

    public struct SeedValues
    {
        public int heightMapSeed;
        public int heatMapSeed;
        public int humiditySeed;
        // outras seeds
    }
}
