namespace Runtime.Elements.Base
{
    using System.Collections.Generic;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class BaseCombatantView : BaseElementView, ITargetableView
    {
        public ITargetable GetTargetablePresenter() => (ITargetable)this.Presenter;

        [field: SerializeField] public List<string> attackAnimations;
        [field: SerializeField] public List<string> deathAnimations;
        [field: SerializeField] public string       moveAnimation;
    }
}