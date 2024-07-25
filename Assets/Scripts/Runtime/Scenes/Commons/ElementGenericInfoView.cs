namespace Runtime.Scenes.Commons
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Scenes.Popups;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class ElementGenericInfoView : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic avatarAnim;
        [SerializeField] private TMP_Text        skillDescription, attackInfoTxt, attackInfoSpeedTxt;

        private SkillBlueprint skillBlueprint;
        private IGameAssets    gameAssets;

        [Inject]
        public void Construct(SkillBlueprint skillBlueprint, IGameAssets gameAssets)
        {
            this.skillBlueprint = skillBlueprint;
            this.gameAssets     = gameAssets;
        }
        
        public void BindData(CharacterInfoPopupModel popupModel)
        {
            this.skillDescription.text   = this.skillBlueprint.GetDataById(popupModel.HeroRuntimeData.heroRecord.ActiveSkill.skillName).Description;
            this.attackInfoTxt.text      = $"{popupModel.HeroRuntimeData.attack}";
            this.attackInfoSpeedTxt.text = $"{popupModel.HeroRuntimeData.attackSpeed}";
            
            var skeletonDataAsset = this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(popupModel.HeroRuntimeData.heroRecord.SkeletonDataAsset).WaitForCompletion();
            this.avatarAnim.ChangeSkeletonDataAsset(skeletonDataAsset, "idle");
        }
    }
}