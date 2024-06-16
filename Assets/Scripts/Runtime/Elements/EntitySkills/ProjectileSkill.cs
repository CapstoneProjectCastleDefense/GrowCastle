namespace Runtime.Elements.EntitySkills
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class ProjectileSkill : BaseEntitySkillPresenter<ProjectileSkillModel>
    {
        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Projectile;

        protected override void InternalActivate()
        {
            var projectile = this.Model.ProjectilePrefab;
            projectile.transform.localPosition = Vector3.zero;
            projectile.transform.Fly(this.Model.StartPoint, this.Model.EndPoint, this.Model.Fragment, this.Model.Duration, this.Model.Delay, this.Model.VectorOrientation, () =>
            {
                DOTween.Kill(projectile.transform);
                projectile.Recycle();
            });
        }
    }

    public class ProjectileSkillModel : IEntitySkillModel
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public string                               Description     { get; }
        public string                               Name            { get; }
        public GameObject                           ProjectilePrefab;
        public Dictionary<StatEnum, (Type, object)> Stats;
        public Vector3                              StartPoint;
        public Vector3                              EndPoint;
        public int                                  Fragment; //read in blueprint
        public float                                Duration; //read in blueprint 
        public float                                Delay; //read in blueprint
        public Vector3                              VectorOrientation; //read in blueprint and may set this to offset instead of static data
    }
}