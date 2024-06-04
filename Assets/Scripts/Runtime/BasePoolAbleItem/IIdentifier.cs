namespace Runtime.BasePoolAbleItem
{
    public interface IIdentifier
    {
        public string Id { get; set; }
    }

    public interface IPoolAbleItemModel : IIdentifier
    {
        public string AddressableName { get; set; }
    }
}