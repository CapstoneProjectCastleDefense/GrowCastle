namespace Runtime.Services
{
    using Runtime.Signals;
    using UnityEngine;
    using Zenject;

    public class TimeCoolDownService : ITickable
    {
        private bool IsActive;

        private readonly SignalBus signalBus;

        public TimeCoolDownService(SignalBus signalBus) { this.signalBus = signalBus; }

        public void Tick()
        {
            if (!this.IsActive) return;
            this.signalBus.Fire(new TimeCooldownSignal(Time.deltaTime));
        }

        public void Resume() { this.IsActive = true; }

        public void Pause() { this.IsActive = false; }
    }
}