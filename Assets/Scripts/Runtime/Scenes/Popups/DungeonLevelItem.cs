namespace Runtime.Scenes.Popups
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DungeonLevelItem : MonoBehaviour
    {
        public string          dungeonId;
        public TextMeshProUGUI dungeonIdText;
        public Button          dungeonSelectBtn;
        public Action<string>  onDungeonSelectBtnClick;

        private void Awake()
        {
            this.dungeonSelectBtn.onClick.AddListener(()=>{this.onDungeonSelectBtnClick?.Invoke(this.dungeonId);});
        }
    }
}