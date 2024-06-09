namespace Models.LocalData
{
    using Models.LocalData.LocalDataController;

    public class LevelLocalData : ILocalDataHaveController<LevelLocalDataController>
    {
        public int CurrentLevel = 1;
        public int CurrentWave  = 1;
        public void Init()
        {
            this.CurrentLevel = 1;
            this.CurrentWave  = 1;
        }
    }
}