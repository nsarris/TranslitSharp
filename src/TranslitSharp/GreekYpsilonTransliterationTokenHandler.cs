using System.Collections.Generic;

namespace TranslitSharp
{
    public class GreekYpsilonTransliterationTokenHandler : TransliterationTokenHandler
    {
        public static new GreekYpsilonTransliterationTokenHandler Instance { get; } = new GreekYpsilonTransliterationTokenHandler();

        private static readonly HashSet<char> ypsilon_preceeding = new HashSet<char> { 'Α', 'α', 'Ε', 'ε', 'Η', 'η' };
        private static readonly HashSet<char> ypsilon_trailing_to_f = new HashSet<char> { 'Θ', 'θ', 'Κ', 'κ', 'Ξ', 'ξ', 'Π', 'π', 'Σ', 'σ', 'Τ', 'τ', 'Φ', 'φ', 'Χ', 'χ', 'Ψ', 'ψ', 'Υ', 'Ύ', 'Ϋ', 'υ', 'ύ', 'ϋ', 'ΰ' };

        public override string Handle(TransliterationContext context)
        {
            if (!context.IsFirstToken && ypsilon_preceeding.Contains(context.CurrentText[context.CurrentTokenIndex - 1]))
            {
                if (context.IsLastToken || ypsilon_trailing_to_f.Contains(context.CurrentText[context.NextTokenIndex]))
                    return context.CurrentTokenCase == TokenCase.Lower ? "f" : "F";
                else
                    return context.CurrentTokenCase == TokenCase.Lower ? "v" : "V";
            }
            else
                return context.ReplacementToken.GetTransliterated(context.CurrentTokenCase);
        }
    }
}
