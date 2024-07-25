namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using System.Collections.Generic;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Interfaces.Skills;
    using Runtime.Services;
    using Runtime.StaticValues;
    using UnityEngine;

    public class CursedVolleyInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly VFXService        vfxService;
        private readonly ObjectPoolManager objectPoolManager;
        private readonly IGameAssets       gameAssets;
        public CursedVolleyInstantHitSkill(SkillAttackBlueprint skillAttackBlueprint, VFXService vfxService, ObjectPoolManager objectPoolManager, IGameAssets gameAssets)
            : base(skillAttackBlueprint)
        {
            this.vfxService        = vfxService;
            this.objectPoolManager = objectPoolManager;
            this.gameAssets        = gameAssets;
        }
        public override string SkillId { get; set; } = EntitySkillName.CursedVolley;
        protected override async void InternalActivate()
        {
            List<GameObject> arrows   = new();
            Vector3          startPos = new Vector3(-1f, -1, 0);
            float            range    = 0.5f;
            for (int i = 0; i < 3; i++)
            {
                var arrowPrefab = this.gameAssets.LoadAssetAsync<GameObject>(this.VFXName).WaitForCompletion();
                var arrow       = this.objectPoolManager.Spawn(arrowPrefab, startPos + new Vector3(range * i, 0),Quaternion.identity);
                arrows.Add(arrow);
            }
            
        }
    }
}