namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.LocalData.LocalDataController;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class SettingPopupView : BaseView
    {
        public Slider musicValue;
        public Slider soundValue;
        public Button exitBtn;
        
        public Transform  startPos;
        public Transform  endPos;
        public GameObject viewField;
    }
    [PopupInfo(nameof(SettingPopupView))]
    public class SettingScreenPresenter : BasePopupPresenter<SettingPopupView>
    {
        private readonly UserLocalDataController userLocalDataController;
        public SettingScreenPresenter(SignalBus signalBus, UserLocalDataController userLocalDataController)
            : base(signalBus)
        {
            this.userLocalDataController = userLocalDataController;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.musicValue.onValueChanged.AddListener(this.OnMusicValueChange);
            this.View.soundValue.onValueChanged.AddListener(this.OnSoundValueChange);
            this.View.exitBtn.onClick.AddListener(this.CloseView);
        }
        public override UniTask BindData()
        {
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            this.View.musicValue.value = this.userLocalDataController.GetMusic.Value;
            this.View.soundValue.value = this.userLocalDataController.GetSound.Value;
            return UniTask.CompletedTask;
        }

        private void OnMusicValueChange(float value) => this.userLocalDataController.GetMusic.Value = value;
        private void OnSoundValueChange(float value) => this.userLocalDataController.GetSound.Value = value;
        
        public override void CloseView() { this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.InOutQuint).onComplete += () => { base.CloseView(); }; }
        
    }
}