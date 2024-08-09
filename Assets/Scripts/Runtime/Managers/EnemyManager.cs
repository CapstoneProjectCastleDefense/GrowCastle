namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.Tags;
    using R3;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Managers.Base;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class EnemyManager : BaseElementManager<EnemyModel, EnemyPresenter, EnemyView>
    {
        private int    counterDeathEnemy;
        private int    targetCounterDeathEnemy;
        private Action onCounterComplete;
        private bool   isStartCounter;

        private readonly EnemyBlueprint          enemyBlueprint;
        public readonly  ReactiveProperty<float> CurrentBossHealth = new(0);
        public           float                   MaxBossHealth;
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

        public void StopCounterDeathEnemy()
        {
            this.counterDeathEnemy       = 0;
            this.targetCounterDeathEnemy = 0;
            this.onCounterComplete       = null;
            this.isStartCounter          = false;
        }

        public void UpdateEnemyDeathCounter()
        {
            if (!this.isStartCounter) return;
            this.counterDeathEnemy++;
            if (this.counterDeathEnemy >= this.targetCounterDeathEnemy)
            {
                this.onCounterComplete?.Invoke();
            }
        }

        public EnemyPresenter SpawnEnemy(string enemyId)
        {
            var enemyRecord = this.enemyBlueprint[enemyId];
            {
                var enemyPresenter = this.CreateElement(new()
                {
                    Id              = enemyId,
                    AddressableName = enemyRecord.PrefabName,
                    BaseStats = new()
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
                Debug.Log("Spawn enemy");
                return enemyPresenter;
            }
        }

        public EnemyPresenter SpawnBossEnemy(string bossId)
        {
            var boss = this.SpawnEnemy(bossId);
            boss.onUpdateHpStat = (value) =>
            {
                this.CurrentBossHealth.Value = value;
            };
            this.MaxBossHealth           = boss.Model.GetStat<float>(StatEnum.MaxHealth);
            this.CurrentBossHealth.Value = boss.Model.GetStat<float>(StatEnum.Health);
            return boss;
        }

        public List<EnemyPresenter> GetAllBossEnemies()
        {
            return this.entities.Where(e => e.Tags.Contains(ElementTag.Boss)).ToList();
        }
    }
}