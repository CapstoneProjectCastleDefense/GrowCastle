namespace Runtime.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Tags;
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

        public List<ITargetable> GetTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, Type[] managerTypes, int count)
        {
            var cache = this.getCustomPresenterSystem.GetAllElementPresenters(managerTypes);
            if (cache.Count == 0) return null;
            var targets = cache.Where(x =>
                    x is ITargetable { IsDead: false } t
                    && x.GetView() != null && x.GetView().LayerMask != host.GetView().LayerMask
                    && (t.TargetThatAttackingMe == null || t.TargetThatAttackingMe.IsDead)
                    && x != host
                    && tagList.Contains(x.GetView().gameObject.tag))
                .Select(x => x as ITargetable)
                .ToList();
            return targets.Count <= count ? targets : this.GetTaggedTarget(host, priority, tagList, targets, count);
        }

        public List<ITargetable> GetTargetsInRange(
            IElementPresenter host,
            AttackPriorityEnum priority,
            List<string> tagList,
            Type[] managerTypes,
            Vector3 center,
            float range)
        {
            var cache = this.getCustomPresenterSystem.GetAllElementPresenters(managerTypes);
            var targets = cache.Where(x =>
                    x is ITargetable { IsDead: false } t
                    && x.GetView().LayerMask != host.GetView().LayerMask
                    && x != host
                    && tagList.Contains(x.GetView().gameObject.tag)
                    && Vector3.Distance(center, x.GetView().transform.position) <= range)
                .Select(x => x as ITargetable)
                .ToList();
            return cache.Count == 0 ? null : targets;
        }

        public List<ITargetable> GetAllEnemyTarget()
        {
            var cache = this.getCustomPresenterSystem.GetAllElementPresenters(typeof(EnemyManager));
            return cache.Where(x =>
                    x is ITargetable { IsDead: false })
                .Select(x => x as ITargetable)
                .ToList();
        }

        public List<ITargetable> GetAllGroundEnemies()
        {
            var cache = this.getCustomPresenterSystem.GetAllElementPresenters(typeof(EnemyManager));
            return cache.Where(x =>
                    x is ITargetable { IsDead: false } target && target.Tags.ContainsAll(new[] { ElementTag.Enemy, ElementTag.Ground }))
                .Select(x => x as ITargetable)
                .ToList();
        }
        
        public List<ITargetable> GetAllFlyEnemies()
        {
            var cache = this.getCustomPresenterSystem.GetAllElementPresenters(typeof(EnemyManager));
            return cache.Where(x =>
                    x is ITargetable { IsDead: false } target && target.Tags.ContainsAll(new[] { ElementTag.Enemy, ElementTag.Fly }))
                .Select(x => x as ITargetable)
                .ToList();
        }

        private List<ITargetable> GetTaggedTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, List<ITargetable> cache, int count)
        {
            List<ITargetable> target = null;
            switch (priority)
            {
                case AttackPriorityEnum.Boss:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Boss.ToString() }, cache, count);
                    break;

                case AttackPriorityEnum.Fly:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Fly.ToString() }, cache, count);
                    break;

                case AttackPriorityEnum.Ground:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Ground.ToString() }, cache, count);
                    break;

                case AttackPriorityEnum.Building:
                    target = this.GetClosestTarget(host, new() { AttackPriorityEnum.Building.ToString() }, cache, count);
                    break;
            }

            if (target == null)
            {
                target = this.GetNormalTarget(host, priority, tagList, cache, count);
            }

            return target;
        }

        private List<ITargetable> GetClosestTarget(IElementPresenter host, List<string> tagList, List<ITargetable> cache, int count)
        {
            cache = cache.Where(x => tagList.Contains(x.GetGameObject().tag)).ToList();
            if (cache.Count == 0)
                return null;
            return cache.OrderBy(x => Vector3.Distance(host.GetView().transform.position, x.GetGameObject().transform.position)).Take(count).ToList();
        }

        private List<ITargetable> GetNormalTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, List<ITargetable> cache, int count)
        {
            List<ITargetable> target;
            switch (priority)
            {
                case AttackPriorityEnum.LowHealth:
                    target = this.GetTargetByHealth(cache, false, count);
                    break;

                case AttackPriorityEnum.HighHealth:
                    target = this.GetTargetByHealth(cache, true, count);
                    break;

                default:
                    target = this.GetClosestTarget(host, tagList, cache, count);
                    break;
            }

            return target;
        }

        private List<ITargetable> GetTargetByHealth(List<ITargetable> cache, bool getHigh, int count)
        {
            cache = cache.OrderBy(x => x.GetStats().GetStat<float>(StatEnum.Health)).ToList();
            return getHigh ? cache.GetRange(cache.Count - count, count) : cache.GetRange(0, count);
        }
    }
}