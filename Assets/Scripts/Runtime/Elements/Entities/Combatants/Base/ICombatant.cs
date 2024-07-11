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

    public interface ICombatant : IElementModel, IHaveStats
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
        public virtual void                                 OnGetHit(float damage) { }
        public virtual void                                 OnDeath()              { }
        public         ITargetable                          TargetThatImAttacking  { get; set; }
        public         ITargetable                          TargetThatImLookingAt  { get; set; }
        public         ITargetable                          TargetThatAttackingMe  { get; set; }
        public         bool                                 IsDead                 { get; set; }
        public virtual Dictionary<StatEnum, (Type, object)> GetStats()             { return this.Model.Stats; }
        public virtual GameObject                           GetGameObject()        { return this.View.gameObject; }
        public         Dictionary<Type, IEffectTag>        CurrentTag             { get; set; }

        protected override async UniTask InitView()
        {
            await base.InitView();
            this.View.Presenter = this;
        }
    }
}