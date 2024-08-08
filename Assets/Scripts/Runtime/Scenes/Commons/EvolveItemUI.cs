namespace Runtime.Scenes.Commons
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Popups;
    using Runtime.Services;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class EvolveItemUI : MonoBehaviour
    {
        private ScreenManager                       screenManager;
        private HeroLocalDataController             heroLocalDataController;
        private ElementEvolutionLocalDataController elementEvolutionLocalDataController;
        private EvolutionBlueprint                  evolutionBlueprint;
        private IGameAssets                         gameAssets;
        private EvolutionInfoBlueprint              evolutionInfoBlueprint;
        private HeroUpgradeService                  heroUpgradeService;

        [Inject] public void Construct(
            ScreenManager                       screenManager,
            HeroLocalDataController             heroLocalDataController,
            ElementEvolutionLocalDataController elementEvolutionLocalDataController,
            EvolutionBlueprint                  evolutionBlueprint,
            IGameAssets                         gameAssets,
            EvolutionInfoBlueprint              evolutionInfoBlueprintInject,
            HeroUpgradeService                  heroUpgradeServiceInject
        )
        {
            this.screenManager                       = screenManager;
            this.heroLocalDataController             = heroLocalDataController;
            this.elementEvolutionLocalDataController = elementEvolutionLocalDataController;
            this.evolutionBlueprint                  = evolutionBlueprint;
            this.gameAssets                          = gameAssets;
            this.evolutionInfoBlueprint              = evolutionInfoBlueprintInject;
            this.heroUpgradeService                  = heroUpgradeServiceInject;
        }

        [SerializeField] private TMP_Text        priceTxt, levelUnlockTxt;
        [SerializeField] private Button          selectBtn;
        [SerializeField] private List<Image>     pathFromParents;
        [SerializeField] private List<Image>     greenPathFromParents;
        [SerializeField] private SkeletonGraphic elementSkeleton;
        [SerializeField] private GameObject      unlockConditions, priceCondition;

        private EvolveItemUIModel     model;
        private EvolutionDetailRecord evolutionDetailRecord;
        private Sequence              blinkTween;

        public void BindData(EvolveItemUIModel param)
        {
            this.model                 = param;
            this.evolutionDetailRecord = this.evolutionBlueprint.GetEvolutionDetailRecord(this.model.ElementId, this.model.EvolutionId);
            this.priceTxt.text         = this.evolutionInfoBlueprint[this.model.EvolutionId].Price.ToString();
            var isUnlock = this.elementEvolutionLocalDataController.IsEvolutionUnlock(this.model.ElementId, this.model.EvolutionId);
            this.priceTxt.gameObject.SetActive(!isUnlock);
            this.selectBtn.onClick.RemoveAllListeners();
            this.selectBtn.onClick.AddListener(() => this.OnClickBtnSelect(this.model));

            this.transform.localScale = Vector3.one;

            this.SetSkeleton();
            this.SetPosition();
            this.SetParentPath();
            this.SetUnlockConditions();
        }

        private void SetSkeleton()
        {
            var skeleton = this.evolutionDetailRecord.IconImg;
            this.elementSkeleton.ChangeSkeletonDataAsset(this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(skeleton).WaitForCompletion());
        }

        private void SetPosition()
        {
            var lineIndex = this.evolutionDetailRecord.LineIndex;
            var linePos   = this.model.LinePos[lineIndex].transform.position;
            var levelPos  = this.model.LevelPos;
            var pos       = new Vector3(linePos.x, levelPos.y, linePos.z);
            this.transform.position = pos;
        }

        private void SetParentPath()
        {
            var currentEvolution = this.elementEvolutionLocalDataController.GetEvolutionElementData(this.model.ElementId);

            var listPredecessor = this.evolutionBlueprint.GetPredecessorEvolveId(this.model.ElementId, currentEvolution.EvolutionId);
            listPredecessor.Add(currentEvolution.EvolutionId);
            var listChild = this.evolutionBlueprint.GetChildrenEvolutionId(this.model.ElementId, currentEvolution.EvolutionId);

            var isPredecessor = listPredecessor.Contains(this.model.EvolutionId);
            var isChild       = listChild.Contains(this.model.EvolutionId);
            var pathList      = isPredecessor || isChild ? this.greenPathFromParents : this.pathFromParents;
            var otherPathList = isPredecessor || isChild ? this.pathFromParents : this.greenPathFromParents;

            var parentPathIndex = this.evolutionDetailRecord.ParentPathIndex;
            for (var i = 0; i < pathList.Count; i++)
            {
                var pathFromParent = pathList[i];
                var isActive       = i == parentPathIndex;
                pathFromParent.gameObject.SetActive(isActive);

                if (isActive && isChild)
                {
                    this.blinkTween = DOTween.Sequence();
                    this.blinkTween
                        .Append(pathFromParent.DOFade(0, .5f))
                        .Append(pathFromParent.DOFade(1, .35f))
                        .SetLoops(-1, LoopType.Restart)
                        .onKill += () =>
                    {
                        pathFromParent.material.color = new Color(1, 1, 1, 1);
                    };
                }
            }

            otherPathList.ForEach(p => p.gameObject.SetActive(false));
        }

        private void SetUnlockConditions()
        {
            var isUnlock = this.elementEvolutionLocalDataController.IsEvolutionUnlock(this.model.ElementId, this.model.EvolutionId);
            this.unlockConditions.gameObject.SetActive(!isUnlock);

            if (isUnlock) return;

            var requireLevel        = this.evolutionInfoBlueprint.GetDataById(this.model.EvolutionId).RequireLevel;
            var isReachRequireLevel = this.heroUpgradeService.GetHeroLevel(this.model.ElementId) >= requireLevel;
            this.levelUnlockTxt.gameObject.SetActive(!isReachRequireLevel);
            this.priceCondition.gameObject.SetActive(isReachRequireLevel);
            if (!isReachRequireLevel)
            {
                this.levelUnlockTxt.text = $"Lv<color=\"green\">{requireLevel}";
            }
            else
            {
                this.priceTxt.text = $"{this.evolutionInfoBlueprint.GetDataById(this.model.EvolutionId).Price}";
            }
        }

        private void OnClickBtnSelect(EvolveItemUIModel param)
        {
            var evolutionData          = this.elementEvolutionLocalDataController.GetEvolutionElementData(param.ElementId);
            var currentSelectEvolution = evolutionData.EvolutionId;

            var heroRuntimeData         = this.heroLocalDataController.GetHeroRuntimeData(param.ElementId);
            var characterInfoPopupModel = new CharacterInfoPopupModel(SlotType.Hero, heroRuntimeData, null, true);

            if (this.evolutionDetailRecord.EvolutionId == currentSelectEvolution)
            {
                this.screenManager.OpenScreen<CharacterInfoPopupPresenter, CharacterInfoPopupModel>(characterInfoPopupModel).Forget();
            }

            else
            {
                this.screenManager.OpenScreen<ConfirmEvolutionPopupPresenter, ConfirmEvolutionPopupModel>(new ConfirmEvolutionPopupModel()
                {
                    CharacterInfoPopupModel = characterInfoPopupModel,
                    ElementId               = param.ElementId,
                    EvolutionId             = this.evolutionDetailRecord.EvolutionId
                }).Forget();
            }
        }

        public void Dispose()
        {
            this.Recycle();
            this.blinkTween.Kill();
        }
    }

    public class EvolveItemUIModel
    {
        public string           ElementId;
        public string           EvolutionId;
        public Vector3          LevelPos;
        public List<GameObject> LinePos;
    }
}