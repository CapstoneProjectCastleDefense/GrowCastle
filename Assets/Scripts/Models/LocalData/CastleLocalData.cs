namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class CastleLocalData : ILocalDataHaveController<CastleLocalDataController>
    {
        public int             Level;
        public List<SlotData>  ListSlotData;
        public List<BlockData> ListBlockData;
        public void            Init() { }

        public class SlotData
        {
            public int    SlotId;
            public string ItemId;
            public bool   IsUnlock;
        }

        public class BlockData
        {
            public string BlockId;
            public int    BlockLevel;
            public bool   IsUnlock;
        }
    }
}