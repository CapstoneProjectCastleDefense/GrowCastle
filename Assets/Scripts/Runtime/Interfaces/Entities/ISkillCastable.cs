namespace Runtime.Interfaces.Entities
{
    using Runtime.Interfaces.Skills;

    public interface ISkillCastable
    {
        void CastSkill(ISkillPresenter skillPresenter, ITargetable target);
    }
}