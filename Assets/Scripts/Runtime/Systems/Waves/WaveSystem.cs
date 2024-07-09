namespace Runtime.Systems.Waves
{
    using System.Collections.Generic;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using Zenject;
    using Time = UnityEngine.Time;

    public class WaveSystem : IGameSystem
    {
        private          bool                                isActiveWave;
        private          float                               waveLoadCoolDown;
        private readonly List<(int waveId, float delayTime)> waveWithDelayTimeQueue = new();

        private readonly EnemyGroupLoaderService  enemyGroupLoaderService;
        private readonly LevelBlueprint           levelBlueprint;
        private readonly SignalBus                signalBus;
        private readonly EnemyManager             enemyManager;
        private readonly WaveBlueprint            waveBlueprint;
        private readonly LevelLocalDataController levelLocalDataController;

        public WaveSystem(
            EnemyGroupLoaderService enemyGroupLoaderService,
            LevelBlueprint levelBlueprint,
            SignalBus signalBus, EnemyManager enemyManager,WaveBlueprint waveBlueprint, LevelLocalDataController levelLocalDataController)
        {
            this.enemyGroupLoaderService  = enemyGroupLoaderService;
            this.levelBlueprint           = levelBlueprint;
            this.signalBus                = signalBus;
            this.enemyManager             = enemyManager;
            this.waveBlueprint            = waveBlueprint;
            this.levelLocalDataController = levelLocalDataController;
        }

        public void Initialize() { this.signalBus.Subscribe<TimeCooldownSignal>(this.OnTimeCooldown); }

        public void Tick() { }

        private void OnTimeCooldown(TimeCooldownSignal signal)
        {
            if (!this.isActiveWave) return;
            if (this.waveLoadCoolDown <= 0 &&
                this.waveWithDelayTimeQueue.Count > 0)
            {
                var record = this.waveWithDelayTimeQueue[0];

                this.enemyGroupLoaderService.LoadWave(record.waveId);
                this.waveLoadCoolDown = record.delayTime;

                this.waveWithDelayTimeQueue.Remove(record);
                return;
            }

            this.waveLoadCoolDown -= Time.deltaTime;
        }

        public void StartCurrentWave(int level)
        {
            this.InitWaveQueue(level);
            this.isActiveWave = true;
            this.enemyManager.StartCounterDeathEnemy(this.CountEnemyInWave(level),this.CompleteCurrentWave);
        }

        private int CountEnemyInWave(int level)
        {
            int totalEnemy = 0;
            this.levelBlueprint[level].LevelToWaveRecords.ForEach(e =>
            {
                this.waveBlueprint[e.Value.WaveId].WaveToEnemy.ForEach(enemy =>
                {
                    totalEnemy+=enemy.Value.Quantity;
                });
            });
            return totalEnemy;
        }

        private void InitWaveQueue(int level)
        {
            this.waveWithDelayTimeQueue.Clear();
            var waveRecord = this.levelBlueprint[level].LevelToWaveRecords;
            foreach (var (waveId, record) in waveRecord)
            {
                this.waveWithDelayTimeQueue.Add((waveId, record.Delay));
            }
        }

        private void CompleteCurrentWave()
        {
            this.waveWithDelayTimeQueue.Clear();
            this.isActiveWave = false;
            this.levelLocalDataController.PassCurrentLevel();
            this.GetCurrentContainer().Resolve<GameStateMachine>().TransitionTo<GameEndWaveState>();
        }

        public void Dispose() { this.signalBus.Unsubscribe<TimeCooldownSignal>(this.OnTimeCooldown); }
    }
}