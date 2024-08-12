namespace Runtime.Extensions
{
    using UnityEngine;

    public class CameraAutoFit : MonoBehaviour
    {
        public float targetWidth  = 16.0f;  // Your target width in units (e.g., for a 9:16 aspect ratio, set 9)
        public float targetHeight = 9.0f; // Your target height in units (e.g., for a 9:16 aspect ratio, set 16)

        // Values between 0 and 1 for horizontal and vertical position relative to the screen
        [Range(0f, 1f)] public float horizontalPosition = 0.5f; // 0.5f for center, 0 for left, 1 for right
        [Range(0f, 1f)] public float verticalPosition   = 0f;   // 0.5f for center, 0 for bottom, 1 for top

        void Start()
        {
            // Calculate the desired aspect ratio
            float targetAspect = targetWidth / targetHeight;

            // Get the current screen's aspect ratio
            float screenAspect = (float)Screen.width / (float)Screen.height;

            // Calculate the difference between the target aspect and the actual aspect
            float differenceInSize = targetAspect / screenAspect;

            // Set the camera's orthographic size based on the target height and aspect ratio difference
            Camera.main.orthographicSize = targetHeight / 2.0f * differenceInSize;

            // Calculate the camera's size in world units
            float cameraHeight = Camera.main.orthographicSize * 2;
            float cameraWidth  = cameraHeight * screenAspect;

            // Calculate the position of the camera based on the specified screen position
            float cameraPosX = cameraWidth * horizontalPosition - cameraWidth / 2.0f;
            float cameraPosY = cameraHeight * verticalPosition - cameraHeight / 2.0f;

            // Position the camera at the calculated position
            Camera.main.transform.position = new Vector3(cameraPosX, cameraPosY, Camera.main.transform.position.z);
        }
    }
}