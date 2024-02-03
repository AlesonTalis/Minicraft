using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using Assets.Scripts.Scriptables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CE
{
    public static class WorldSettingsCE
    {
        public static WorldSettings Init(string seed, BiomeSetting[] biomeSettings)
        {
            System.Random rnd = new (seed.GetHashCode());

            var setings = new WorldSettings
            {
                globalSeed = seed,
            };

            setings.heightMapSettings = GenerateMapSettings(rnd.Next(), 250f);
            setings.heatMapSettings = GenerateMapSettings(rnd.Next(), 300f);
            setings.humiditySettings = GenerateMapSettings(rnd.Next(), 200f);
            setings.biomeSettings = biomeSettings;


            GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(setings);

            return setings;
        }

        static MapSettings GenerateMapSettings(int seed, 
            float scale = 1000f,
            int octavesMin = 3, int octavesMax = 6,
            float persistance = 0.5f,
            float lacunarity = 4f,
            float power = 2.5f,
            float intensity = 0.5f)
        {
            System.Random rnd = new (seed);

            return new Gen.MapSettings
            {
                seed = rnd.Next(),
                scale = ((float)rnd.NextDouble() * scale) + (scale * 0.5f),
                octaves = rnd.Next(octavesMin, octavesMax),
                persistance = (float)(rnd.NextDouble() * persistance) + (persistance * 0.5f),
                lacunarity = (float)(rnd.NextDouble() * lacunarity) + 1f,
                power = (float)(rnd.NextDouble() * power) + 1f,
                intensity = (float)(rnd.NextDouble() * intensity) + (intensity * 0.5f),
            };
        }
    }
}
