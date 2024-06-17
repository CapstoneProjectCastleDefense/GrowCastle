namespace Runtime.Interfaces.Entities
{
    public interface ITargetable
    {
        void        OnGetHit(float damage);
        void        OnDeath();
        ITargetable TargetThatImAttacking { get; set; }
        ITargetable TargetThatAttackingMe { get; set; }
        bool        IsDead { get; }
    }
}