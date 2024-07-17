namespace Runtime.Combat.CombatActions.Models
{
    using System.Collections.Generic;
    using global::Models.Tags;
    using Runtime.Interfaces.Entities;

    public class InstantHitModel : ICombatActionModel
    {
        public ITargetable      Target     { get; set; }
        public string           VfxName    { get; set; }
        public List<IEffectTag> EffectTags { get; set; }
    }
}