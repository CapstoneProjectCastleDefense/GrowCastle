using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.MVP;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using Models.LocalData.LocalDataController;
using Runtime.StateMachines.GameStateMachine;
using Runtime.StateMachines.GameStateMachine.States;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Scenes.Popups
{
    public class DungeonLevelItem : TViewMono
    {
        public string dungeonId;
        public TextMeshProUGUI dungeonIdText;
        public Button dungeonSelectBtn;
        public Action<string> onDungeonSelectBtnClick;

        private void Awake()
        {
            this.dungeonSelectBtn.onClick.AddListener(()=>{this.onDungeonSelectBtnClick?.Invoke(this.dungeonId);});
        }
    }
    public class DungeonSelectLevelPopupView : BaseView
    {
        public Button exitButton;
        public List<DungeonLevelItem> dungeonLevelItems;

    }

    [PopupInfo(nameof(DungeonSelectLevelPopupView),isOverlay:true)]
    public class DungeonSelectLevelPopupPresenter : BasePopupPresenter<DungeonSelectLevelPopupView>
    {
        private readonly DungeonLocalDataController dungeonLocalDataController;
        private readonly GameStateMachine gameStateMachine;

        public DungeonSelectLevelPopupPresenter(SignalBus signalBus, DungeonLocalDataController dungeonLocalDataController, GameStateMachine gameStateMachine) : base(signalBus)
        {
            this.dungeonLocalDataController = dungeonLocalDataController;
            this.gameStateMachine = gameStateMachine;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.exitButton.onClick.AddListener(this.CloseView);
        }

        public override UniTask BindData()
        {
            this.dungeonLocalDataController.CheckStatusOfAllDungeon();
            this.View.dungeonLevelItems.ForEach(item =>
            {
                item.dungeonIdText.text = item.dungeonId;
                item.onDungeonSelectBtnClick = null;
                item.onDungeonSelectBtnClick += (dungeonId) =>
                {
                    this.dungeonLocalDataController.currentSelectedDungeon = dungeonId;
                    this.gameStateMachine.TransitionTo<GameDungeonModeState>();
                };
                item.gameObject.SetActive(this.dungeonLocalDataController.CheckDungeonIsUnlock(item.dungeonId));
            });
            return UniTask.CompletedTask;
        }
        
    }
}