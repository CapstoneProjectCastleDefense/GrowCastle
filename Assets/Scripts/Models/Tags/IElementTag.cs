namespace Models.Tags
{
    public interface IElementTag
    {
        public Tag ElementTag { get; }
    }

    public enum Tag
    {
        Enemy,
        Hero,
        Tower,
        Castle,
        Boss,
        Summoner,
        BleedAffect,
        StunEffect,
        FearEffect,
    }
}