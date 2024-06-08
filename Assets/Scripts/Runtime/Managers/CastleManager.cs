namespace Runtime.Managers
{
    using System.Linq;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Managers.Base;

    public class CastleManager : BaseElementManager<CastleModel,CastlePresenter,CastleView>
    {
        private readonly CastleLocalDataController castleLocalDataController;

        public CastleManager(BaseElementPresenter<CastleModel, CastleView, CastlePresenter>.Factory factory, CastleLocalDataController castleLocalDataController)
            : base(factory)
        {
            this.castleLocalDataController = castleLocalDataController;
        }
        public override void Initialize()
        {

        }
        public override void DisposeAllElement()
        {

        }
        public void UpgradeCastle()
        {
            this.castleLocalDataController.UpgradeCastle();
            this.entities.First().UpdateBlockBaseOnCurrentLevel();
        }
    }
}