namespace Runtime.Systems.Waves
{
    using System;
    using System.Collections.Generic;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Managers;
    using Runtime.Signals;
    using Zenject;

    public class WaveLoader : IInitializable, IDisposable
    {
        private readonly List<WaveToEnemyRecord> inQueueWaves = new();
        private          float                   waveLoadCoolDown;

        private readonly WaveBlueprint  waveBlueprint;
        private readonly EnemyManager   enemyManager;
        private readonly SignalBus      signalBus;

        public WaveLoader(
            WaveBlueprint waveBlueprint,
            EnemyManager enemyManager,
            SignalBus signalBus)
        {
            this.waveBlueprint  = waveBlueprint;
            this.enemyManager   = enemyManager;
            this.signalBus      = signalBus;
        }

        private void OnTimeCooldown(TimeCooldownSignal signal)
        {
            if (this.inQueueWaves.Count <= 0) return;
            if (this.waveLoadCoolDown > 0)
            {
                this.waveLoadCoolDown -= signal.DeltaTime;
            }
            else
            {
                var record = this.inQueueWaves[0];
                this.waveLoadCoolDown = record.Delay;
                this.SpawnEnemyGroup(record.EnemyId, record.Quantity);
                this.inQueueWaves.Remove(record);
            }
        }

        public void LoadWave(int waveId)
        {
            var waveRecord = this.waveBlueprint[waveId];
            foreach (var (_, record) in waveRecord.WaveToEnemy)
            {
                this.inQueueWaves.Add(record);
            }
        }

        private void SpawnEnemyGroup(string enemyId, int quantity)
        {
            for (var i = 0; i < quantity; i++)
            {
                this.enemyManager.SpawnEnemy(enemyId);
            }
        }

        public void Initialize() { this.signalBus.Subscribe<TimeCooldownSignal>(this.OnTimeCooldown); }

        public void Dispose() { this.signalBus.Unsubscribe<TimeCooldownSignal>(this.OnTimeCooldown); }
    }
}