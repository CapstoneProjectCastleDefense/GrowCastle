namespace Runtime.Scenes.Commons
{
    using GameFoundation.Scripts.Utilities;
    using UnityEngine;
    using UnityEngine.UI;

    public class ScreenHasButtonSound : MonoBehaviour
    {
        private void Awake()
        {
            var buttons = this.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                button.onClick.AddListener(this.PlayButtonClickSound);
            }
        }

        private void PlayButtonClickSound() { AudioService.Instance.PlaySound("button_sfx"); }
    }
}