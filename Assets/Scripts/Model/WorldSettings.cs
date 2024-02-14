using Assets.Scripts.Gen;
using Assets.Scripts.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class WorldSettings
    {
        public string globalSeed { get; set; }

        public MapSettings heightMapSettings;
        public MapSettings heatMapSettings;
        public MapSettings humidityMapSettings;
        public MapSettings detailsMapSettings;
        public BiomeSetting[] biomeSettings;
        public ItemSettings[] itemSettings;
    }
}
