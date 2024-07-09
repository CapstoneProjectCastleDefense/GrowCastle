namespace Models.LocalData.LocalDataController
{
    using Models.Blueprints;
    using R3;

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
        public int CurrentLevelValue => this.levelLocalData.CurrentLevel.Value;

        public ReactiveProperty<int> CurrentLevel => this.levelLocalData.CurrentLevel;

        public int CurrentWaveValue => this.levelLocalData.CurrentWave.Value;

        public ReactiveProperty<int> CurrentWave => this.levelLocalData.CurrentWave;

        public LevelRecord GetCurrentLevelData() => this.levelBlueprint.GetDataById(this.CurrentLevelValue);

        public WaveRecord GetCurrentWaveData() => this.waveBlueprint.GetDataById(this.CurrentWaveValue);

        public void PassCurrentWave()
        {
            
        }

        public void PassCurrentLevel()
        {
            this.levelLocalData.CurrentLevel.Value++;
            if (this.levelLocalData.CurrentLevel.Value > this.levelBlueprint.Count) this.levelLocalData.CurrentLevel.Value = 1;
        }
        public void InitData()
        {
            
        }
    }
}