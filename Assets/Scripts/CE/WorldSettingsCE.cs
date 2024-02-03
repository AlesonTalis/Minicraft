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

            setings.heightMapSettings = GenerateMapSettings(rnd.Next());
            setings.heatMapSettings = GenerateMapSettings(rnd.Next());
            setings.humiditySettings = GenerateMapSettings(rnd.Next());
            setings.biomeSettings = biomeSettings;


            GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(setings);

            return setings;
        }

        static MapSettings GenerateMapSettings(int seed)
        {
            System.Random rnd = new (seed);

            return new Gen.MapSettings
            {
                seed = rnd.Next(),
                scale = (float)rnd.NextDouble() * 1000f,
                octaves = rnd.Next(3, 6),
                persistance = (float)(rnd.NextDouble() * 0.5f) + 0.25f,
                lacunarity = (float)(rnd.NextDouble() * 2.5f) + 1f,
                power = (float)(rnd.NextDouble() * 2.5f) + 1f,
                intensity = (float)(rnd.NextDouble() * 0.5f) + 0.5f,
            };
        }
    }
}
