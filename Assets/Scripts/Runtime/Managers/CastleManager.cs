namespace Runtime.Managers
{
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Managers.Base;

    public class CastleManager : BaseElementManager<CastleModel,CastlePresenter,CastleView>
    {
        public CastleManager(BaseElementPresenter<CastleModel, CastleView, CastlePresenter>.Factory factory)
            : base(factory)
        {
        }
        public override void Initialize()
        {
            
        }
        public override void DisposeAllElement()
        {
            
        }
    }
}