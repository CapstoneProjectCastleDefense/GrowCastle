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
    using System;
    using System.Linq;
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
        private readonly CastleManager           castleManager;
        public           SlotManager             slotManager;
        public SlotPresenter(SlotModel model, ObjectPoolManager objectPoolManager, IGameAssets gameAssets, SlotLocalDataController slotLocalDataController, CastleManager castleManager)
            : base(model, objectPoolManager)
        {
            this.gameAssets              = gameAssets;
            this.slotLocalDataController = slotLocalDataController;
            this.castleManager           = castleManager;
        }

        public SlotView GetSlotView => this.View;

        public override UniTask UpdateView()
        {
            this.View                    = this.castleManager.entities.First().CastleView.listSlotView.First(e => e.id == this.Model.Id);
            this.IsViewInit              = true;
            this.View.image.sprite       = this.gameAssets.LoadAssetAsync<Sprite>(this.Model.SlotRecord.Image).WaitForCompletion();
            this.View.transform.position = this.Model.SlotRecord.Position;
            this.View.OnMouseClick       = this.OnClick;
            this.UpdateSlotBaseOnCurrentLevel();
            return UniTask.CompletedTask;
        }

        public void UpdateSlotBaseOnCurrentLevel()
        {
            if (this.slotLocalDataController.GetSlotData(this.Model.SlotRecord.Id).IsUnlock)
            {
                this.View.gameObject.SetActive(true);
            }
            else
            {
                this.View.gameObject.SetActive(false);
            }
        }

        protected override UniTask<GameObject> CreateView() { return new UniTask<GameObject>(); }

        public void OnClick()
        {
            this.slotManager.UpdateCurrentSelectedSlot(this);
            Debug.Log("Click slot " + this.Model.Id + "!!");
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