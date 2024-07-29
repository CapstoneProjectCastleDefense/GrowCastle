using System.Linq;

namespace Runtime.Systems
{
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Managers;

    public class GenerateGameLevelSystem : IGameSystem
    {
        private readonly MapLevelManager           mapLevelManager;
        private readonly ArcherManager             archerManager;
        private readonly LevelLocalDataController  levelLocalDataController;
        private readonly LevelBlueprint            levelBlueprint;
        private readonly CastleManager             castleManager;
        private readonly CastleLocalDataController castleLocalDataController;
        private readonly SlotManager               slotManager;
        private readonly DungeonModeBlueprint      dungeonModeBlueprint;

        private bool isGenerateComplete;

        public GenerateGameLevelSystem(MapLevelManager mapLevelManager,ArcherManager archerManager, LevelLocalDataController levelLocalDataController, LevelBlueprint levelBlueprint, CastleManager castleManager, CastleLocalDataController castleLocalDataController,SlotManager slotManager, DungeonModeBlueprint dungeonModeBlueprint)
        {
            this.mapLevelManager           = mapLevelManager;
            this.archerManager             = archerManager;
            this.levelLocalDataController  = levelLocalDataController;
            this.levelBlueprint            = levelBlueprint;
            this.castleManager             = castleManager;
            this.castleLocalDataController = castleLocalDataController;
            this.slotManager               = slotManager;
            this.dungeonModeBlueprint = dungeonModeBlueprint;
        }

        public void GenerateCurrentLevelGame()
        {
            this.GenerateMapLevel();
            if(this.isGenerateComplete) return;
            this.GenerateCastle().ContinueWith(this.GenerateArcher);
            this.GenerateSlot();
            this.isGenerateComplete = true;
        }

        public void GenerateDungeon(string dungeonId)
        {
            var currentDungeonRecord = this.dungeonModeBlueprint.GetDataById(dungeonId);
            this.mapLevelManager.CreateElement(new() { AddressableName = "BaseDungeonMap", EnvironmentId = currentDungeonRecord.EnvironmentId});
        }

        private void GenerateMapLevel()
        {
            var currentLevelRecord = this.levelBlueprint.GetDataById(this.levelLocalDataController.CurrentLevelValue);
            this.mapLevelManager.CreateElement(new() { AddressableName = currentLevelRecord.PrefabName, EnvironmentId = currentLevelRecord.LevelToWaveRecords.First().EnvironmentId});
        }

        private void GenerateMapDungeon()
        {
            
        }

        private UniTask GenerateCastle()
        {
            CastleModel castleModel = new CastleModel() { Stats = this.castleLocalDataController.GetCastleStat(), AddressableName = "Castle" };
            return this.castleManager.CreateElement(castleModel).UpdateView();
        }

        private void GenerateArcher()
        {
            this.archerManager.CreateAllUnlockedArcher();
        }

        private void GenerateSlot()
        {
            this.slotManager.CreateAllSlot();
        }

        public void Initialize() { }
        public void Tick()       { }

        public void Dispose() { }
    }
}