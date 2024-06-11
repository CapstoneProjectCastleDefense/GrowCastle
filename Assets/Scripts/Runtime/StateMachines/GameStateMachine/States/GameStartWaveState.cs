namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.Systems;
    using Runtime.Systems.Waves;

    public class GameStartWaveState : BaseGameState
    {
        private readonly WaveSystem waveSystem;
        public GameStartWaveState(WaveSystem waveSystem) { this.waveSystem = waveSystem; }
        public override void Enter()
        {
            this.waveSystem.StartCurrentWave(1);
        }

        public override void Exit()
        {

        }
    }
}