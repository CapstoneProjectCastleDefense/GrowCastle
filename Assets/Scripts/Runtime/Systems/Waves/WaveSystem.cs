namespace Runtime.Systems.Waves
{
    using System.Collections.Generic;
    using Models.Blueprints;
    using Runtime.Signals;
    using Zenject;
    using Time = UnityEngine.Time;

    public class WaveSystem : IGameSystem
    {
        private          bool                                isActiveWave;
        private          float                               waveLoadCoolDown;
        private readonly List<(int waveId, float delayTime)> waveWithDelayTimeQueue = new();

        private readonly WaveLoader     waveLoader;
        private readonly LevelBlueprint levelBlueprint;
        private readonly SignalBus      signalBus;

        public WaveSystem(WaveLoader waveLoader,
                          LevelBlueprint levelBlueprint,
                          SignalBus signalBus)
        {
            this.waveLoader     = waveLoader;
            this.levelBlueprint = levelBlueprint;
            this.signalBus      = signalBus;
        }

        public void Initialize() { this.signalBus.Subscribe<TimeCooldownSignal>(this.OnTimeCooldown); }

        public void Tick() { }

        private void OnTimeCooldown(TimeCooldownSignal signal)
        {
            if(!this.isActiveWave) return;
            if (this.waveLoadCoolDown <= 0 &&
                this.waveWithDelayTimeQueue.Count > 0)
            {
                var record = this.waveWithDelayTimeQueue[0];

                this.waveLoader.LoadWave(record.waveId);
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

        public void EndCurrentWave()
        {
            this.waveWithDelayTimeQueue.Clear();
            this.isActiveWave = false;
        }

        public void Dispose()
        {
            this.signalBus.Unsubscribe<TimeCooldownSignal>(this.OnTimeCooldown);
        }
    }
}