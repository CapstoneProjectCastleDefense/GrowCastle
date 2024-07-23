namespace Runtime.Scenes.Commons
{
    using System.Collections.Generic;
    using Models;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class EvolveItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text    priceTxt;
        [SerializeField] private Button      selectBtn;
        [SerializeField] private Image       itemImg;
        [SerializeField] private List<Image> pathFromParents;

        public void BindData(EvolveItemUIModel param) { this.SetParentPath(param.EvolutionDetailRecord.ParentPathIndex); }

        private void SetParentPath(int parentPathIndex)
        {
            for (var i = 0; i < this.pathFromParents.Count; i++)
            {
                var pathFromParent = this.pathFromParents[i];
                pathFromParent.gameObject.SetActive(i == parentPathIndex);
            }
        }
    }

    public class EvolveItemUIModel
    {
        public Vector3               LevelPos;
        public List<GameObject>      LinePos;
        public EvolutionDetailRecord EvolutionDetailRecord;
    }
}