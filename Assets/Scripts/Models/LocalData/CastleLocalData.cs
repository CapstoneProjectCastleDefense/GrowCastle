namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using R3;
    using Runtime.Enums;

    public class CastleLocalData : ILocalDataHaveController<CastleLocalDataController>
    {
        public int             Level;
        public readonly Dictionary<StatEnum, ReactiveProperty<float>> Stats = new();
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