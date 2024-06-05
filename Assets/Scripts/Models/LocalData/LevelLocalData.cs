namespace Models.LocalData
{
    using GameFoundation.Scripts.Interfaces;

    public class LevelLocalData : ILocalData
    {
        public int    CurrentLevel;
        public void Init()
        {
            this.CurrentLevel  = 1;
        }
    }
}