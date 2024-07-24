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

        public void BindData(EvolveItemUIModel param)
        {
            this.priceTxt.text = param.EvolutionDetailRecord.Price.ToString();
            
            this.selectBtn.onClick.RemoveAllListeners();
            this.selectBtn.onClick.AddListener(() => this.OnClickBtnSelect(param));
            
            this.transform.localScale = Vector3.one;
            
            this.SetPosition(param);
            this.SetParentPath(param.EvolutionDetailRecord.ParentPathIndex);
        }

        private void SetPosition(EvolveItemUIModel param)
        {
            var lineIndex = param.EvolutionDetailRecord.LineIndex;
            var linePos = param.LinePos[lineIndex].transform.position;
            var levelPos = param.LevelPos;
            var pos = new Vector3(linePos.x, levelPos.y, linePos.z);
            this.transform.position = pos;
        }
        
        private void SetParentPath(int parentPathIndex)
        {
            for (var i = 0; i < this.pathFromParents.Count; i++)
            {
                var pathFromParent = this.pathFromParents[i];
                pathFromParent.gameObject.SetActive(i == parentPathIndex);
            }
        }

        private void OnClickBtnSelect(EvolveItemUIModel param)
        {
            
        }
    }

    public class EvolveItemUIModel
    {
        public Vector3               LevelPos;
        public List<GameObject>      LinePos;
        public EvolutionDetailRecord EvolutionDetailRecord;
    }
}