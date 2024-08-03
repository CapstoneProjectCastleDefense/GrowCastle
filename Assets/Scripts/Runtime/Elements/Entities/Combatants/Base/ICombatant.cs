namespace Runtime.Elements.Base
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public interface ICombatant : IElementModel, IHaveStatsModel
    {
    }

    public interface ICombatantPresenter : IElementPresenter, ITargetable
    {
    }

    public abstract class BaseCombatantPresenter<TModel, TView, TPresenter> : BaseElementPresenter<TModel, TView, TPresenter>, ICombatantPresenter
        where TModel : ICombatant
        where TView : BaseCombatantView
        where TPresenter : BaseCombatantPresenter<TModel, TView, TPresenter>
    {
        protected BaseCombatantPresenter(TModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
        public virtual void                             OnGetHit(float damage) { }
        public virtual void                             OnDeath()              { }
        public virtual ITargetable                      TargetThatImAttacking  { get; set; }
        public virtual ITargetable                      TargetThatImLookingAt  { get; set; }
        public virtual ITargetable                      TargetThatAttackingMe  { get; set; }
        public virtual bool                             IsDead                 { get; set; }
        Dictionary<StatEnum, (Type, object)> IHaveStats.GetStats()             { return this.Model.Stats; }

        public virtual void UpdateStats() { }

        public virtual GameObject                   GetGameObject()   { return this.View.gameObject; }
        public virtual Dictionary<Type, IEffectTag> CurrentEffectTags { get; set; } = new();
        public virtual List<ElementTag>             Tags              { get; set; }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.Presenter = this;
        }
    }
}