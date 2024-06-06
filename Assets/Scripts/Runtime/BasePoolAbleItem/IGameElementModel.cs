namespace Runtime.BasePoolAbleItem
{
    using Runtime.Elements.Base;

    public interface IGameElementModel : IIdentifier
    {
        public string AddressableName { get; set; }
    }
}