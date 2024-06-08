namespace Runtime.Scenes
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using UnityEngine.UI;
    using Zenject;

    public class GameplayScreenView : BaseView
    {
        public Image backGround;
    }

    [ScreenInfo(nameof(GameplayScreenView))]
    public class GameplayScreenPresenter : BaseScreenPresenter<GameplayScreenView>
    {
        public GameplayScreenPresenter(SignalBus signalBus) : base(signalBus)
        {
        }
        
        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync().Forget();
        }

        public override UniTask BindData()
        {
            UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() =>
            {
                this.View.backGround.DOFade(0, 2).SetEase(Ease.OutQuad);
            });
            return UniTask.CompletedTask;
        }
    }
}