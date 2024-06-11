namespace Runtime.Systems
{
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Managers;

    public class GenerateGameLevelSystem : IGameSystem
    {
        private readonly MapLevelManager           mapLevelManager;
        private readonly LevelLocalDataController  levelLocalDataController;
        private readonly LevelBlueprint            levelBlueprint;
        private readonly CastleManager             castleManager;
        private readonly CastleLocalDataController castleLocalDataController;

        public GenerateGameLevelSystem(MapLevelManager mapLevelManager, LevelLocalDataController levelLocalDataController, LevelBlueprint levelBlueprint, CastleManager castleManager, CastleLocalDataController castleLocalDataController)
        {
            this.mapLevelManager           = mapLevelManager;
            this.levelLocalDataController  = levelLocalDataController;
            this.levelBlueprint            = levelBlueprint;
            this.castleManager             = castleManager;
            this.castleLocalDataController = castleLocalDataController;
        }

        public void GenerateCurrentLevelGame()
        {
            this.GenerateMapLevel();
            this.GenerateCastle();
        }

        private void GenerateMapLevel()
        {
            var currentLevelRecord = this.levelBlueprint.GetDataById(this.levelLocalDataController.CurrentLevel);
            this.mapLevelManager.CreateElement(new MapLevelModel() { LevelRecord = currentLevelRecord });
        }

        private void GenerateCastle()
        {
            CastleModel castleModel = new CastleModel() { CastleStat = this.castleLocalDataController.GetCasteStat(), AddressableName = "Castle" };
            this.castleManager.CreateElement(castleModel);
        }

        public void Initialize() { }
        public void Tick()       { }

        public void Dispose() { }
    }
}