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

        private readonly EnemyGroupLoaderService    enemyGroupLoaderService;
        private readonly LevelBlueprint             levelBlueprint;
        private readonly SignalBus                  signalBus;
        private readonly EnemyManager               enemyManager;
        private readonly WaveBlueprint              waveBlueprint;
        private readonly LevelLocalDataController   levelLocalDataController;
        private readonly UserLocalDataController    userLocalDataController;
        private readonly DungeonModeBlueprint       dungeonModeBlueprint;
        private readonly DungeonLocalDataController dungeonLocalDataController;

        public WaveSystem(
            EnemyGroupLoaderService enemyGroupLoaderService,
            LevelBlueprint levelBlueprint,
            SignalBus signalBus,
            EnemyManager enemyManager,
            WaveBlueprint waveBlueprint,
            LevelLocalDataController levelLocalDataController,
            UserLocalDataController userLocalDataController,
            DungeonModeBlueprint dungeonModeBlueprint,
            DungeonLocalDataController dungeonLocalDataController)
        {
            this.enemyGroupLoaderService    = enemyGroupLoaderService;
            this.levelBlueprint             = levelBlueprint;
            this.signalBus                  = signalBus;
            this.enemyManager               = enemyManager;
            this.waveBlueprint              = waveBlueprint;
            this.levelLocalDataController   = levelLocalDataController;
            this.userLocalDataController    = userLocalDataController;
            this.dungeonModeBlueprint       = dungeonModeBlueprint;
            this.dungeonLocalDataController = dungeonLocalDataController;
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

                this.enemyGroupLoaderService.LoadEnemyFromWave(record.waveId);
                this.waveLoadCoolDown = record.delayTime;

                this.waveWithDelayTimeQueue.Remove(record);
                return;
            }

            this.waveLoadCoolDown -= Time.deltaTime;
        }

        #region Dungeon

        public void StartDungeonWave(string dungeonId)
        {
            this.InitDungeonWaveQueue(dungeonId);
            this.isActiveWave = true;
        }

        private void InitDungeonWaveQueue(string dungeonId)
        {
            this.waveWithDelayTimeQueue.Clear();
            var waveRecord = this.dungeonModeBlueprint[dungeonId].DungeonWaveRecord;
            foreach (var record in waveRecord)
            {
                this.waveWithDelayTimeQueue.Add((record.WaveId, record.Delay));
            }
        }

        public void ClearDungeon()
        {
            this.waveWithDelayTimeQueue.Clear();
            this.isActiveWave = false;
            this.enemyGroupLoaderService.UnloadEnemyFromWave();
        }

        private void EndCurrentDungeon(bool isComplete)
        {
            this.waveWithDelayTimeQueue.Clear();
            this.isActiveWave                                   = false;
            this.dungeonLocalDataController.isWinCurrentDungeon = isComplete;
            this.GetCurrentContainer().Resolve<GameStateMachine>().TransitionTo<GameDungeonModeEndState>();
        }

        #endregion

        public void StartCurrentWave(int level)
        {
            this.InitWaveQueue(level);
            this.isActiveWave = true;
            this.enemyManager.StartCounterDeathEnemy(this.CountEnemyInWave(level), this.CompleteCurrentWave);
        }


        private int CountEnemyInWave(int level)
        {
            int totalEnemy = 0;
            this.levelBlueprint[level].LevelToWaveRecords.ForEach(e => { this.waveBlueprint[e.WaveId].WaveToEnemy.ForEach(enemy => { totalEnemy += enemy.Value.Quantity; }); });
            return totalEnemy;
        }

        private void InitWaveQueue(int level)
        {
            this.waveWithDelayTimeQueue.Clear();
            var waveRecord = this.levelBlueprint[level].LevelToWaveRecords;
            foreach (var record in waveRecord)
            {
                this.waveWithDelayTimeQueue.Add((record.WaveId, record.Delay));
            }
        }


        public void ClearWave()
        {
            this.waveWithDelayTimeQueue.Clear();
            this.isActiveWave = false;
            this.enemyGroupLoaderService.UnloadEnemyFromWave();
        }


        private void CompleteCurrentWave()
        {
            this.waveWithDelayTimeQueue.Clear();
            this.isActiveWave                              = false;
            this.userLocalDataController.IsWinCurrentLevel = true;
            this.levelLocalDataController.PassCurrentLevel();
            this.GetCurrentContainer().Resolve<GameStateMachine>().TransitionTo<GameEndWaveState>();
        }


        public void Dispose() { this.signalBus.Unsubscribe<TimeCooldownSignal>(this.OnTimeCooldown); }
    }
}