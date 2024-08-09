namespace Models.Blueprints
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;

    [CsvHeaderKey("Id")]
    [BlueprintReader("ElementSkinBlueprint", true)]
    public class ElementSkinBlueprint : GenericBlueprintReaderByRow<string, ElementSkinRecord>
    {
        public string GetSkinByLevel(string elementId, int level)
        {
            var levelCapToSkinId = this[elementId].LevelCapToSkinId;

            var selectCap = 0;
            foreach (var (levelCap, skinId) in levelCapToSkinId)
            {
                if (level >= levelCap)
                {
                    selectCap = levelCap;
                }
                else
                {
                    break;
                }
            }

            return levelCapToSkinId[selectCap];
        }
    }

    public class ElementSkinRecord
    {
        public string                  Id               { get; set; }
        public Dictionary<int, string> LevelCapToSkinId { get; set; }
    }
}