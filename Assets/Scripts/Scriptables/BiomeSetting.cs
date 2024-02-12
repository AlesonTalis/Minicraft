using UnityEngine;
using GD.MinMaxSlider;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Scriptables
{
    [System.Serializable]
    public class BiomeSetting
    {
        internal int index;

        public string? FileName { get; set; }

        [SerializeField]
        //        [HideInInspector]
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        //[SerializeField]
        //private string biomeName;
        //public string BiomeName
        //{
        //    get
        //    {
        //        return biomeName;
        //    }
        //    set
        //    {
        //        biomeName = value;
        //    }
        //}

        [SerializeField]
        private string biomeTitle;
        public string BiomeTitle
        {
            get
            {
                return biomeTitle;
            }
            set
            {
                biomeTitle = value;
            }
        }

        [SerializeField]
        private bool biomeIsOcean;
        public bool BiomeIsOcean
        {
            get
            {
                return biomeIsOcean;
            }
            set
            {
                biomeIsOcean = value;
            }
        }

        public float BiomeOceanHeight { get; set; }

        [SerializeField]
        private bool biomeRespectOcean;
        public bool BiomeRespectOcean
        {
            get
            {
                return biomeRespectOcean;
            }
            set
            {
                biomeRespectOcean = value;
            }
        }

        [SerializeField]
        private Color32 biomeColor;
        public Color32 BiomeColor
        {
            get
            {
                return biomeColor;
            }
            set
            {
                biomeColor = value;
            }
        }

        [SerializeField]
        [MinMaxSlider(0f, 1f)]
        private Vector2 biomeHeightRange;
        public V2 BiomeHeightRange
        {
            get
            {
                return new() {
                    x = biomeHeightRange.x,
                    y = biomeHeightRange.y
                };
            }
            set
            {
                biomeHeightRange = new Vector2()
                {
                    x = value.x,
                    y = value.y,
                };
            }
        }

        [SerializeField]
        [MinMaxSlider(0f, 1f)]
        private Vector2 biomeTemperatureRange;
        public V2 BiomeTemperatureRange
        {
            get
            {
                return new()
                {
                    x = biomeTemperatureRange.x,
                    y = biomeTemperatureRange.y
                };
            }
            set
            {
                biomeTemperatureRange = new Vector2()
                {
                    x = value.x,
                    y = value.y,
                };
            }
        }

        [SerializeField]
        [MinMaxSlider(0f, 1f)]
        private Vector2 biomeHumidityRange;
        public V2 BiomeHumidityRange
        {
            get
            {
                return new()
                {
                    x = biomeHumidityRange.x,
                    y = biomeHumidityRange.y
                };
            }
            set
            {
                biomeHumidityRange = new Vector2()
                {
                    x = value.x,
                    y = value.y,
                };
            }
        }

        /// <summary>
        /// <blockid,(range)>
        /// </summary>
        public Dictionary<ushort, int[]> SurfaceFiller { get; set; }// = new Dictionary<ushort, int[]>()// constructor only
        //{
        //    { 4, new [] {1 } },
        //    { 5, new [] {4,6 } }
        //};

        /// <summary>
        /// <blockid,(chance to fill with that block)>
        /// </summary>
        public Dictionary<ushort, int> UnderSurfaceFiller { get; set; }// = new Dictionary<ushort, int>()
        //{
        //    { 2, 100 },
        //};
        
        // TODO: boulders, ores, decorations (geodes,pool,fossil,etc...)


        public void ApplyId()
        {
            id = Guid.NewGuid().GetHashCode();
        }

        public void ApplyOceanHeight(float oceanHeight)
        {
            if (biomeRespectOcean is false) return;

            BiomeOceanHeight = oceanHeight;

            if (biomeIsOcean)
            {
                biomeHeightRange.y = Mathf.Clamp(biomeHeightRange.y, 0f, oceanHeight);
            }
            else
            {
                biomeHeightRange.x = Mathf.Clamp(biomeHeightRange.x, oceanHeight, 1f);
            }
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        public string GetFileName(string fileExtension = ".biome.json")
        {
            return FileName is not null
                ? FileName : biomeTitle.Replace(" ", "_") + fileExtension;
        }

    }

    public struct V2
    {
        public float x { get; set; }
        public float y { get; set; }
    }
}
