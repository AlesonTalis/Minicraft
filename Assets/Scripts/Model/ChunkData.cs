using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class ChunkData : BaseData
    {
        public SubChunkData[] subChunks;

        public int chunkId { get; set; }

        public string chunkUID { get; set; }

        public float[,] heightMapData { get; set; }
        public float[,] heatMapData { get; set; }
        public float[,] humidityMapData { get; set; }
        public float[,] detailsMapData { get; set; }

        public int[,] biomeMapData { get; set; }
    }
}
