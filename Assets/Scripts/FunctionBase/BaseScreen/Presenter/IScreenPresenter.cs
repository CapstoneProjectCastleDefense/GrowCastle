namespace FunctionBase.BaseScreen.Presenter
{
    using System;
    using Cysharp.Threading.Tasks;
    using FunctionBase.BaseScreen.View;
    using Zenject;

    public interface IScreenPresenter : IInitializable, IDisposable
    {
        UniTask BindData();
        UniTask OpenViewAsync();
        UniTask CloseViewAsync();
        void    CloseView();
        void    HideView();
        void    OnViewReady();
        UniTask SetView(IScreenView view);
    }

    public abstract class ScreenPresenter<TView> : IScreenPresenter where TView : IScreenView
    {
        public TView View { get; private set; }

        public ScreenPresenter() { }

        public virtual  void    Initialize() { }
        public virtual  void    Dispose()    { }
        public abstract UniTask BindData();

        public virtual UniTask OpenViewAsync()
        {
            if (this.View == null)
            {
                throw new NullReferenceException("View is null");
            }

            return this.View.ScreenStatus == ScreenStatus.Opened ? UniTask.CompletedTask : this.View.Open();
        }
        public virtual UniTask CloseViewAsync()
        {
            if (this.View == null)
            {
                throw new NullReferenceException("View is null");
            }

            return this.View.ScreenStatus == ScreenStatus.Closed ? UniTask.CompletedTask : this.View.Close();
        }
        public         void CloseView()   { this.CloseViewAsync().Forget(); }
        
        public virtual void HideView()
        {
            if (this.View == null)
            {
                throw new NullReferenceException("View is null");
            }
            if(this.View.ScreenStatus == ScreenStatus.Hide) return;
            this.View.Hide();
            this.Dispose();
        }
        public virtual void OnViewReady() { }

        public async UniTask SetView(IScreenView view)
        {
            this.View = (TView)view;
            await UniTask.WaitUntil(() => this.View.IsReady);
            this.OnViewReady();
        }
    }
    
    public abstract class PopupPresenter<TView> : ScreenPresenter<TView> where TView : IScreenView
    {
        public PopupPresenter() { }

        public override async UniTask OpenViewAsync()
        {
            await this.BindData();

            if (this.View.ScreenStatus == ScreenStatus.Opened) return;
            await this.View.Open();
        }
        public override async UniTask CloseViewAsync()
        {
            if (this.View.ScreenStatus == ScreenStatus.Closed) return;
            await this.View.Close();
            this.Dispose();
        }
        public override void HideView()
        {
            if (this.View.ScreenStatus == ScreenStatus.Hide) return;
            this.View.Hide();
            this.Dispose();
        }
    }
}