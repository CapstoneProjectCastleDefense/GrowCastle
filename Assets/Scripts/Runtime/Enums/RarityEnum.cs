namespace Runtime.Enums
{
    public enum RarityEnum
    {
        Common,
        Rare,
        Legendary,
    }

    public static class RarityToFragments
    {
        public static int GetFragments(this RarityEnum rarity)
        {
            return rarity switch
            {
                RarityEnum.Common => 2,
                RarityEnum.Rare => 4,
                RarityEnum.Legendary => 8,
                _ => 0
            };
        }
    }
}