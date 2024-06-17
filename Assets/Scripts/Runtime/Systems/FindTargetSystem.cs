namespace Runtime.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers.Entity;
    using UnityEngine;

    public class FindTargetSystem : IGameSystem
    {
        private readonly EntityManager     entityManager;
        private          List<ITargetable> Targetables  => this.entityManager.GetAllElementPresenter().Select(x => x as ITargetable).ToList();
        public           void              Initialize() { }
        public           void              Tick()       { }
        public           void              Dispose()    { }

        public FindTargetSystem(EntityManager entityManager) { this.entityManager = entityManager; }

        public ITargetable GetTarget(ITargetable host, AttackPriorityEnum priority, List<string> tagList)
        {
            var cache = this.Targetables.Where(x => x.LayerMask != host.LayerMask && x != host && tagList.Contains(x.Tag)).ToList();
            return cache.Count == 0 ? null : this.GetTaggedTarget(host, priority, tagList, cache);
        }

        private ITargetable GetTaggedTarget(ITargetable host, AttackPriorityEnum priority, List<string> tagList, List<ITargetable> cache)
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

        private ITargetable GetClosestTarget(ITargetable host, List<string> tagList, List<ITargetable> cache)
        {
            cache = cache.Where(x => tagList.Contains(x.Tag)).ToList();
            if (cache.Count == 0)
                return null;
            return cache.OrderBy(x => Vector3.Distance(host.GetView<Transform>().position, x.GetView<Transform>().position)).First();
        }

        private ITargetable GetNormalTarget(ITargetable host, AttackPriorityEnum priority, List<string> tagList, List<ITargetable> cache)
        {
            ITargetable target = null;
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
            cache = cache.OrderBy(x => x.GetModel<float>()).ToList();
            return getHigh ? cache.Last() : cache.First();
        }
    }
}