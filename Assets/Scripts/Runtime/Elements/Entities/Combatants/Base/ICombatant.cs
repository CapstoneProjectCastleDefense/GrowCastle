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
        public         ITargetable                      TargetThatImAttacking  { get; set; }
        public         ITargetable                      TargetThatImLookingAt  { get; set; }
        public         ITargetable                      TargetThatAttackingMe  { get; set; }
        public         bool                             IsDead                 { get; set; }
        Dictionary<StatEnum, (Type, object)> IHaveStats.GetStats()             { return this.Model.Stats; }

        public virtual void UpdateStats() { }

        public virtual GameObject                   GetGameObject() { return this.View.gameObject; }
        public         Dictionary<Type, IEffectTag> CurrentTag      { get; set; } = new();

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.Presenter = this;
        }
    }
}