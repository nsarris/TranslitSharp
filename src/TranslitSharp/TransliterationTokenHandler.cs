namespace TranslitSharp
{
    public class TransliterationTokenHandler
    {
        public static TransliterationTokenHandler Instance { get; } = new TransliterationTokenHandler();
        public virtual string Handle(TransliterationContext context)
        {
            return context.ReplacementToken.GetTransliterated(context.CurrentTokenCase);
        }
    }
}
