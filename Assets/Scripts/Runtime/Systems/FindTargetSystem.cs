namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers.Entity;
    using UnityEngine;

    public class FindTargetSystem : IGameSystem
    {
        private readonly GetCustomPresenterSystem           getCustomPresenterSystem;
        private          List<IElementPresenter> Presenters   => this.getCustomPresenterSystem.GetAllElementPresenter().ToList();
        public           void                    Initialize() { }
        public           void                    Tick()       { }
        public           void                    Dispose()    { }

        public FindTargetSystem(GetCustomPresenterSystem getCustomPresenterSystem) { this.getCustomPresenterSystem = getCustomPresenterSystem; }

        public ITargetable GetTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList)
        {
            var cache = this.Presenters.Where(x =>
                x is ITargetable
                && x.GetView().LayerMask != host.GetView().LayerMask
                && x != host && tagList.Contains(x.GetView().Tag)
            ).ToList();
            return cache.Count == 0 ? null : this.GetTaggedTarget(host, priority, tagList, cache) as ITargetable;
        }

        private IElementPresenter GetTaggedTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, List<IElementPresenter> cache)
        {
            IElementPresenter target = null;
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

        private IElementPresenter GetClosestTarget(IElementPresenter host, List<string> tagList, List<IElementPresenter> cache)
        {
            cache = cache.Where(x => tagList.Contains(x.GetView().Tag)).ToList();
            if (cache.Count == 0)
                return null;
            return cache.OrderBy(x => Vector3.Distance(host.GetView().transform.position, x.GetView().transform.position)).First();
        }

        private IElementPresenter GetNormalTarget(IElementPresenter host, AttackPriorityEnum priority, List<string> tagList, List<IElementPresenter> cache)
        {
            IElementPresenter target = null;
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
        private IElementPresenter GetTargetByHealth(List<IElementPresenter> cache, bool getHigh)
        {
            cache = cache.OrderBy(x => x.GetModelGeneric<IHaveStats>().GetStat<float>(StatEnum.Health)).ToList();
            return getHigh ? cache.Last() : cache.First();
        }
    }
}