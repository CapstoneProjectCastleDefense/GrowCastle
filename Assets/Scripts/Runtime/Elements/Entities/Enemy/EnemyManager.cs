namespace Runtime.Elements.Entities.Enemy
{
    using Runtime.Elements.Base;
    using Runtime.Managers.Base;

    public class EnemyManager : BaseElementManager<EnemyModel, EnemyPresenter, EnemyView>
    {
        public EnemyManager(BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>.Factory factory) : base(factory)
        {
        }
        public override void Initialize()        {  }
        public override void DisposeAllElement()
        {
            var cache = this.entities.ToArray();
            this.entities.Clear();
            foreach (var entity in cache)
            {
                entity.Dispose();
            }
        }
    }
}