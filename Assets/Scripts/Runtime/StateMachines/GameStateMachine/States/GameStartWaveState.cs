namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.Systems;

    public class GameStartWaveState : BaseGameState
    {
        private readonly WaveManageSystem waveManageSystem;
        public GameStartWaveState(WaveManageSystem waveManageSystem) { this.waveManageSystem = waveManageSystem; }
        public override void Enter()
        {
            this.waveManageSystem.StartCurrentWave();
        }

        public override void Exit()
        {

        }
    }
}