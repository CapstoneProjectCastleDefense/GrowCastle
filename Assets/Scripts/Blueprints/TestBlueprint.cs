namespace Blueprints
{
    using FunctionBase.BlueprintManager.BlueprintBase;
    using UnityFoundation.Scripts.BlueprintManager;

    [DataInfo("Test")] public class TestBlueprint : BlueprintDataCsv<int, TestRecord>
    {
    }

    public class TestRecord
    {
        [KeyOfRecord] public int                              Id;
        public               string                           Name;
        public               int                              Age;
        public               BlueprintByRow<int, LevelRecord> LevelRecords = new();
    }

    public class LevelRecord
    {
        [KeyOfRecord] public int Level;
        public               int Damage;
        public               int Health;
    }
}