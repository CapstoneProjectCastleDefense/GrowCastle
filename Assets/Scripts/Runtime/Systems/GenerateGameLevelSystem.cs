namespace Runtime.Systems
{
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Managers;

    public class GenerateGameLevelSystem : IGameSystem
    {
        private readonly MapLevelManager           mapLevelManager;
        private readonly LevelLocalData            levelLocalData;
        private readonly LevelBlueprint            levelBlueprint;
        private readonly CastleManager             castleManager;
        private readonly CastleLocalDataController castleLocalDataController;
        public GenerateGameLevelSystem(MapLevelManager mapLevelManager, LevelLocalData levelLocalData, LevelBlueprint levelBlueprint, CastleManager castleManager,CastleLocalDataController castleLocalDataController)
        {
            this.mapLevelManager           = mapLevelManager;
            this.levelLocalData            = levelLocalData;
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
            var currentLevelRecord = this.levelBlueprint.GetDataById(this.levelLocalData.CurrentLevel);
            this.mapLevelManager.CreateElement(new MapLevelModel(){LevelRecord = currentLevelRecord});
        }

        private void GenerateCastle()
        {
            CastleModel castleModel = new CastleModel() {CastleStat = this.castleLocalDataController.GetCasteStat(), AddressableName = "Castle"};
            this.castleManager.CreateElement(castleModel);
        }
        public void Initialize()
        {

        }
        public void Tick()
        {

        }
    }
}