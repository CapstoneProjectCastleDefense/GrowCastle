namespace Runtime.Scenes.Commons
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Scenes.Adapters.Evolution;
    using Runtime.Services;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class ElementGenericInfoView : MonoBehaviour
    {
        private string                  selectedSkillId;
        private bool                    disableSelectAbility;
        private ElementGenericInfoModel model;

        [SerializeField] private SkeletonGraphic avatarAnim;
        [SerializeField] private TMP_Text        skillDescription, levelTxt, attackInfoTxt, attackInfoSpeedTxt;
        [SerializeField] private AbilityAdapter  abilityAdapter;

        private IGameAssets             gameAssets;
        private EvolutionInfoBlueprint  evolutionInfoBlueprint;
        private DiContainer             diContainer;
        private HeroLocalDataController heroLocalDataController;
        private HeroUpgradeService      heroUpgradeService;
        private SkillBlueprint          skillBlueprint;
        private ElementSkinBlueprint    elementSkinBlueprint;

        [Inject]
        public void Construct(
            IGameAssets gameAssetsInject,
            EvolutionInfoBlueprint evolutionInfoBlueprintInject,
            DiContainer diContainerInject,
            HeroLocalDataController heroLocalDataControllerInject,
            HeroUpgradeService heroUpgradeServiceInject,
            SkillBlueprint skillBlueprintInject,
            ElementSkinBlueprint elementSkinBlueprintInject
        )
        {
            this.gameAssets              = gameAssetsInject;
            this.evolutionInfoBlueprint  = evolutionInfoBlueprintInject;
            this.diContainer             = diContainerInject;
            this.heroLocalDataController = heroLocalDataControllerInject;
            this.heroUpgradeService      = heroUpgradeServiceInject;
            this.skillBlueprint          = skillBlueprintInject;
            this.elementSkinBlueprint    = elementSkinBlueprintInject;
        }

        public async void BindData(ElementGenericInfoModel infoModel)
        {
            this.model = infoModel;
            var heroRuntimeData = this.heroLocalDataController.GetHeroRuntimeData(this.model.ElementId);
            this.levelTxt.text = $"{this.heroUpgradeService.GetHeroLevel(this.model.ElementId)}";
            this.attackInfoTxt.text
                = $"{Math.Round(this.heroLocalDataController.GetStatAfterEquipItem(StatEnum.Attack, this.heroUpgradeService.GetCurrentAttack(this.model.ElementId), this.model.ElementId), 1)}";
            this.attackInfoSpeedTxt.text = $"{Math.Round(this.heroLocalDataController.GetStatAfterEquipItem(StatEnum.AttackSpeed, 1f, this.model.ElementId), 1)}";

            var skeletonDataAsset = await this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(heroRuntimeData.heroRecord.SkeletonDataAsset);
            this.avatarAnim.ChangeSkeletonDataAsset(skeletonDataAsset, "idle");
            this.SetSkin();
            if (heroRuntimeData.heroRecord.HeroType == SlotType.Tower)
            {
                this.abilityAdapter.gameObject.SetActive(false);
                return;
            }
            this.abilityAdapter.gameObject.SetActive(true);
            this.InitAdapter(infoModel).Forget();
        }

        public void Rebind()
        {
            this.levelTxt.text = $"{this.heroUpgradeService.GetHeroLevel(this.model.ElementId)}";
            this.attackInfoTxt.text
                = $"{Math.Round(this.heroLocalDataController.GetStatAfterEquipItem(StatEnum.Attack, this.heroUpgradeService.GetCurrentAttack(this.model.ElementId), this.model.ElementId), 1)}";
            this.attackInfoSpeedTxt.text = $"{Math.Round(this.heroLocalDataController.GetStatAfterEquipItem(StatEnum.AttackSpeed, 1f, this.model.ElementId), 1)}";
            this.SetSkin();
        }

        private async UniTaskVoid InitAdapter(ElementGenericInfoModel elementGenericInfoModel)
        {
            var skills       = this.evolutionInfoBlueprint.GetDataById(elementGenericInfoModel.EvolutionId).Abilities;
            var skillRecords = skills.Select(abilityId => this.skillBlueprint.GetDataById(abilityId)).ToList();

            var firstSkill = skillRecords.First();
            this.skillDescription.text = firstSkill.Description;
            this.selectedSkillId       = firstSkill.Id;

            var modelList = skillRecords.Select(record => new AbilityUIModel()
                {
                    Id         = record.Id,
                    Icon       = record.Icon,
                    OnSelected = this.OnAbilityUISelected,
                    SelectedId = this.selectedSkillId
                })
                .ToList();

            this.disableSelectAbility = true;
            await this.abilityAdapter.InitItemAdapter(modelList, this.diContainer);
            this.OnAbilityUISelected(this.selectedSkillId);
            this.disableSelectAbility = false;
        }

        private void OnAbilityUISelected(string abilityId)
        {
            if (this.disableSelectAbility) return;

            this.selectedSkillId = abilityId;
            var skills       = this.evolutionInfoBlueprint.GetDataById(this.model.EvolutionId).Abilities;
            var skillRecords = skills.Select(id => this.skillBlueprint.GetDataById(id)).ToDictionary(a => a.Id);
            var description  = skillRecords[this.selectedSkillId].Description;
            this.skillDescription.text = description;
            foreach (var abilityUIPresenter in this.abilityAdapter.GetPresenters())
            {
                abilityUIPresenter.UpdateSelectedId(abilityId);
            }
        }
        
        private void SetSkin()
        {
            var id            = this.model.ElementId;
            var heroLocalData = this.heroLocalDataController.GetHeroLocalData(id);
            var selectSkin    = this.elementSkinBlueprint.GetSkinByLevel(id, heroLocalData.Level);
            this.avatarAnim.Skeleton.SetSkin(selectSkin);
            this.avatarAnim.Skeleton.SetSlotsToSetupPose();
            this.avatarAnim.LateUpdate();
        }
    }

    public class ElementGenericInfoModel
    {
        public string ElementId;
        public string EvolutionId;
    }
}