
using Assets.Scripts.Gen;
using System.Collections.Concurrent;

namespace Assets.Scripts.Concurents
{
    internal static class ItemsSettingsDictionary
    {
        public static ConcurrentDictionary<string,ItemSettings> items = new ConcurrentDictionary<string,ItemSettings>();
    }
}
