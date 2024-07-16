namespace Models.LocalData
{
    using Models.LocalData.LocalDataController;
    using R3;

    public class UserLocalData : ILocalDataHaveController<UserLocalDataController>
    {
        public ReactiveProperty<float> CurrentUserLevel { get; set; } = new(1);
        public ReactiveProperty<float> Sound     { get; set; } = new(1);
        public ReactiveProperty<float> Music     { get; set; } = new(1);
        public void Init()
        {
            
        }
    }
}