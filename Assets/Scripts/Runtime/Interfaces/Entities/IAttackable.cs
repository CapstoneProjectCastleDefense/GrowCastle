namespace Runtime.Interfaces.Entities
{
    public interface IAttackable
    {
        void Attack(ITargetable target);
        ITargetable FindTarget();
    }
}