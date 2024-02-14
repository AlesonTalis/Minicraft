using Assets.Scripts.Concurents;
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
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CE
{
    public static class WorldSettingsCE
    {
        public static WorldSettings Init(string seed, BiomeSetting[] biomeSettings, ItemSettings[] itemSettings, float globalScale = 5f)
        {
            System.Random rnd = new (seed.GetHashCode());

            var setings = new WorldSettings
            {
                globalSeed = seed,
            };

            setings.heightMapSettings = GenerateMapSettings(rnd.Next(), globalScale * 250f, 3,5, 0.65f, 5f, 1.2f, 0.9f);
            setings.heatMapSettings = GenerateMapSettings(rnd.Next(), globalScale * 300f);
            setings.humidityMapSettings = GenerateMapSettings(rnd.Next(), globalScale * 200f);
            setings.detailsMapSettings = GenerateMapSettings(rnd.Next(), globalScale * 100f, 4,6, 0.85f, 2.5f);
            setings.biomeSettings = biomeSettings;
            setings.itemSettings = itemSettings;

            //GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(setings);// debug

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

            return new MapSettings
            {
                seed = rnd.Next(),
                scale = scale,//((float)rnd.NextDouble() * scale) + (scale * 0.333f),
                octaves = rnd.Next(octavesMin, octavesMax),
                persistance = (float)(rnd.NextDouble() * persistance) + (persistance * 0.333f),
                lacunarity = (float)(rnd.NextDouble() * lacunarity) + 1f,
                power = (float)(rnd.NextDouble() * power) + (power * 0.333f),
                intensity = (float)(rnd.NextDouble() * intensity) + (intensity * 0.333f),
            };
        }
    }
}
