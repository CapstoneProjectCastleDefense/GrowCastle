namespace Runtime.Signals
{
    public class TimeCooldownSignal
    {
        public float DeltaTime;

        public TimeCooldownSignal(float deltaTime)
        {
            this.DeltaTime = deltaTime;
        }
    }
}