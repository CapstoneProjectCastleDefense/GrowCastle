using System.Collections.Generic;
using Models.LocalData.LocalDataController;

namespace Models.LocalData
{
    public class DungeonLocalData : ILocalDataHaveController<DungeonLocalDataController>
    {
        public Dictionary<string, DungeonData> dungeonData = new();
        public void Init()
        {
            
            
        }
    }

    public class DungeonData
    {
        public string Id { get; set; }
        public int NumberCompleted { get; set; }
        public bool IsUnlock { get; set; }
    }
}