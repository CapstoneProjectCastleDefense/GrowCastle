namespace Runtime.Elements.Base
{
    using System.Collections.Generic;
    using Runtime.Interfaces.Entities;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class BaseCombatantView : BaseElementView, ITargetableView
    {
        public ITargetable GetTargetablePresenter() => (ITargetable)this.Presenter;

        [field: SerializeField] public List<string> attackAnimations;
        [field: SerializeField] public List<string> deathAnimations;
        [field: SerializeField] public string       moveAnimation;
        [field: SerializeField] public List<string> targetTags;
        [field: SerializeField] public List<string> selfTags;
        [field: SerializeField] public Transform    ProjectileSpawnPoint { get; private set; }
    }
}