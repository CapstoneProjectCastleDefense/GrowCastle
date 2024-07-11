namespace Runtime.Elements.Base
{
    using Runtime.Interfaces.Entities;

    public class BaseCombatantView : BaseElementView, ITargetableView
    {
        public ITargetable GetTargetablePresenter() => (ITargetable)this.Presenter;
    }
}