namespace Runtime.Elements.Entities.Castles
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class CastlePresenter : BaseElementPresenter<CastleModel, CastleView, CastlePresenter>, ICastlePresenter
    {
        private readonly CastleLocalDataController castleLocalDataController;
        private readonly IGameAssets               gameAssets;
        private readonly BlockBlueprint            blueprint;

        public CastlePresenter(CastleModel model, ObjectPoolManager objectPoolManager, CastleLocalDataController castleLocalDataController, IGameAssets gameAssets, BlockBlueprint blueprint)
            : base(model, objectPoolManager)
        {
            this.castleLocalDataController = castleLocalDataController;
            this.gameAssets                = gameAssets;
            this.blueprint                 = blueprint;
        }

        public             CastleView          CastleView   => this.View;
        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            await UniTask.WaitUntil(() => this.View != null);
            this.View.transform.position = new(-8.54f, -1.33f, 0);
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
        public void OnGetHit(float damage)
        {
            var hp = this.Model.GetStat<float>(StatEnum.Health);
            if(this.IsDead) return;
            hp -= damage;
            Debug.Log($"Castle get hit {damage} hp left {hp}");
            this.Model.SetStat(StatEnum.Health, hp);
            if (hp <= 0) this.OnDeath();
        }
        public void OnDeath()
        {
            Debug.Log("Lose");
        }
        public ITargetable TargetThatImAttacking { get; set; }
        public ITargetable TargetThatAttackingMe { get; set; }
        public bool        IsDead                { get; }
    }

    public class CastleModel : IElementModel, IHaveStats
    {
        public string Id              { get; set; }
        public string AddressableName { get; set; }

        // public CastleStat                           CastleStat { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats { get; set; }
    }

    public class CastleStat
    {
        public int Mp            { get; set; }
        public int Hp            { get; set; }
        public int GoldToUpgrade { get; set; }
    }
}