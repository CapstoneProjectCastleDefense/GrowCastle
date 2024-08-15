namespace Runtime.Scenes.Adapters.Evolution
{
    using System;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using UnityEngine;
    using UnityEngine.UI;

    public class AbilityUIView : TViewMono
    {
        [SerializeField] private Image  bgImg,iconImg;
        [SerializeField] private Button selectBtn;


        public Image  BgImg     => this.bgImg;
        public Image  IconImg   => this.iconImg;
        public Button SelectBtn => this.selectBtn;
    }

    public class AbilityUIPresenter : BaseUIItemPresenter<AbilityUIView, AbilityUIModel>
    {
        public AbilityUIPresenter(IGameAssets gameAssets) : base(gameAssets) { }

        private AbilityUIModel model;
        public override void BindData(AbilityUIModel param)
        {
            this.model              = param;
            this.View.BgImg.enabled = this.model.Id == this.model.SelectedId;
            this.View.SelectBtn.onClick.RemoveAllListeners();
            this.View.SelectBtn.onClick.AddListener(() => this.OnSelected(this.model));
            this.View.IconImg.sprite = this.GameAssets.LoadAssetAsync<Sprite>(this.model.Icon).WaitForCompletion();
        }

        public void UpdateSelectedId(string selectedId)
        {
            this.model.SelectedId   = selectedId;
            this.View.BgImg.enabled = this.model.Id == this.model.SelectedId;
        }
        
        private void OnSelected(AbilityUIModel param)
        {
            param.OnSelected?.Invoke(param.Id);
        }
    }

    public class AbilityUIModel
    {
        public string         Id         { get; set; }
        public string         Icon       { get; set; }
        public string         SelectedId { get; set; }
        public Action<string> OnSelected { get; set; }
    }
}