namespace Runtime.Elements.Entities.Castles
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Elements.Entities.Castles.Block;
    using UnityEngine;

    public class CastleView : BaseCombatantView
    {
        public List<BlockView>  listBlockView  = new();
        public List<ArcherSlot> listArcherSlot = new();
        public RectTransform castleUpPopUp;
        public RectTransform archerUpPopUp;

    }
}