namespace FunctionBase.ScreenManager
{
    using UnityEngine;

    public class RootUICanvas : MonoBehaviour
    {
        [field: SerializeField] public Transform RootUIShowTransform    { get; private set; }
        [field: SerializeField] public Transform RootUIClosedTransform  { get; private set; }
        [field: SerializeField] public Transform RootUIOverlayTransform { get; private set; }

        [field: SerializeField] public Camera UICamera { get; private set; }
        
        private void Awake()
        {
            this.RootUIShowTransform    ??= this.transform;
            this.RootUIClosedTransform  ??= this.transform;
            this.RootUIOverlayTransform ??= this.transform;
        }
    }
}