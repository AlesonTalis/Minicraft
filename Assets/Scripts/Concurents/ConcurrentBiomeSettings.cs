using Assets.Scripts.Scriptables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Concurents
{
    public static class ConcurrentBiomeSettings
    {
        public static ConcurrentDictionary<int,BiomeSetting> biomesDict = new ConcurrentDictionary<int, BiomeSetting>();
        public static List<BiomeSetting> biomesList = new List<BiomeSetting>();

        public static void FillDictionary(this BiomeSetting[] biomeSetting)
        {
            biomeSetting.ToList().FillDictionary();
        }

        public static void FillDictionary(this List<BiomeSetting> biomeSetting)
        {
            biomesDict.Clear();
            biomesList.Clear();

            for (int i = 0; i < biomeSetting.Count; i++)
            {
                biomeSetting[i].index = i;
                biomesDict.TryAdd(i, biomeSetting[i]);
                biomesList.Add(biomeSetting[i]);
            }
        }
    }
}
