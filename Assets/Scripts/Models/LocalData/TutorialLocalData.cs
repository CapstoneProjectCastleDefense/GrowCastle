namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class TutorialLocalData : ILocalDataHaveController<TutorialLocalDataController>
    {
        public Dictionary<string, TutorialData> tutorialData = new();
        public void Init()
        {

        }
    }

    public class TutorialData
    {
        public string TutId  { get; set; }
        public bool   IsDone { get; set; }
    }

}