namespace Runtime.Scenes.Popups
{
    using Models.Blueprints;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChestView : MonoBehaviour
    {
        public ChestType       chestType;
        public Button          chestButton;
        public Image           chestIcon;
        public TextMeshProUGUI chestNumber;
    }
}