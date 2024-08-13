namespace Models.LocalData
{
    using Models.LocalData.LocalDataController;
    using R3;

    public class LevelLocalData : ILocalDataHaveController<LevelLocalDataController>
    {
        public ReactiveProperty<int> CurrentLevel { get; set; } = new(1);
        public ReactiveProperty<int> CurrentWave  { get; set; } = new(1);
        public int                   EnemyLevel   { get; set; } = 1;

        public void Init()
        {
        }
    }
}