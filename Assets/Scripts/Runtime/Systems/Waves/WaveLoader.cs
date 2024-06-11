namespace Runtime.Systems.Waves
{
    using System;
    using Codice.CM.Common;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Enums;

    public class WaveLoader
    {
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

        public async UniTask LoadWave(int waveId)
        {
            var waveRecord = this.waveBlueprint[waveId];
            foreach (var (enemyId, record) in waveRecord.WaveToEnemy)
            {
                this.SpawnEnemyGroup(enemyId, record.Quantity);
                await UniTask.Delay(TimeSpan.FromSeconds(record.Delay));
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
                    Stats = new()
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