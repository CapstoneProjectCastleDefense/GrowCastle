namespace Runtime.Elements.Entities.Camera
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera  camera;
        public  Vector3 startPos;

        private void Awake()
        {
            this.camera = this.GetComponent<Camera>();
        }

        private void Start()
        {

            this.camera.transform.position = this.startPos;
        }
    }
}