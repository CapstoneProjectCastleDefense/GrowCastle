namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Signals;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;
    using Zenject;

    public class ArcherNormalAttack : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        public override string SkillId { get; set; } = EntitySkillName.ArcherNormalAttack;

        private Dictionary<ICombatantPresenter, float> castersAttackCooldownTime = new();

        public ArcherNormalAttack(SignalBus signalBus,
                                  FindTargetSystem findTargetSystem,
                                  EffectManager effectManager,
                                  VFXService vfxService,
                                  ProjectileManager projectileManager,
                                  ProjectileBlueprint projectileBlueprint)
            : base(signalBus, findTargetSystem, effectManager, vfxService, projectileManager, projectileBlueprint)
        {
        }

        public override void Cast(ICombatantPresenter caster) { this.castersAttackCooldownTime.Add(caster, 0); }

        public override void Tick()
        {
            base.Tick();
            for (var i = 0; i < this.castersAttackCooldownTime.Count; i++)
            {
                var caster       = this.castersAttackCooldownTime.ElementAt(i).Key;
                var cooldownTime = this.Cooldown(caster, Time.deltaTime);
                if (cooldownTime > 0) continue;

                if (this.Attack(caster))
                {
                    this.ResetCoolDown(caster);
                }
            }
        }

        private float Cooldown(ICombatantPresenter caster, float reduceTime)
        {
            //check if has haste buff, reduce cooldown more
            var stats = caster.GetStats();
            if (stats.TryGetValue(StatEnum.HasteBuff, out (Type type, object value) param))
            {
                reduceTime *= (float)param.value;
            }

            var cooldownTime = this.castersAttackCooldownTime[caster];
            cooldownTime                           -= reduceTime;
            this.castersAttackCooldownTime[caster] =  cooldownTime;
            return cooldownTime;
        }

        private bool Attack(ICombatantPresenter caster)
        {
            var target = this.FindTarget(caster);

            if (target == null) return false;

            var projectileRecord = this.projectileBlueprint.GetDataById(this.SkillId);
            var damage           = caster.GetStats().GetStat<float>(StatEnum.Attack); //todo: calculate damage with crit chance, ...
            this.FireProjectile(new ProjectileModel
                {
                    Id              = this.SkillId,
                    AddressableName = projectileRecord.PrefabName,
                    StartPoint      = caster.GetView().transform.position,
                    EndPoint        = target.GetGameObject().transform.position,
                    Damage          = damage,
                    OnProjectileHit = this.OnProjectileHit
                })
                .Forget();
            return true;
        }

        public ITargetable FindTarget(ICombatantPresenter caster)
        {
            var stats    = caster.GetStats();
            var priority = stats.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);
            if (priority == default)
            {
                stats.SetStat(StatEnum.AttackPriority, AttackPriorityEnum.Default);
            }

            var target = this.findTargetSystem.GetTarget(caster, priority, this.GetTags().ToList(), this.GetManagerTypes());

            return target;
        }

        public override string[] GetTags() { return new[] { "Fly", "Ground", "Boss" }; }

        public override Type[] GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager) }; }

        private void ResetCoolDown(ICombatantPresenter caster)
        {
            var stats       = caster.GetStats();
            var attackSpeed = stats.GetStat<float>(StatEnum.AttackSpeed);
            if (stats.TryGetValue(StatEnum.HasteBuff, out (Type type, object value) param))
            {
                attackSpeed *= (float)param.value;
            }

            var cooldownTime = 1 / attackSpeed;
            this.castersAttackCooldownTime[caster] = cooldownTime;
        }

        protected override void OnProjectileHit(Collider2D collider2D, ProjectilePresenter projectile)
        {
            base.OnProjectileHit(collider2D, projectile);
            var objHit = collider2D.gameObject;
            //todo: check target layer mask from model instead of static input
            if (objHit.layer == LayerMask.NameToLayer("Enemy"))
            {
                var targetableView = objHit.GetComponentInParent<ITargetableView>();
                if (targetableView != null &&
                    !targetableView.GetTargetablePresenter().IsDead)
                {
                    this.effectManager.Execute(targetableView.GetTargetablePresenter(), new InstantDamageTag { Damage = projectile.Model.Damage });
                    projectile.GetView().transform.DOKill();
                    projectile.GetView().Recycle();
                    projectile.isFlyComplete = true;
                    this.RemoveProjectile(projectile);
                }
            }
        }
    }

    public static class ArcherNormalAttackConfig
    {
    }
}