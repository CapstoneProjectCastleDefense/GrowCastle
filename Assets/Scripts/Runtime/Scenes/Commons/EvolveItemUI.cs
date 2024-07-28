namespace Runtime.Scenes.Commons
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Models;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Popups;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class EvolveItemUI : MonoBehaviour
    {
        private ScreenManager              screenManager;
        private HeroLocalDataController    heroLocalDataController;
        private ElementLocalDataController elementLocalDataController;

        [Inject]
        public void Construct(ScreenManager screenManager,
            HeroLocalDataController heroLocalDataController,
            ElementLocalDataController elementLocalDataController
        )
        {
            this.screenManager              = screenManager;
            this.heroLocalDataController    = heroLocalDataController;
            this.elementLocalDataController = elementLocalDataController;
        }

        [SerializeField] private TMP_Text    priceTxt;
        [SerializeField] private Button      selectBtn;
        [SerializeField] private Image       itemImg;
        [SerializeField] private List<Image> pathFromParents;

        public void BindData(EvolveItemUIModel param)
        {
            this.priceTxt.text = param.EvolutionDetailRecord.Price.ToString();

            this.selectBtn.onClick.RemoveAllListeners();
            this.selectBtn.onClick.AddListener(() => this.OnClickBtnSelect(param));

            this.transform.localScale = Vector3.one;

            this.SetPosition(param);
            this.SetParentPath(param.EvolutionDetailRecord.ParentPathIndex);
        }

        private void SetPosition(EvolveItemUIModel param)
        {
            var lineIndex = param.EvolutionDetailRecord.LineIndex;
            var linePos   = param.LinePos[lineIndex].transform.position;
            var levelPos  = param.LevelPos;
            var pos       = new Vector3(linePos.x, levelPos.y, linePos.z);
            this.transform.position = pos;
        }

        private void SetParentPath(int parentPathIndex)
        {
            for (var i = 0; i < this.pathFromParents.Count; i++)
            {
                var pathFromParent = this.pathFromParents[i];
                pathFromParent.gameObject.SetActive(i == parentPathIndex);
            }
        }

        private void OnClickBtnSelect(EvolveItemUIModel param)
        {
            var evolutionData          = this.elementLocalDataController.GetEvolutionElementData(param.ElementId);
            var currentSelectEvolution = evolutionData.EvolutionId;

            var heroRuntimeData         = this.heroLocalDataController.GetHeroRuntimeData(param.ElementId);
            var characterInfoPopupModel = new CharacterInfoPopupModel(SlotType.Hero, heroRuntimeData, null, true);

            if (param.EvolutionDetailRecord.EvolutionId == currentSelectEvolution)
            {
                this.screenManager.OpenScreen<CharacterInfoPopupPresenter, CharacterInfoPopupModel>(characterInfoPopupModel).Forget();
            }

            else
            {
                this.screenManager.OpenScreen<ConfirmEvolutionPopupPresenter, ConfirmEvolutionPopupModel>(new ConfirmEvolutionPopupModel()
                {
                    CharacterInfoPopupModel = characterInfoPopupModel,
                    ElementId               = param.ElementId,
                    EvolutionId             = param.EvolutionDetailRecord.EvolutionId
                }).Forget();
            }
        }
    }

    public class EvolveItemUIModel
    {
        public string                ElementId;
        public Vector3               LevelPos;
        public List<GameObject>      LinePos;
        public EvolutionDetailRecord EvolutionDetailRecord;
    }
}