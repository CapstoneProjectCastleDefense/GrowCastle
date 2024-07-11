namespace Runtime.Elements.Entities.Castles
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using UnityEngine;
    using Zenject;

    public class CastlePresenter : BaseCombatantPresenter<CastleModel, CastleView, CastlePresenter>
    {
        private readonly CastleLocalDataController castleLocalDataController;
        private readonly IGameAssets               gameAssets;
        private readonly BlockBlueprint            blueprint;
        private readonly SignalBus                 signalBus;

        public CastlePresenter(
            CastleModel model,
            ObjectPoolManager objectPoolManager,
            CastleLocalDataController castleLocalDataController,
            IGameAssets gameAssets,
            BlockBlueprint blueprint,
            SignalBus signalBus)
            : base(model, objectPoolManager)
        {
            this.castleLocalDataController = castleLocalDataController;
            this.gameAssets                = gameAssets;
            this.blueprint                 = blueprint;
            this.signalBus                 = signalBus;
        }

        public             CastleView          CastleView   => this.View;
        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            await UniTask.WaitUntil(() => this.View != null);
            this.View.transform.position = new(-6.5f, -0.75f, 0);
            this.UpdateBlockBaseOnCurrentLevel();
        }

        public void UpdateBlockBaseOnCurrentLevel()
        {
            this.View.listBlockView.ForEach(blockView =>
            {
                var blockData       = this.castleLocalDataController.GetBlockDataById(blockView.blockId);
                var blockDataRecord = this.blueprint.GetDataById(blockData.BlockId);
                blockView.gameObject.SetActive(blockData.IsUnlock);
                blockView.blockImage.sprite = this.gameAssets.LoadAssetAsync<Sprite>(blockDataRecord.BlockToLevelRecords[blockData.BlockLevel].Image).WaitForCompletion();
            });
        }
        public override void Dispose() { }

        public void ResetHealth()
        {
            this.TargetThatAttackingMe = null;
            this.TargetThatImLookingAt = null;
            this.TargetThatImAttacking = null;
            
            var maxHp = this.Model.GetStat<float>(StatEnum.MaxHealth);
            this.Model.SetStat(StatEnum.Health, maxHp);
            this.signalBus.Fire(new UpdateCastleStatSignal() { CastleStats = this.Model });
        }
        public override void OnGetHit(float damage)
        {
            var hp = this.Model.GetStat<float>(StatEnum.Health);
            if (this.IsDead) return;
            hp -= damage;
            Debug.Log($"Castle get hit {damage} hp left {hp}");

            if (hp <= 0)
            {
                hp = 0;
                this.OnDeath();
            }

            this.Model.SetStat(StatEnum.Health, hp);
            this.signalBus.Fire(new UpdateCastleStatSignal() { CastleStats = this.Model });
        }
        public override void OnDeath()
        {
            Debug.Log("Lose");
            this.GetCurrentContainer().Resolve<GameStateMachine>().TransitionTo<GameEndWaveState>();
        }
    }

    public class CastleModel : ICombatant
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
    }
}