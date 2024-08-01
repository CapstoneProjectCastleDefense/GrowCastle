namespace Helpers
{
    using UnityEngine;

    public class ObjectAutoDestroy : MonoBehaviour
    {
        public  bool  isActive;
        public  float timeExist;
        private float timer;

        public void StartAutoDestroy(float time)
        {
            this.isActive  = true;
            this.timer     = 0;
            this.timeExist = time;
        }
        private void Start() { this.timer = 0; }
        private void Update()
        {
            if (!this.isActive) return;
            if (this.timer >= this.timeExist)
            {
                Destroy(this.gameObject);
                return;
            }

            this.timer += Time.deltaTime;
        }
    }
}