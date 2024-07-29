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
    public class DungeonEndPopupView : BaseView
    {
        public GameObject winGameObject;
        public GameObject loseGameObject;
        public Transform  startPos;
        public Transform  endPos;
        
    }
    [PopupInfo(nameof(DungeonEndPopupView),isOverlay:true)]
    public class DungeonEndPopupPresenter : BasePopupPresenter<DungeonEndPopupView>
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly DungeonLocalDataController dungeonLocalDataController;

        public DungeonEndPopupPresenter(SignalBus signalBus, GameStateMachine gameStateMachine, DungeonLocalDataController dungeonLocalDataController) : base(signalBus)
        {
            this.gameStateMachine = gameStateMachine;
            this.dungeonLocalDataController = dungeonLocalDataController;
        }

        public override UniTask BindData()
        {
            var position = this.View.startPos.position;
            this.View.loseGameObject.transform.position = position;
            this.View.winGameObject.transform.position  = position;
            this.View.loseGameObject.SetActive(!this.dungeonLocalDataController.isWinCurrentDungeon);
            this.View.winGameObject.SetActive(this.dungeonLocalDataController.isWinCurrentDungeon);
            
            var animObj = this.dungeonLocalDataController.isWinCurrentDungeon ? this.View.winGameObject : this.View.loseGameObject;
            this.DoAnimation(animObj);
            
            return UniTask.CompletedTask;
        }
        
        private void DoAnimation(GameObject gameObject)
        {
            gameObject.transform.position = this.View.startPos.position;
            gameObject.transform.DOMove(this.View.endPos.position, 1f).SetEase(Ease.OutElastic).onComplete +=async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                gameObject.SetActive(false);
            };
        }
    }
}