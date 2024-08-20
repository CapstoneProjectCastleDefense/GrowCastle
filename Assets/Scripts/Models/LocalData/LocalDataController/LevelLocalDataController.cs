namespace Models.LocalData.LocalDataController
{
    using Models.Blueprints;
    using R3;
    using Runtime.Signals.Quests;
    using Runtime.StaticValues;
    using Zenject;

    public class LevelLocalDataController : ILocalDataController
    {
        private readonly LevelLocalData levelLocalData;
        private readonly LevelBlueprint levelBlueprint;
        private readonly WaveBlueprint  waveBlueprint;
        private readonly SignalBus      signalBus;

        public LevelLocalDataController(LevelLocalData levelLocalData, LevelBlueprint levelBlueprint, WaveBlueprint waveBlueprint, SignalBus signalBus)
        {
            this.levelLocalData = levelLocalData;
            this.levelBlueprint = levelBlueprint;
            this.waveBlueprint  = waveBlueprint;
            this.signalBus      = signalBus;
        }

        public int CurrentLevelValue => this.levelLocalData.CurrentLevel.Value % this.levelBlueprint.Count == 0?this.levelLocalData.CurrentLevel.Value : this.levelLocalData.CurrentLevel.Value % this.levelBlueprint.Count;

        public int EnemyStrange => this.levelLocalData.EnemyLevel;

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
            if (this.levelLocalData.CurrentLevel.Value > this.levelBlueprint.Count)
            {
                this.levelLocalData.EnemyLevel++;
            }
            this.signalBus.Fire(new QuestTriggerSignal() { TriggerSignalId = QuestTriggerSignalId.CompleteWave, Value = 1 });
        }

        public void InitData()
        {
        }
    }
}