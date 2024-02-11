using Assets.Scripts.Scriptables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Concurents
{
    public static class BiomeSettingsDictionary
    {
        public static ConcurrentDictionary<int,BiomeSetting> biomes = new ConcurrentDictionary<int, BiomeSetting>();

        public static void FillDictionary(this BiomeSetting[] biomeSetting)
        {
            biomeSetting.ToList().FillDictionary();
        }

        public static void FillDictionary(this List<BiomeSetting> biomeSetting)
        {
            biomes.Clear();
            biomeSetting.ForEach(f => biomes.TryAdd(f.Id, f));
        }
    }
}
