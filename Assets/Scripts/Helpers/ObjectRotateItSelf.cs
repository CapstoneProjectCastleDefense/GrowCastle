namespace Helpers
{
    using DG.Tweening;
    using UnityEngine;

    public class ObjectRotateItSelf : MonoBehaviour
    {
        public Vector3 rotationAmount = new(0, 360, 0); 
        public float   duration       = 0.2f; 
        private void Start()
        {
            DOTween.Kill(this.transform);
        }
        public void StatRotate()
        {
            DOTween.Kill(this.transform);
            this.transform.DORotate(this.rotationAmount, this.duration, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental); 
        }
    }
}