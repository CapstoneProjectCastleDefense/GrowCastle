namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class EnemyManager : BaseElementManager<EnemyModel, EnemyPresenter, EnemyView>
    {
        private int    counterDeathEnemy;
        private int    targetCounterDeathEnemy;
        private Action onCounterComplete;
        private bool   isStartCounter;

        private readonly EnemyBlueprint enemyBlueprint;
        public EnemyManager(
            BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>.Factory factory,
            EnemyBlueprint enemyBlueprint
        )
            : base(factory)
        {
            this.enemyBlueprint = enemyBlueprint;
        }

        public override void Initialize() { }

        public void StartCounterDeathEnemy(int targetCounter, Action onCounterCompleteAction)
        {
            this.counterDeathEnemy       = 0;
            this.targetCounterDeathEnemy = targetCounter;
            this.onCounterComplete       = onCounterCompleteAction;
            this.isStartCounter          = true;
        }
        public void UpdateEnemyDeathCounter()
        {
            if (!this.isStartCounter) return;
            this.counterDeathEnemy++;
            if (this.counterDeathEnemy >= this.targetCounterDeathEnemy) this.onCounterComplete?.Invoke();
        }

        public void SpawnEnemy(string enemyId)
        {
            var enemyRecord = this.enemyBlueprint[enemyId];
            {
                var enemyPresenter = this.CreateElement(new()
                {
                    Id              = enemyId,
                    AddressableName = enemyRecord.PrefabName,
                    Stats = new()
                    {
                        { StatEnum.Attack, (typeof(float), enemyRecord.Attack.baseValue) },
                        { StatEnum.MaxAttack, (typeof(float), enemyRecord.Attack.baseValue) },
                        { StatEnum.Health, (typeof(float), enemyRecord.HP.baseValue) },
                        { StatEnum.MaxHealth, (typeof(float), enemyRecord.HP.baseValue) },
                        { StatEnum.MoveSpeed, (typeof(float), enemyRecord.Speed.baseValue) },
                        { StatEnum.MaxSpeed, (typeof(float), enemyRecord.Speed.baseValue) },
                        { StatEnum.AttackRange, (typeof(float), enemyRecord.AttackRange) },
                        { StatEnum.AttackSpeed, (typeof(float), 1f) },
                        { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Building) },
                        { StatEnum.Gold, (typeof(float), enemyRecord.Gold.baseValue) },
                        { StatEnum.Exp, (typeof(float), enemyRecord.Exp.baseValue) }
                    },
                    StartPos = new(Random.Range(15f, 20f), Random.Range(-2.5f, -1.5f), 0),
                });
                enemyPresenter.UpdateView().Forget();
                enemyPresenter.SetManager(this);
            }

            Debug.Log("Spawn enemy");
        }
    }
}