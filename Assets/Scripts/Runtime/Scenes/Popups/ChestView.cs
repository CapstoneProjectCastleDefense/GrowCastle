namespace Runtime.Scenes.Popups
{
    using Models.Blueprints;
    using Runtime.Enums;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChestView : MonoBehaviour
    {
        public ResourceType    chestType;
        public Button          chestButton;
        public Image           chestIcon;
        public TextMeshProUGUI chestNumber;
    }
}