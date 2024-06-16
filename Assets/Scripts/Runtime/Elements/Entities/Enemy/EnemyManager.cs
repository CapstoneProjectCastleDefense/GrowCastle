﻿namespace Runtime.Elements.Entities.Enemy
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using UnityEngine;

    public class EnemyManager : BaseElementManager<EnemyModel, EnemyPresenter, EnemyView>
    {
        private readonly EnemyBlueprint enemyBlueprint;
        public EnemyManager(BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>.Factory factory,
            EnemyBlueprint enemyBlueprint)
            : base(factory)
        {
            this.enemyBlueprint = enemyBlueprint;
        }
        public override void Initialize() { }
        public override void DisposeAllElement()
        {
            var cache = this.entities.ToArray();
            this.entities.Clear();
            foreach (var entity in cache)
            {
                entity.Dispose();
            }
        }

        public void SpawnEnemy(string enemyId)
        {
            var enemyRecord = this.enemyBlueprint[enemyId];
            {
                var enemyPresenter = this.CreateElement(new EnemyModel()
                {
                    Id              = enemyId,
                    AddressableName = enemyRecord.PrefabName,
                    Stats = new Dictionary<StatEnum, (Type, object)>
                    {
                        { StatEnum.Attack, (typeof(float), enemyRecord.Attack.baseValue) },
                        { StatEnum.Health, (typeof(float), enemyRecord.HP.baseValue) },
                        { StatEnum.MoveSpeed, (typeof(float), enemyRecord.Speed.baseValue) },
                    },
                    StartPos = new Vector3(0, -2, 0)
                });
                enemyPresenter.UpdateView().Forget();
            }
            
            Debug.Log("Spawn enemy");
        }
    }
}