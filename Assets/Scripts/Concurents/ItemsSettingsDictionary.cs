
using Assets.Scripts.Gen;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Concurents
{
    internal static class ItemsSettingsDictionary
    {
        public static ConcurrentDictionary<string,ItemSettings> items = new ConcurrentDictionary<string,ItemSettings>();

        public static void FillItemsDictionary(this ItemSettings[] itemSettings)
        {
            itemSettings.ToList().FillItemsDictionary();
        }

        public static void FillItemsDictionary(this List<ItemSettings> itemSettings)
        {
            items.Clear();
            itemSettings.ForEach(f => items.TryAdd(f.itemId.ToString(), f));
        }
    }
}
