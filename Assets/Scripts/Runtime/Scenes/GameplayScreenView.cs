namespace Runtime.Scenes
{
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

        public override async UniTask BindData()
        {
            this.View.backGround.DOFade(0, 1).SetEase(Ease.OutQuad);
        }
    }
}