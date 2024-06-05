namespace Runtime.Elements.Base
{
    public interface IIdentifier
    {
        public string Id { get; set; }
    }

    public interface IElementModel : IIdentifier
    {
        public string AddressableName { get; set; }
    }
}