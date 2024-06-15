namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.Managers;
    using Runtime.Systems;
    using Runtime.Systems.Waves;

    public class GameStartWaveState : BaseGameState
    {
        private readonly WaveSystem  waveSystem;
        private readonly SlotManager slotManager;
        public GameStartWaveState(WaveSystem waveSystem,SlotManager slotManager)
        {
            this.waveSystem  = waveSystem;
            this.slotManager = slotManager;
        }
        public override void Enter()
        {
            this.waveSystem.StartCurrentWave(1);
            this.slotManager.DeActiveAllSlot();
        }

        public override void Exit()
        {

        }
    }
}