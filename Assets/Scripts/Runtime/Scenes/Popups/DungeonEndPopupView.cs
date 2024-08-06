using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using Models.LocalData.LocalDataController;
using Runtime.StateMachines.GameStateMachine;
using Runtime.StateMachines.GameStateMachine.States;
using UnityEngine;
using Zenject;

namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Scenes.Adapters.Chest;
    using UnityEngine.UI;

    public class DungeonEndPopupView : BaseView
    {
        public GameObject             winGameObject;
        public GameObject             loseGameObject;
        public Transform              startPos;
        public Transform              endPos;
        public Button                 continueBtn;
        public GameObject             rewardField;
        public ChestRewardItemAdapter chestRewardItemAdapter;
    }

    [PopupInfo(nameof(DungeonEndPopupView), isOverlay: true)]
    public class DungeonEndPopupPresenter : BasePopupPresenter<DungeonEndPopupView>
    {
        private readonly GameStateMachine           gameStateMachine;
        private readonly DungeonLocalDataController dungeonLocalDataController;
        private readonly ChestLocalDataController   chestLocalDataController;
        private readonly DiContainer                diContainer;

        public DungeonEndPopupPresenter(
            SignalBus                  signalBus,
            GameStateMachine           gameStateMachine,
            DungeonLocalDataController dungeonLocalDataController,
            ChestLocalDataController   chestLocalDataController,
            DiContainer                diContainer
        )
            : base(signalBus)
        {
            this.gameStateMachine           = gameStateMachine;
            this.dungeonLocalDataController = dungeonLocalDataController;
            this.chestLocalDataController   = chestLocalDataController;
            this.diContainer                = diContainer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.continueBtn.onClick.AddListener(this.OnContinueBtnClick);
        }

        public override UniTask BindData()
        {
            var position = this.View.startPos.position;
            this.View.loseGameObject.transform.position = position;
            this.View.winGameObject.transform.position  = position;
            this.View.loseGameObject.SetActive(!this.dungeonLocalDataController.isWinCurrentDungeon);
            this.View.winGameObject.SetActive(this.dungeonLocalDataController.isWinCurrentDungeon);
            this.View.continueBtn.gameObject.SetActive(false);
            this.View.rewardField.gameObject.SetActive(false);
            var animObj = this.dungeonLocalDataController.isWinCurrentDungeon ? this.View.winGameObject : this.View.loseGameObject;
            this.DoAnimation(animObj);

            return UniTask.CompletedTask;
        }

        private void DoAnimation(GameObject gameObject)
        {
            DOTween.Kill(gameObject.transform);
            gameObject.transform.position                                                                  =  this.View.startPos.position;
            gameObject.transform.DOMove(this.View.endPos.position, 1f).SetEase(Ease.OutElastic).onComplete += this.ShowItemReward;
        }

        private async void ShowItemReward()
        {
            this.View.rewardField.gameObject.SetActive(true);
            var dungeonDataRecord = this.dungeonLocalDataController.GetDungeonRecord(this.dungeonLocalDataController.currentSelectedDungeon);
            var rewardData        = new List<ChestRewardItemModel>();
            dungeonDataRecord.DungeonRewardRecord.ForEach(reward =>
            {
                for (int i = 0; i < reward.Value; i++)
                {
                    this.chestLocalDataController.ReceiveChest(reward.RewardId.ToEnum<ResourceType>());
                }

                rewardData.Add(new()
                {
                    PoolItem = new()
                    {
                        ItemId = reward.RewardId, Value = reward.Value,
                    },
                });
            });
            await this.View.chestRewardItemAdapter.InitItemAdapter(rewardData, this.diContainer);
            this.View.continueBtn.gameObject.SetActive(true);
        }

        private void OnContinueBtnClick()
        {
            this.CloseView();
            this.gameStateMachine.TransitionTo<GamePrepareState>();
        }
    }
}