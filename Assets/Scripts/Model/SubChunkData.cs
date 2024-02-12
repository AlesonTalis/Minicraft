using Assets.Scripts.Model;

namespace Assets.Scripts.Model
{
    public class SubChunkData : BaseData
    {
        public int SubchunkId;
        
        public ushort[,,] BlockArray { get; set; }

        public ushort[][,] BlockArrayBuffer { get; set; }
    }
}