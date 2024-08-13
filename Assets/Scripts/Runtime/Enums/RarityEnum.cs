namespace Runtime.Enums
{
    public enum RarityEnum
    {
        Common,
        Rare,
        Legendary,
    }

    public static class RarityExtension
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

        public static float GetMultiplier(this RarityEnum rarity)
        {
            return rarity switch
            {
                RarityEnum.Common => 1,
                RarityEnum.Rare => 1f,
                RarityEnum.Legendary => 1f,
                _ => 0
            };
        }
    }
}