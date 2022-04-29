namespace TranslitSharp
{
    public class TransliterationOptions
    {
        public TransliterationOptions(
            CaseConversion caseConversion = CaseConversion.None,
            int estimatedLength = 0)
        {
            CaseConversion = caseConversion;
            EstimatedLength = estimatedLength;
        }

        /// <summary>
        /// Used to control the case of the converted string
        /// </summary>
        public CaseConversion CaseConversion { get; }

        /// <summary>
        /// Used to setup the initial capacity of the internal string builder. The string builder doubles capacity when reaching the limit. 
        /// This might infer an large allocation waste on long strings when only a few characters are replaced with longer strings. Set to 0 or less to disable.
        /// </summary>
        public int EstimatedLength { get; }
    }
}
