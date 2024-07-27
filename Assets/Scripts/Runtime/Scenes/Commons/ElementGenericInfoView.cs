namespace Runtime.Scenes.Commons
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
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

        private SkillBlueprint         skillBlueprint;
        private IGameAssets            gameAssets;
        private EvolutionInfoBlueprint evolutionInfoBlueprint;
        private DiContainer            diContainer;

        [Inject]
        public void Construct(SkillBlueprint skillBlueprint,
            IGameAssets gameAssets,
            EvolutionInfoBlueprint evolutionInfoBlueprint,
            DiContainer diContainer)
        {
            this.skillBlueprint         = skillBlueprint;
            this.gameAssets             = gameAssets;
            this.evolutionInfoBlueprint = evolutionInfoBlueprint;
            this.diContainer            = diContainer;
        }

        public void BindData(CharacterInfoPopupModel popupModel)
        {
            this.skillDescription.text   = this.skillBlueprint.GetDataById(popupModel.HeroRuntimeData.heroRecord.ActiveSkill.skillName).Description;
            this.attackInfoTxt.text      = $"{popupModel.HeroRuntimeData.attack}";
            this.attackInfoSpeedTxt.text = $"{popupModel.HeroRuntimeData.attackSpeed}";

            var skeletonDataAsset = this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(popupModel.HeroRuntimeData.heroRecord.SkeletonDataAsset).WaitForCompletion();
            this.avatarAnim.ChangeSkeletonDataAsset(skeletonDataAsset, "idle");

            this.InitAdapter(popupModel).Forget();
        }

        private async UniTaskVoid InitAdapter(CharacterInfoPopupModel popupModel)
        {
            var abilityRecords = this.evolutionInfoBlueprint.GetDataById(ElementId).AbilityRecords;
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
}