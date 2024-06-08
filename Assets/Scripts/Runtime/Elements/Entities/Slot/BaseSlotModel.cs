using Runtime.Elements.Base;
using Runtime.Interfaces.Entities;

namespace Assets.Scripts.Runtime.Elements.Entities.Slot
{
    public abstract class BaseSlotModel : IElementModel, IHaveEffect
    {
        public string Id { get; set; }
        public string AddressableName { get; set; }
        public SlotEffect SlotEffect { get; set; }

    }

    public class SlotEffect
    {
    }

}
