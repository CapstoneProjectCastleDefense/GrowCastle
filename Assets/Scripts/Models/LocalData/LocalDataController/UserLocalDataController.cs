namespace Models.LocalData.LocalDataController
{
    using R3;
    using Runtime.Signals.Quests;
    using Runtime.StaticValues;
    using Zenject;

    public class UserLocalDataController : ILocalDataController
    {
        public           bool          IsWinCurrentLevel;
        private readonly UserLocalData userLocalData;
        private readonly SignalBus     signalBus;

        public UserLocalDataController(UserLocalData userLocalData, SignalBus signalBus)
        {
            this.userLocalData = userLocalData;
            this.signalBus     = signalBus;
        }
        public ReactiveProperty<float> GetCurrentUserLevel => this.userLocalData.CurrentUserLevel;

        public void UpgradeUserLevel()
        {
            this.userLocalData.CurrentUserLevel.Value++;
            this.signalBus.Fire(new QuestTriggerSignal(){TriggerSignalId = QuestTriggerSignalId.ReachLevel, Value = this.GetCurrentUserLevel.Value, isReset = true});

        }

        public void InitData()
        {

        }
    }
}