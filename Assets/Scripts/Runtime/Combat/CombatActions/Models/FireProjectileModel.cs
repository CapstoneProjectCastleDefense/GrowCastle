namespace Runtime.Combat.CombatActions.Models
{
    using System.Collections.Generic;
    using global::Models.Tags;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;

    public class FireProjectileModel : ICombatActionModel
    {
        public string              ProjectileId       { get; set; }
        public ITargetable         Target             { get; set; }
        public ICombatantPresenter CombatantPresenter { get; set; }
        public List<IEffectTag>    EffectTags         { get; set; }

        public FireProjectileModel(string projectileId, ITargetable target, ICombatantPresenter combatantPresenter, List<IEffectTag> effectTags)
        {
            this.ProjectileId       = projectileId;
            this.Target             = target;
            this.CombatantPresenter = combatantPresenter;
            this.EffectTags         = effectTags;
        }
    }
}