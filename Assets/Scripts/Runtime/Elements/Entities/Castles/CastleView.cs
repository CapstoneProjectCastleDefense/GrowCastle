namespace Runtime.Elements.Entities.Castles
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Elements.Entities.Castles.Block;

    public class CastleView : BaseCombatantView
    {
        public List<BlockView>  listBlockView  = new();
        public List<ArcherSlot> listArcherSlot = new();
    }
}