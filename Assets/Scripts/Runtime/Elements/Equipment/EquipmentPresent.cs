namespace Runtime.Elements.Equipment
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Items;
    using UnityEngine;

    public class EquipmentPresent : BaseElementPresenter<EquipmentModel, EquipmentView, EquipmentPresent>, IEquipment
    {
        private readonly EquipmentBlueprint equipmentBlueprint;
        public EquipmentPresent(EquipmentModel model, ObjectPoolManager objectPoolManager, EquipmentBlueprint equipmentBlueprint) : base(model, objectPoolManager)
        {
            this.equipmentBlueprint = equipmentBlueprint;
        }
        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.equipmentBlueprint.GetDataById(this.Model.Id).PrefabName); }
        public override    void Dispose() { }
        public             EquipmentType EquipmentType => this.Model.EquipmentType;
        public             ItemType ItemType => ItemType.Equipment;
        public             void OnEquip(IHaveStatsModel target) { target.Plus(this.Model); }
        public             void OnUnEquip(IHaveStatsModel target) { target.Minus(this.Model); }
        public             Dictionary<StatEnum, (Type, object)> GetStats() { throw new NotImplementedException(); }
        public             void UpdateStats() { throw new NotImplementedException(); }
    }
}