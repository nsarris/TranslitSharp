namespace TranslitSharp
{
    public static partial class TransliteratorConfigurationExtensions
    {
        public static TransliteratorConfiguration ConfigureGreekToLatin(this TransliteratorConfiguration configuration)
            => configuration
            .AddCharacterMaps(
                ("Α", "A"),
                ("Ά", "A"),
                ("Β", "V"),
                ("Γ", "G"),
                ("Δ", "D"),
                ("Ε", "E"),
                ("Έ", "E"),
                ("Ζ", "Z"),
                ("Η", "I"),
                ("Ή", "I"),
                ("Θ", "TH"),
                ("Ι", "I"),
                ("Ϊ", "I"),
                ("Ί", "I"),
                ("Κ", "K"),
                ("Λ", "L"),
                ("Μ", "M"),
                ("Ν", "N"),
                ("Ξ", "X"),
                ("Ο", "O"),
                ("Ό", "O"),
                ("Π", "P"),
                ("Ρ", "R"),
                ("Σ", "S"),
                ("Τ", "T"),
                ("Φ", "F"),
                ("Χ", "CH"),
                ("Ψ", "PS"),
                ("Ω", "O"),
                ("Ώ", "O"),
                ("ΟΥ", "OU"),
                ("ΟΎ", "OU"),
                ("ΌΎ", "OU"),
                ("ΌΥ", "OU"),
                ("ΓΓ", "NG"),
                ("ΓΧ", "NCH"),
                ("ΓΞ", "NX"))
            .AddCharacterMaps(
                ("ΐ", "i", map => map.AddAllCasePermutations = false),
                ("ς", "s", map => map.AddAllCasePermutations = false),
                ("Υ", "Y", map => map.Handler = GreekYpsilonTransliterationTokenHandler.Instance),
                ("Ύ", "Y", map => map.Handler = GreekYpsilonTransliterationTokenHandler.Instance),
                ("Ϋ", "Y", map => map.Handler = GreekYpsilonTransliterationTokenHandler.Instance),
                ("ΰ", "y", map => { map.Handler = GreekYpsilonTransliterationTokenHandler.Instance; map.AddAllCasePermutations = false; }),
                ("ΜΠ", "MP", map => map.Handler = GreekMpTransliterationTokenHandler.Instance));
    }
}
