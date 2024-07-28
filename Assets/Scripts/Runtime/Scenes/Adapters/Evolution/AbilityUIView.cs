namespace Runtime.Scenes.Adapters.Evolution
{
    using System;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using UnityEngine;
    using UnityEngine.UI;

    public class AbilityUIView : TViewMono
    {
        [SerializeField] private Button selectBtn;

        public Button SelectBtn => this.selectBtn;
    }

    public class AbilityUIPresenter : BaseUIItemPresenter<AbilityUIView, AbilityUIModel>
    {
        public AbilityUIPresenter(IGameAssets gameAssets) : base(gameAssets) { }
        public override void BindData(AbilityUIModel param)
        {
            this.View.SelectBtn.onClick.RemoveAllListeners();
            this.View.SelectBtn.onClick.AddListener(() => this.OnSelected(param));
        }

        private void OnSelected(AbilityUIModel param)
        {
            param.OnSelected?.Invoke(param.Id);
        }
    }

    public class AbilityUIModel
    {
        public string         Id         { get; set; }
        public string         SelectedId { get; set; }
        public Action<string> OnSelected { get; set; }
    }
}