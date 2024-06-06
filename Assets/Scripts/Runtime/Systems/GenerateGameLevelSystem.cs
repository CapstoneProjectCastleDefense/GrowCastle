namespace Runtime.Systems
{
    using Models.Blueprints;
    using Models.LocalData;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Managers;

    public class GenerateGameLevelSystem : IGameSystem
    {
        private readonly MapLevelManager mapLevelManager;
        private readonly LevelLocalData  levelLocalData;
        private readonly LevelBlueprint  levelBlueprint;
        public GenerateGameLevelSystem(MapLevelManager mapLevelManager, LevelLocalData levelLocalData, LevelBlueprint levelBlueprint)
        {
            this.mapLevelManager = mapLevelManager;
            this.levelLocalData  = levelLocalData;
            this.levelBlueprint  = levelBlueprint;
        }

        public void GenerateCurrentLevelGame()
        {
            var currentLevelRecord = this.levelBlueprint.GetDataById(this.levelLocalData.CurrentLevel);
            this.mapLevelManager.CreateElement(new MapLevelModel(){LevelRecord = currentLevelRecord});
        }
        public void Initialize()
        {
            
        }
        public void Tick()
        {
            
        }
    }
}