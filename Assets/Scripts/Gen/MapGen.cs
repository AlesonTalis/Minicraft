using Assets.Scripts.Model;
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
        const float HOLE_PREVENTER_MULTIPLIER = 0.75f;
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
        public static float[,] GenerateHeightMap(int size, Vector2 offset, MapSettings settings, ref ChunkData chunk)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value) =>
            {
                value *= value * HOLE_PREVENTER_MULTIPLIER;
                value += HOLE_PREVENTER_MIN_VALUE;

                return value;
            }, out var borders);

            for (var i = 0; i < 4; i++)
            {
                chunk.bufferData[i].heightMapData = borders[i];
            }

            return heightMap;
        }

        public static float[,] GenerateHeatMap(int size, Vector2 offset, MapSettings settings, ref ChunkData chunk)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value) =>
            {
                return value;
            }, out var borders);

            for (var i = 0; i < 4; i++)
            {
                chunk.bufferData[i].heatMapData = borders[i];
            }

            return heightMap;
        }
        public static float[,] GenerateHumidityMap(int size, Vector2 offset, MapSettings settings, ref ChunkData chunk)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value) =>
            {
                return value;
            }, out var borders);

            for (var i = 0; i < 4; i++)
            {
                chunk.bufferData[i].humidityMapData = borders[i];
            }

            return heightMap;
        }

        public static int[,] GenerateBiomeMap(int size, float[,] heightMap, float[,] heatMap, float[,] humidityMap, BiomeSetting[] biomes, ref ChunkData chunk)
        {
            size += 2;
            var width = size;
            var height = size;

            var biomeMap = new int[width, height];
            var buffer = chunk.bufferData;

            for (int i = 0; i < 4; i++)
            {
                buffer[i].biomeMapData = new int[size - 2];
            }

            biomeMap.Loop((l) =>
            {
                var value = -1;
                int x = l.x, y = l.y;

                float height = 0;
                float heat = 0;
                float humidity = 0;
                BiomeSetting biome;

                if (x == 0 || x == size - 1)
                {
                    if (y > 0 && y < size - 1)
                    {
                        height = buffer[x == 0 ? 0 : 1].heightMapData[y];
                        heat = buffer[x == 0 ? 0 : 1].heatMapData[y];
                        humidity = buffer[x == 0 ? 0 : 1].humidityMapData[y];

                        biome = biomes.GetBiomeInRange(height, heat, humidity);

                        buffer[x == 0 ? 0 : 1].biomeMapData[y-1] = biome.index;
                    }
                }
                else if (y == 0 || y == size - 1)
                {
                    if (x > 0 && x < size - 1)
                    {
                        height = buffer[x == 0 ? 2 : 3].heightMapData[x];
                        heat = buffer[x == 0 ? 2 : 3].heatMapData[x];
                        humidity = buffer[x == 0 ? 2 : 3].humidityMapData[x];

                        biome = biomes.GetBiomeInRange(height, heat, humidity);

                        buffer[x == 0 ? 2 : 3].biomeMapData[x-1] = biome.index;
                    }
                }
                else
                {
                    height = heightMap[x - 1, y - 1];
                    heat = heatMap[x - 1, y - 1];
                    humidity = humidityMap[x - 1, y - 1];

                    biome = biomes.GetBiomeInRange(height, heat, humidity);

                    value = biome.index;
                }

                return value;
            });

            chunk.bufferData = buffer;

            return biomeMap;
        }

        static float[,] Generate2dFloat(int size, Vector2 offset, MapSettings settings, Func<float, float> actionPerPoint, out float[][] borders)
        {
            size += 2;
            float[,] map = new float[size, size];
            float[,] mapFinal = new float[size - 2, size - 2];

            FastNoise noise = new FastNoise(settings.seed);

            Vector2[] octavesOffset = new Vector2[settings.octaves];
            System.Random rand = new System.Random(settings.seed);

            for (int i = 0; i < settings.octaves; i++)
            {
                int x = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);
                int y = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);

                octavesOffset[i] = new Vector2(x, y);
            }

            float[][] bord = new float[][]
            {
                new float[size],
                new float[size],
                new float[size],
                new float[size],
            };

            map.Loop((l) => {
                float value = 0;
                float amplitude = 1;
                float frequency = 1;

                int x = l.x;
                int y = l.y;

                for (int o = 0; o < octavesOffset.Length; o++)
                {
                    (float fx, float fy) = GetPosition(offset + octavesOffset[o], l.x, l.y, settings, frequency);

                    value += noise.GetSimplex(fx, fy) * amplitude;

                    frequency *= settings.lacunarity;
                    amplitude *= settings.persistance;
                }

                value *= settings.intensity;

                value = actionPerPoint(value);

                value = Mathf.Clamp01((value + 1f) * 0.5f);

                value = Mathf.Pow(value, settings.power);


                if (x == 0 || x == size - 1)
                {
                    if (y > 0 && y < size - 1)
                        bord[x == 0 ? 0 : 1][y-1] = value;
                }
                else if (y == 0 || y == size - 1)
                {
                    if (x > 0 && x < size - 1)
                        bord[y == 0 ? 2 : 3][x-1] = value;
                }
                else
                {
                    mapFinal[x - 1, y - 1] = value;
                }

                return null;
            });

            borders = bord;

            return mapFinal;
        }

        [Obsolete]
        public static float[,] GenerateHeightMap(int size, Vector2 offset, MapSettings settings)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value) =>
            {
                value *= value * HOLE_PREVENTER_MULTIPLIER;
                value += HOLE_PREVENTER_MIN_VALUE;

                return value;
            });

            return heightMap;
        }

        [Obsolete]
        public static float[,] GenerateHeatMap(int size, Vector2 offset, MapSettings settings)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value) =>
            {
                return value;
            });

            return heightMap;
        }

        [Obsolete]
        public static float[,] GenerateHumidityMap(int size, Vector2 offset, MapSettings settings)
        {
            var heightMap = Generate2dFloat(size, offset, settings, (value) =>
            {
                return value;
            });

            return heightMap;
        }

        [Obsolete]
        public static int[,] GenerateBiomeMap(float[,] heightMap, float[,] heatMap, float[,] humidityMap, BiomeSetting[] biomes)
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

                var biome = biomes.GetBiomeInRange(height, heat, humidity);

                // index para debug
                value = biome.index;// biome.Id;

                return value;
            });

            return biomeMap;
        }

        static float[,] Generate2dFloat(int size, Vector2 offset, MapSettings settings, Func<float, float> actionPerPoint)
        {
            float[,] map = new float[size, size];

            FastNoise noise = new FastNoise(settings.seed);

            Vector2[] octavesOffset = new Vector2[settings.octaves];
            System.Random rand = new System.Random(settings.seed);

            for (int i = 0; i < settings.octaves; i++)
            {
                int x = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);
                int y = rand.Next(-LARGE_NUMBER, LARGE_NUMBER);

                octavesOffset[i] = new Vector2(x, y);
            }

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

                value *= settings.intensity;

                value = actionPerPoint(value);

                value = Mathf.Clamp01((value + 1f) * 0.5f);

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
