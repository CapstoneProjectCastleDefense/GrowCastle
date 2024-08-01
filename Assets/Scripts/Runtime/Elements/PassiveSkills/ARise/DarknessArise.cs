namespace Runtime.Elements.PassiveSkills.ARise
{
    using Models.Tags;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;

    public class DarknessArise : IPassiveSkillPresenter
    {
        private readonly EnemyManager  enemyManager;
        private readonly EffectManager effectManager;
        public           HeroPresenter HeroPresenter { get; set; }

        public DarknessArise(EnemyManager enemyManager, EffectManager effectManager)
        {
            this.enemyManager  = enemyManager;
            this.effectManager = effectManager;
        }

        public void Init()
        {
            this.HeroPresenter.OnActiveSkillCasted += this.ActiveSkill;
        }

        public void Tick()
        {
        }

        public void ActiveSkill()
        {
            this.enemyManager.GetAllBossEnemies().ForEach(boss =>
            {
                boss.onAttackComplete = this.OnEnemyAttackComplete;
            });
        }

        private void OnEnemyAttackComplete(EnemyPresenter enemyPresenter)
        {
            this.effectManager.AddEffectToTarget(enemyPresenter, new InstantDamageTag() { Damage = enemyPresenter.Model.GetStat<float>(StatEnum.Attack) * 0.15f });
        }
    }
}