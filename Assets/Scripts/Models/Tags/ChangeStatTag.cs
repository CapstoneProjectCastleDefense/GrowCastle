namespace Models.Tags
{
    public class ChangeStatTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.ChangeStat;
        public string        EffectStatId         { get; set; }
    }
}