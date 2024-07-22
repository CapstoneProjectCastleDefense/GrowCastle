namespace Models.LocalData.LocalDataController
{
    using R3;

    public class UserLocalDataController : ILocalDataController
    {
        public           bool          IsWinCurrentLevel;
        private readonly UserLocalData userLocalData;
        public UserLocalDataController(UserLocalData userLocalData) { this.userLocalData = userLocalData; }
        public ReactiveProperty<float> GetCurrentUserLevel => this.userLocalData.CurrentUserLevel;

        public void UpgradeUserLevel() { this.userLocalData.CurrentUserLevel.Value++; }
        public void InitData()         { }
    }
}