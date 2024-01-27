using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "BiomeSetting", menuName = "MapGen/Biome Settings")]
    public class BiomeSettingSC : ScriptableObject
    {
        public string biomeName;
        public int biomeId;
        public string biomeDescription;

        public Color biomeColor;

        [Header("Height Range")]
        [Range(0f,1f)]
        public float biomeMinHeight;
        [Range(0f,1f)]
        public float biomeMaxHeight;
        public bool isOcean = false;

        [Header("Temperature Range")]
        [Range(0f,1f)]
        public float biomeMinTemperature;
        [Range(0f,1f)]
        public float biomeMaxTemperature;

        [Header("Humidity Range")]
        [Range(0f,1f)]
        public float biomeMinHumidity;
        [Range(0f,1f)]
        public float biomeMaxHumidity;
    }

    public struct BiomeSetting
    {
        public string? fileName;

        public string biomeName { get; set; }
        public string biomeTitle { get; set; }
        public bool biomeIsOcean { get; set; }

        public Color32 biomeColor { get; set; }

        public Vector2 biomeHeightRange { get; set; }
        public Vector2 biomeTemperatureRange { get; set; }
        public Vector2 biomeHumidityRange { get; set; }
    }
}
