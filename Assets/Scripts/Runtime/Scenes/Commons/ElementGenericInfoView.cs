namespace Runtime.Scenes.Commons
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Adapters.Evolution;
    using Runtime.Scenes.Popups;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class ElementGenericInfoView : MonoBehaviour
    {
        private string selectedAbilityId;
        private bool   disableSelectAbility;

        private const string ElementId = "knight_evolve_1";

        [SerializeField] private SkeletonGraphic avatarAnim;
        [SerializeField] private TMP_Text        skillDescription, attackInfoTxt, attackInfoSpeedTxt;
        [SerializeField] private AbilityAdapter  abilityAdapter;

        private IGameAssets             gameAssets;
        private EvolutionInfoBlueprint  evolutionInfoBlueprint;
        private DiContainer             diContainer;
        private HeroLocalDataController heroLocalDataController;

        [Inject]
        public void Construct(IGameAssets gameAssets,
            EvolutionInfoBlueprint evolutionInfoBlueprint,
            DiContainer diContainer,
            HeroLocalDataController heroLocalDataController)
        {
            this.gameAssets              = gameAssets;
            this.evolutionInfoBlueprint  = evolutionInfoBlueprint;
            this.diContainer             = diContainer;
            this.heroLocalDataController = heroLocalDataController;
        }

        public void BindData(ElementGenericInfoModel model)
        {
            var heroRuntimeData = this.heroLocalDataController.GetHeroRuntimeData(model.ElementId);
            this.attackInfoTxt.text      = $"{heroRuntimeData.attack}";
            this.attackInfoSpeedTxt.text = $"{heroRuntimeData.attackSpeed}";

            var skeletonDataAsset = this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(heroRuntimeData.heroRecord.SkeletonDataAsset).WaitForCompletion();
            this.avatarAnim.ChangeSkeletonDataAsset(skeletonDataAsset, "idle");

            this.InitAdapter(model).Forget();
        }

        private async UniTaskVoid InitAdapter(ElementGenericInfoModel model)
        {
            var abilityRecords = this.evolutionInfoBlueprint.GetDataById(model.EvolutionId).AbilityRecords;
            this.selectedAbilityId = abilityRecords.First().Key;
            var modelList      = new List<AbilityUIModel>();
            foreach (var (key, record) in abilityRecords)
            {
                var abilityUIModel = new AbilityUIModel()
                {
                    Id         = key,
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
            var abilityRecords = this.evolutionInfoBlueprint.GetDataById(ElementId).AbilityRecords;
            var description    = abilityRecords[this.selectedAbilityId].AbilityDescription;
            this.skillDescription.text = description;
        }
    }

    public class ElementGenericInfoModel
    {
        public string ElementId;
        public string EvolutionId;
        public string SkeletonDataAsset;
    }
}