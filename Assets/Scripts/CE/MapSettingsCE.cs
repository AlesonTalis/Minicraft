using Assets.Scripts.Gen;

namespace Assets.Scripts.CE
{
    internal static class MapSettingsCE
    {
        public static MapSettings ApplySeed(this MapSettings settings, int seed)
        {
            settings.seed = seed;

            return settings;
        }
    }
}
