namespace TranslitSharp
{
    public class TransliterationOptions
    {
        public TransliterationOptions(CaseConversion caseConversion = CaseConversion.None)
        {
            CaseConversion = caseConversion;
        }

        public CaseConversion CaseConversion { get; }
    }
}
