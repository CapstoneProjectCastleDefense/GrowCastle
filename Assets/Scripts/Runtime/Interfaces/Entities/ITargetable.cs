namespace Runtime.Interfaces.Entities
{
    public interface ITargetable
    {
        void OnGetHit(float damage);
        void OnDeath();
    }
}