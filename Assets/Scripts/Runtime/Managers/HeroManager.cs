namespace Runtime.Managers
{
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Managers.Base;

    public class HeroManager : BaseElementManager<HeroModel, HeroPresenter,HeroView>
    {
        public HeroManager(BaseElementPresenter<HeroModel, HeroView, HeroPresenter>.Factory factory) : base(factory)
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