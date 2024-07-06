namespace Models.Tags
{
    public class BleedTag : IElementTag
    {
        public Tag   ElementTag => Tag.BleedAffect;
        public float Duration;
        public float TimeDelay;
        public float Timer;
    }
}