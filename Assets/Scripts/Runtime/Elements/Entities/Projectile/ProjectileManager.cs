namespace Runtime.Elements.Entities.Projectile
{
    using Runtime.Elements.Base;
    using Runtime.Managers.Base;

    public class ProjectileManager : BaseElementManager<ProjectileModel, ProjectilePresenter, ProjectileView>
    {
        public ProjectileManager(BaseElementPresenter<ProjectileModel, ProjectileView, ProjectilePresenter>.Factory factory) : base(factory) { }
        public override void Initialize() { }

        public override void DisposeAllElement() { }
    }
}