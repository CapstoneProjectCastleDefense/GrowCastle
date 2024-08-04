namespace Runtime.Scenes.Commons
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Adapters.Evolution;
    using Runtime.Services;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class ElementGenericInfoView : MonoBehaviour
    {
        private string                  selectedAbilityId;
        private bool                    disableSelectAbility;
        private ElementGenericInfoModel model;

        [SerializeField] private SkeletonGraphic avatarAnim;
        [SerializeField] private TMP_Text        skillDescription, levelTxt, attackInfoTxt, attackInfoSpeedTxt;
        [SerializeField] private AbilityAdapter  abilityAdapter;

        private IGameAssets             gameAssets;
        private EvolutionInfoBlueprint  evolutionInfoBlueprint;
        private DiContainer             diContainer;
        private HeroLocalDataController heroLocalDataController;
        private AbilityInfoBlueprint    abilityInfoBlueprint;
        private ElementUpgradeService   elementUpgradeService;

        [Inject]
        public void Construct(IGameAssets gameAssetsInject,
            EvolutionInfoBlueprint evolutionInfoBlueprintInject,
            DiContainer diContainerInject,
            HeroLocalDataController heroLocalDataControllerInject,
            AbilityInfoBlueprint abilityInfoBlueprintInject,
            ElementUpgradeService elementUpgradeServiceInject)
        {
            this.gameAssets              = gameAssetsInject;
            this.evolutionInfoBlueprint  = evolutionInfoBlueprintInject;
            this.diContainer             = diContainerInject;
            this.heroLocalDataController = heroLocalDataControllerInject;
            this.abilityInfoBlueprint    = abilityInfoBlueprintInject;
            this.elementUpgradeService   = elementUpgradeServiceInject;
        }

        public void BindData(ElementGenericInfoModel infoModel)
        {
            this.model = infoModel;
            var heroRuntimeData = this.heroLocalDataController.GetHeroRuntimeData(this.model.ElementId);
            this.levelTxt.text           = $"{this.elementUpgradeService.GetElementLevel(this.model.ElementId)}";
            this.attackInfoTxt.text      = $"{(int)this.elementUpgradeService.GetCurrentAttack(this.model.ElementId)}";
            this.attackInfoSpeedTxt.text = $"{heroRuntimeData.attackSpeed}";

            var skeletonDataAsset = this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(heroRuntimeData.heroRecord.SkeletonDataAsset).WaitForCompletion();
            this.avatarAnim.ChangeSkeletonDataAsset(skeletonDataAsset, "idle");

            this.InitAdapter(infoModel).Forget();
        }

        public void Rebind()
        {
            this.levelTxt.text      = $"{this.elementUpgradeService.GetElementLevel(this.model.ElementId)}";
            this.attackInfoTxt.text = $"{(int)this.elementUpgradeService.GetCurrentAttack(this.model.ElementId)}";
        }

        private async UniTaskVoid InitAdapter(ElementGenericInfoModel elementGenericInfoModel)
        {
            var abilities      = this.evolutionInfoBlueprint.GetDataById(elementGenericInfoModel.EvolutionId).Abilities;
            var abilityRecords = abilities.Select(abilityId => this.abilityInfoBlueprint.GetDataById(abilityId)).ToList();

            var firstAbility = abilityRecords.First();
            this.skillDescription.text = firstAbility.AbilityDescription;
            this.selectedAbilityId     = firstAbility.AbilityId;

            var modelList = new List<AbilityUIModel>();

            foreach (var record in abilityRecords)
            {
                var abilityUIModel = new AbilityUIModel()
                {
                    Id         = record.AbilityId,
                    OnSelected = this.OnAbilityUISelected,
                    SelectedId = this.selectedAbilityId
                };

                modelList.Add(abilityUIModel);
            }

            this.disableSelectAbility = true;
            await this.abilityAdapter.InitItemAdapter(modelList, this.diContainer);
            this.OnAbilityUISelected(this.selectedAbilityId);
            this.disableSelectAbility = false;
        }

        private void OnAbilityUISelected(string abilityId)
        {
            if (this.disableSelectAbility) return;

            this.selectedAbilityId = abilityId;
            var abilities      = this.evolutionInfoBlueprint.GetDataById(this.model.EvolutionId).Abilities;
            var abilityRecords = abilities.Select(id => this.abilityInfoBlueprint.GetDataById(id)).ToDictionary(a => a.AbilityId);
            var description    = abilityRecords[this.selectedAbilityId].AbilityDescription;
            this.skillDescription.text = description;
        }
    }

    public class ElementGenericInfoModel
    {
        public string ElementId;
        public string EvolutionId;
    }
}