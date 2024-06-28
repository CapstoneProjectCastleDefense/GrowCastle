namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class CastleLocalData : ILocalDataHaveController<CastleLocalDataController>
    {
        public int             Level;
        public List<BlockData> ListBlockData = new();
        public void            Init() { }

        
        public class BlockData
        {
            public string BlockId;
            public int    BlockLevel;
            public bool   IsUnlock;
        }
    }
}