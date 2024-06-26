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

        public GenerateGameLevelSystem(MapLevelManager mapLevelManager,ArcherManager archerManager, LevelLocalDataController levelLocalDataController, LevelBlueprint levelBlueprint, CastleManager castleManager, CastleLocalDataController castleLocalDataController,SlotManager slotManager)
        {
            this.mapLevelManager           = mapLevelManager;
            this.archerManager             = archerManager;
            this.levelLocalDataController  = levelLocalDataController;
            this.levelBlueprint            = levelBlueprint;
            this.castleManager             = castleManager;
            this.castleLocalDataController = castleLocalDataController;
            this.slotManager               = slotManager;
        }

        public void GenerateCurrentLevelGame()
        {
            this.GenerateMapLevel();
            this.GenerateCastle().ContinueWith(this.GenerateArcher);
            this.GenerateSlot();
        }

        private void GenerateMapLevel()
        {
            var currentLevelRecord = this.levelBlueprint.GetDataById(this.levelLocalDataController.CurrentLevel);
            this.mapLevelManager.CreateElement(new() { LevelRecord = currentLevelRecord });
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