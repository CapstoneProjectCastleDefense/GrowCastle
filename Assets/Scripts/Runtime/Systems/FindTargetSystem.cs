namespace Runtime.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Managers.Entity;
    using UnityEngine;

    public class FindTargetSystem : IGameSystem
    {
        public void Initialize() { }
        public void Tick()       { }
        public void Dispose()    { }

        private readonly GetCustomPresenterSystem getCustomPresenterSystem;
        public FindTargetSystem(GetCustomPresenterSystem getCustomPresenterSystem) { this.getCustomPresenterSystem = getCustomPresenterSystem; }

        public ITargetable GetTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, Type[] managerTypes)
        {
            var cache = this.getCustomPresenterSystem.GetAllElementPresenters(managerTypes);
            var targets = cache.Where(x =>
                    x is ITargetable { IsDead: false } t
                    && (t.TargetThatAttackingMe == null || t.TargetThatAttackingMe.IsDead)
                    && x.GetView().LayerMask != host.GetView().LayerMask
                    && x != host
                    && tagList.Contains(x.GetView().gameObject.tag))
                .Select(x => x as ITargetable)
                .ToList();
            return cache.Count == 0 ? null : this.GetTaggedTarget(host, priority, tagList, targets);
        }

        public List<ITargetable> GetAllEnemyTarget()
        {
            var cache = this.getCustomPresenterSystem.GetAllElementPresenters(typeof(EnemyManager));
            return cache.Where(x =>
                    x is ITargetable { IsDead: false })
                .Select(x => x as ITargetable)
                .ToList();
        }

        private ITargetable GetTaggedTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, List<ITargetable> cache)
        {
            ITargetable target = null;
            switch (priority)
            {
                case AttackPriorityEnum.Boss:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Boss.ToString() }, cache);
                    break;

                case AttackPriorityEnum.Fly:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Fly.ToString() }, cache);
                    break;

                case AttackPriorityEnum.Ground:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Ground.ToString() }, cache);
                    break;

                case AttackPriorityEnum.Building:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Building.ToString() }, cache);
                    break;
            }

            if (target == null)
            {
                target = this.GetNormalTarget(host, priority, tagList, cache);
            }

            return target;
        }

        private ITargetable GetClosestTarget(IElementPresenter host, List<string> tagList, List<ITargetable> cache)
        {
            cache = cache.Where(x => tagList.Contains(x.GetGameObject().tag)).ToList();
            if (cache.Count == 0)
                return null;
            return cache.OrderBy(x => Vector3.Distance(host.GetView().transform.position, x.GetGameObject().transform.position)).First();
        }

        private ITargetable GetNormalTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, List<ITargetable> cache)
        {
            ITargetable target;
            switch (priority)
            {
                case AttackPriorityEnum.LowHealth:
                    target = this.GetTargetByHealth(cache, false);
                    break;

                case AttackPriorityEnum.HighHealth:
                    target = this.GetTargetByHealth(cache, true);
                    break;

                default:
                    target = this.GetClosestTarget(host, tagList, cache);
                    break;
            }

            return target;
        }

        private ITargetable GetTargetByHealth(List<ITargetable> cache, bool getHigh)
        {
            cache = cache.OrderBy(x => x.GetStats().GetStat<float>(StatEnum.Health)).ToList();
            return getHigh ? cache.Last() : cache.First();
        }
    }
}