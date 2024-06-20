namespace Runtime.Managers
{
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Leader;
    using Runtime.Managers.Base;

    public class LeaderManager : BaseElementManager<LeaderModel, LeaderPresenter, LeaderView>
    {
        public LeaderManager(BaseElementPresenter<LeaderModel, LeaderView, LeaderPresenter>.Factory factory) : base(factory) { }
        public override void Initialize()        { }
        public override void DisposeAllElement() { }
    }
}