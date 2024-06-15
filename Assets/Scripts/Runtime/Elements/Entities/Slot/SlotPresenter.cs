namespace Runtime.Elements.Entities.Slot
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Extensions;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using UnityEngine;

    public class SlotModel : IElementModel
    {
        public string Id              { get; set; }
        public string AddressableName { get; set; }

        public SlotRecord SlotRecord { get; set; }
    }

    public class SlotPresenter : BaseElementPresenter<SlotModel, SlotView, SlotPresenter>
    {
        private readonly IGameAssets             gameAssets;
        private readonly SlotLocalDataController slotLocalDataController;
        public           SlotManager             slotManager;
        public SlotPresenter(SlotModel model, ObjectPoolManager objectPoolManager, IGameAssets gameAssets, SlotLocalDataController slotLocalDataController)
            : base(model, objectPoolManager)
        {
            this.gameAssets              = gameAssets;
            this.slotLocalDataController = slotLocalDataController;
        }

        public SlotView GetSlotView => this.View;

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.image.sprite       = this.gameAssets.LoadAssetAsync<Sprite>(this.Model.SlotRecord.Image).WaitForCompletion();
            this.View.transform.position = this.Model.SlotRecord.Position;
            this.View.OnMouseClick       = this.OnClick;
        }
        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public void OnClick()
        {
            this.slotManager.UpdateCurrentSelectedSlot(this);
            Debug.Log("Click slot!!");
        }

        public void LoadHero(IHeroPresenter heroPresenter)
        {
            //using hero
        }

        public void DeActiveView()
        {
            this.View.image.DOFade(0, 0.1f);
            this.View.image.GetComponent<BoxCollider2D>().enabled = false;
        }

        public void UnLoadHero() { }

        public void ShowInventory() { }


        public override void Dispose() { }
    }
}