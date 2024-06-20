namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using UnityEngine;

    public class HeroManager : BaseElementManager<HeroModel, HeroPresenter,HeroView>
    {
        public HeroManager(BaseElementPresenter<HeroModel, HeroView, HeroPresenter>.Factory factory) : base(factory)
        {
        }

        public void CreateSingleHero(string id,Transform parent)
        {
            this.CreateElement(new() {
                Id         = id,
                ParentView = parent,
                Stats = new()
                {
                    { StatEnum.Attack, (typeof(float), 2f) },
                    { StatEnum.Health, (typeof(float), 10f) },
                    { StatEnum.AttackSpeed, (typeof(float), 1f) },
                },
            }).UpdateView().Forget();
        }

        public void ChangeAttackStatusOfAllHero(bool canAttack)
        {
            this.entities.ForEach(e=>e.SetAttackStatus(canAttack));
        }

        public override void Initialize()
        {

        }

        public override void DisposeAllElement()
        {

        }
    }
}