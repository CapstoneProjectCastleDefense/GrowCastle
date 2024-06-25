namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Summoner", true)] [CsvHeaderKey("Id")]
    public class SummonerBlueprint : GenericBlueprintReaderByRow<string,SummonerRecord>
    {
        
    }

    public class SummonerRecord
    {
        public string Id         { get; set; }
        public string Name       { get; set; }
        public string PrefabName { get; set; }
        public string Skill      { get; set; }
    }
    //Id,Name,SummonerId,Skill
}