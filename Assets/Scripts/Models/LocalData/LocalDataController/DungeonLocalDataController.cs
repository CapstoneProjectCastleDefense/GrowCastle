using System.Linq;
using GameFoundation.Scripts.Utilities.Extension;
using Models.Blueprints;

namespace Models.LocalData.LocalDataController
{
    using Extensions;

    public class DungeonLocalDataController : ILocalDataController
    {
        private readonly DungeonModeBlueprint     dungeonModeBlueprint;
        private readonly DungeonLocalData         dungeonLocalData;
        private readonly LevelLocalDataController levelLocalDataController;

        public string currentSelectedDungeon;
        public bool   isWinCurrentDungeon;


        public DungeonLocalDataController(DungeonModeBlueprint dungeonModeBlueprint, DungeonLocalData dungeonLocalData, LevelLocalDataController levelLocalDataController)
        {
            this.dungeonModeBlueprint     = dungeonModeBlueprint;
            this.dungeonLocalData         = dungeonLocalData;
            this.levelLocalDataController = levelLocalDataController;
        }
        public void InitData()
        {
            if (this.dungeonLocalData.dungeonData.Count == 0)
            {
                this.dungeonModeBlueprint.ForEach(dungeon => { this.dungeonLocalData.dungeonData.Add(dungeon.Key, new DungeonData() { Id = dungeon.Key, IsUnlock = false, NumberCompleted = 0 }); });
            }

            this.dungeonLocalData.dungeonData.First().Value.IsUnlock = true;
        }

        public bool CheckDungeonIsUnlock(string dungeonId) { return this.dungeonLocalData.dungeonData[dungeonId].IsUnlock; }

        public DungeonData GetDungeonData(string dungeonId) { return this.dungeonLocalData.dungeonData[dungeonId]; }
        
        public DungeonModeRecord GetDungeonRecord(string dungeonId) { return this.dungeonModeBlueprint.GetDataById(dungeonId); }
        public void CheckStatusOfAllDungeon()
        {
            this.dungeonLocalData.dungeonData.ForEach(dungeon =>
            {
                if (dungeon.Value.IsUnlock || this.levelLocalDataController.CurrentLevel.Value >=
                    this.dungeonModeBlueprint.GetDataById(dungeon.Key).RequireLevel)
                {
                    dungeon.Value.IsUnlock = true;
                }
                else
                {
                    dungeon.Value.IsUnlock = false;
                }
            });
        }

        public void CompleteCurrentDungeon()
        {
            if(this.currentSelectedDungeon.IsNullOrEmpty()) return;
            this.isWinCurrentDungeon = true;
            this.dungeonLocalData.dungeonData[this.currentSelectedDungeon].NumberCompleted++;
        }
    }
}