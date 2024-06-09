namespace Runtime.Systems
{
    public class WaveManageSystem : IGameSystem
    {
        private int  totalEnemy;
        private bool isActiveWave;
        
        public void Initialize()
        {
            
        }
        public void Tick()
        {
            if(!this.isActiveWave) return;
            if (this.totalEnemy == 0)
            {
                this.isActiveWave = false;
                this.CompleteCurrentWave();
            }
        }

        public void StartCurrentWave()
        {
            this.isActiveWave = true;
            //start spawn enemy and count total enemy need  to complete wave
        }

        private void CompleteCurrentWave()
        {
            
        }
    }
}