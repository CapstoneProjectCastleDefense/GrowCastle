namespace Runtime.Systems.Waves
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Enums;
    using UnityEngine;
    using Zenject;

    public class WaveLoader : ITickable
    {
        private readonly List<WaveToEnemyRecord> inQueueWaves = new();
        private          float                   waveLoadCoolDown;

        private readonly WaveBlueprint  waveBlueprint;
        private readonly EnemyBlueprint enemyBlueprint;
        private readonly EnemyManager   enemyManager;

        public WaveLoader(
            WaveBlueprint waveBlueprint,
            EnemyBlueprint enemyBlueprint,
            EnemyManager enemyManager)
        {
            this.waveBlueprint  = waveBlueprint;
            this.enemyBlueprint = enemyBlueprint;
            this.enemyManager   = enemyManager;
        }

        public void Tick()
        {
            if (this.inQueueWaves.Count <= 0) return;
            if (this.waveLoadCoolDown > 0)
            {
                this.waveLoadCoolDown -= Time.deltaTime;
            }
            else
            {
                var record = this.inQueueWaves[0];
                this.waveLoadCoolDown = record.Delay;
                this.SpawnEnemyGroup(record.EnemyId, record.Quantity);
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
            var enemyRecord = this.enemyBlueprint[enemyId];
            for (var i = 0; i < quantity; i++)
            {
                var enemyController = this.enemyManager.CreateElement(new EnemyModel()
                {
                    Id              = enemyId,
                    AddressableName = enemyRecord.PrefabName,
                    Stats = new Dictionary<StatEnum, (Type, object)>
                    {
                        { StatEnum.Attack, (typeof(float), enemyRecord.Attack.baseValue) },
                        { StatEnum.Health, (typeof(float), enemyRecord.HP.baseValue) },
                        { StatEnum.MoveSpeed, (typeof(float), enemyRecord.Speed.baseValue) },
                    }
                });
                enemyController.UpdateView().Forget();
            }
        }
    }
}