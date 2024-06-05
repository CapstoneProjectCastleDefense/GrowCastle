namespace Runtime.Managers
{
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Managers.Base;

    public class MapLevelManager : BaseElementManager<MapLevelModel,MapLevelPresenter,MapLevelView>
    {
        public MapLevelManager(BaseElementPresenter<MapLevelModel, MapLevelView, MapLevelPresenter>.Factory factory)
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