using Assets.Scripts.Model;

namespace Assets.Scripts.Model
{
    public class SubChunkData : CubeData 
    {
        public int subchunkId;
        
        public int chunkPosX, chunkPosY, chunkPosZ;

        public ushort[,,] blockArray;
    }
}