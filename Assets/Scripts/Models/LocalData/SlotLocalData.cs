namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class SlotLocalData : ILocalDataHaveController<SlotLocalDataController>
    {
        public List<SlotData> SlotData = new();
        public void Init()
        {
            
        }
    }
    
    public class SlotData
    {
        public int      SlotId;
        public string   DeployObjectId;
        public bool     IsUnlock;
        public SlotType SlotType;
    }

    public enum SlotType
    {
        Hero,
        Tower,
        Leader,
        Archer
    }

}