namespace TranslitSharp
{
    public class GreekMpTransliterationTokenHandler : TransliterationTokenHandler
    {
        public static new GreekMpTransliterationTokenHandler Instance { get; } = new GreekMpTransliterationTokenHandler();

        public override string Handle(TransliterationContext context)
        {
            if (context.IsStartOfWord() || context.IsEndOfWord())
                return context.CurrentTokenCase == TokenCase.Lower ? "b" : "B";
            else
                return base.Handle(context);
        }
    }
}
