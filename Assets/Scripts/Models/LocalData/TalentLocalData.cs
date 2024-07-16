namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class TalentLocalData : ILocalDataHaveController<TalentLocalDataController>
    {
        public Dictionary<TalentType, int> TalentData { get; set; } = new();
        public void Init()
        {
            
        }
    }

    public enum TalentType
    {
        IncreaseArcherAttack,
        IncreaseCastleHp,
    }
}