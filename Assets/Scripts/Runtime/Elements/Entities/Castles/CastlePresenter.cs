namespace Runtime.Elements.Entities.Castles
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Models.Tags;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Managers;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using TMPro;
    using UnityEngine;
    using Zenject;
    using static PlasticPipe.Server.MonitorStats;

    public class CastlePresenter : BaseCombatantPresenter<CastleModel, CastleView, CastlePresenter>
    {
        private readonly CastleLocalDataController castleLocalDataController;
        private readonly IGameAssets               gameAssets;
        private readonly BlockBlueprint            blueprint;
        private readonly SignalBus                 signalBus;
        private readonly EnemyManager              enemyManager;
        private readonly UserLocalDataController   userLocalDataController;

        public CastlePresenter(
            CastleModel model,
            ObjectPoolManager objectPoolManager,
            CastleLocalDataController castleLocalDataController,
            IGameAssets gameAssets,
            BlockBlueprint blueprint,
            SignalBus signalBus,
            EnemyManager enemyManager,
            UserLocalDataController userLocalDataController)
            : base(model, objectPoolManager)
        {
            this.castleLocalDataController = castleLocalDataController;
            this.gameAssets                = gameAssets;
            this.blueprint                 = blueprint;
            this.signalBus                 = signalBus;
            this.enemyManager              = enemyManager;
            this.userLocalDataController   = userLocalDataController;
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

        public void OnUpgrade()
        {
            this.UpdateBlockBaseOnCurrentLevel();
            this.CastleUpgradePopUp();
            this.Model.Stats = this.castleLocalDataController.GetCastleStat();
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

        public bool UseManaForSkill(float manaValue)
        {
            var currentMana = this.Model.GetStat<float>(StatEnum.Mana);
            if (currentMana >= manaValue)
            {
                currentMana -= manaValue;
                this.Model.SetStat(StatEnum.Mana, currentMana);
                this.castleLocalDataController.UpdateStats(this.Model.GetStat<float>(StatEnum.Health), currentMana);
                this.signalBus.Fire(new UpdateCastleStatSignal() { CastleStats = this.Model });
                return true;
            }

            return false;
        }

        public void ResetHealthAndMana()
        {
            this.TargetThatAttackingMe = null;
            this.TargetThatImLookingAt = null;
            this.TargetThatImAttacking = null;
            this.IsDead                = false;
            var maxHp = this.Model.GetStat<float>(StatEnum.MaxHealth);
            this.Model.SetStat(StatEnum.Health, maxHp);

            var maxMana = this.Model.GetStat<float>(StatEnum.MaxMana);
            this.Model.SetStat(StatEnum.Mana, maxMana);
            this.castleLocalDataController.UpdateStats(this.Model.GetStat<float>(StatEnum.MaxHealth), this.Model.GetStat<float>(StatEnum.MaxMana));

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
                this.Model.SetStat(StatEnum.Health, hp);
                this.OnDeath();
                return;
            }

            this.Model.SetStat(StatEnum.Health, hp);
            this.castleLocalDataController.UpdateStats(hp, this.Model.GetStat<float>(StatEnum.Mana));

            this.signalBus.Fire(new UpdateCastleStatSignal() { CastleStats = this.Model });
        }
        public override void OnDeath()
        {
            Debug.Log("Lose");
            this.userLocalDataController.IsWinCurrentLevel = false;
            this.GetCurrentContainer().Resolve<GameStateMachine>().TransitionTo<GameEndWaveState>();
        }

        public void CastleUpgradePopUp() { UpgradePopUp(this.View.castleUpPopUp, Vector2.zero); }

        public void ArcherUpgradePopUp()
        {
            Vector2 position = new Vector2(-2.6f, 0f);
            this.UpgradePopUp(this.View.archerUpPopUp, position);
        }

        internal void UpgradePopUp(RectTransform popUp, Vector2 startPosition)
        {
            popUp.DOKill();
            popUp.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            popUp.anchoredPosition                             = startPosition;
            popUp.gameObject.SetActive(true);
            popUp.DOAnchorPosY(2f, 0.3f).OnComplete(() => { popUp.gameObject.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(() => { popUp.gameObject.SetActive(false); }); });
        }

        public void UpdateStat()
        {
            this.Model.Stats = this.castleLocalDataController.GetCastleStat();
            this.signalBus.Fire(new UpdateCastleStatSignal() { CastleStats = this.Model });
        }
    }

    public class CastleModel : ICombatant
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
    }
}