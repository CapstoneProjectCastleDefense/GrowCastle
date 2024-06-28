namespace Runtime.Elements.Entities.Slot
{
    using System;
    using Runtime.Elements.Base;
    using UnityEngine;

    public class SlotView : BaseElementView
    {
        public  string         id;
        public  SpriteRenderer image;
        public  Transform      heroPos;
        public  Action         OnMouseClick;
        private void           OnMouseDown() { this.OnMouseClick?.Invoke(); }
    }
}