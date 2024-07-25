namespace Runtime.Interfaces.Entities
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;
    using Runtime.Enums;
    using UnityEngine;

    public interface ITargetable : IHaveStats
    {
        void                                OnGetHit(float damage);
        void                                OnDeath();
        ITargetable                         TargetThatImAttacking { get; set; }
        ITargetable                         TargetThatImLookingAt { get; set; }
        ITargetable                         TargetThatAttackingMe { get; set; }
        bool                                IsDead                { get; }
        GameObject                          GetGameObject();
        public Dictionary<Type, IEffectTag> CurrentEffectTags { get; set; }
        public List<ElementTag>             Tags              { get; set; }
    }

    public interface ITargetableView
    {
        public ITargetable GetTargetablePresenter();
    }
}