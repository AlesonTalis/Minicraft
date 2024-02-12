using Assets.Scripts.Model;

namespace Assets.Scripts.Model
{
    public class SubChunkData : BaseData
    {
        public int SubchunkId;
        
        public ushort[,,] BlockArray { get; set; }

        /// <summary>
        /// 0=+Y,1=-Y,2=+X,3=-X,4=+Z,5=-Z
        /// </summary>
        public ushort[][,] BlockArrayBuffer { get; set; }
    }
}