namespace Runtime.Interfaces
{
    public interface IAbleToLevelUp
    {
        int  Level    { get; set; }
        int  MaxLevel { get; }
        void LevelUp();
    }
}