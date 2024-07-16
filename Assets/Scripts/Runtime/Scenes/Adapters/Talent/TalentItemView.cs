namespace Runtime.Scenes.Adapters.Talent
{
    using System;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TalentItemModel
    {
        public TalentType              TalentType;
        public Action<TalentType, int> OnItemClickAction;
    }

    public class TalentItemView : TViewMono
    {
        public Button          talentBtn;
        public Image           talentIcon;
        public TextMeshProUGUI level;
    }

    public class TalentItemPresenter : BaseUIItemPresenter<TalentItemView, TalentItemModel>
    {
        private readonly TalentBlueprint           talentBlueprint;
        private readonly TalentLocalDataController talentLocalDataController;
        private          TalentItemModel           model;
        public TalentItemPresenter(IGameAssets gameAssets, TalentBlueprint talentBlueprint, TalentLocalDataController talentLocalDataController)
            : base(gameAssets)
        {
            this.talentBlueprint           = talentBlueprint;
            this.talentLocalDataController = talentLocalDataController;
        }
        public override void BindData(TalentItemModel param)
        {
            this.model = param;
            var talentLevel = this.talentLocalDataController.GetTalentLevel(this.model.TalentType);
            var talentData  = this.talentBlueprint.GetDataById(this.model.TalentType);
            this.View.talentIcon.sprite = this.GameAssets.LoadAssetAsync<Sprite>(talentData.Icon).WaitForCompletion();
            this.View.level.text        = talentLevel == 0 ? "" : $"{talentData.TalentLevelToDataRecords[talentLevel].Level}";
            this.View.talentBtn.onClick.RemoveAllListeners();
            this.View.talentBtn.onClick.AddListener(this.OnItemClick);
        }
        private void OnItemClick()
        {
            var talentLevel = this.talentLocalDataController.GetTalentLevel(this.model.TalentType);
            this.model.OnItemClickAction?.Invoke(this.model.TalentType, talentLevel + 1);
        }
    }
}