namespace Runtime.Systems.Waves
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Time = UnityEngine.Time;

    public class WaveSystem : IGameSystem
    {
        private bool                                isActiveWave;
        private float                               waveLoadCoolDown;
        private List<(int waveId, float delayTime)> WaveWithDelayTimeQueue = new();

        private readonly WaveLoader     waveLoader;
        private readonly LevelBlueprint levelBlueprint;

        public WaveSystem(WaveLoader waveLoader,
                          LevelBlueprint levelBlueprint)
        {
            this.waveLoader     = waveLoader;
            this.levelBlueprint = levelBlueprint;
        }

        public void Initialize() { }

        public void Tick()
        {
            if (!this.isActiveWave) return;
            if (this.waveLoadCoolDown <= 0 &&
                this.WaveWithDelayTimeQueue.Count > 0)
            {
                var record = this.WaveWithDelayTimeQueue[0];

                this.waveLoader.LoadWave(record.waveId);
                this.waveLoadCoolDown = record.delayTime;

                this.WaveWithDelayTimeQueue.Remove(record);
                return;
            }

            this.waveLoadCoolDown -= Time.deltaTime;
        }

        public void StartCurrentWave(int level)
        {
            this.InitWaveQueue(level);
            this.isActiveWave = true;
            //start spawn enemy and count total enemy need  to complete wave
        }

        private void InitWaveQueue(int level)
        {
            this.WaveWithDelayTimeQueue.Clear();
            var waveRecord = this.levelBlueprint[level].LevelToWaveRecords;
            foreach (var (waveId, record) in waveRecord)
            {
                this.WaveWithDelayTimeQueue.Add((waveId, record.Delay));
            }
        }

        public void EndCurrentWave() { this.WaveWithDelayTimeQueue.Clear(); }
    }
}