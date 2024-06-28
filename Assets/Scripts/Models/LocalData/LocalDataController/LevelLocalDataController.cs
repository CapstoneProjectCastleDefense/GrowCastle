namespace Models.LocalData.LocalDataController
{
    using Models.Blueprints;

    public class LevelLocalDataController : ILocalDataController
    {
        private readonly LevelLocalData levelLocalData;
        private readonly LevelBlueprint levelBlueprint;
        private readonly WaveBlueprint  waveBlueprint;
        public LevelLocalDataController(LevelLocalData levelLocalData, LevelBlueprint levelBlueprint, WaveBlueprint waveBlueprint)
        {
            this.levelLocalData = levelLocalData;
            this.levelBlueprint = levelBlueprint;
            this.waveBlueprint  = waveBlueprint;
        }
        public int CurrentLevel => this.levelLocalData.CurrentLevel;

        public int CurrentWave => this.levelLocalData.CurrentWave;

        public LevelRecord GetCurrentLevelData() => this.levelBlueprint.GetDataById(this.CurrentLevel);

        public WaveRecord GetCurrentWaveData() => this.waveBlueprint.GetDataById(this.CurrentWave);

        public void PassCurrentWave()
        {
            
        }

        public void PassCurrentLevel()
        {
            
        }
        public void InitData()
        {
            
        }
    }
}