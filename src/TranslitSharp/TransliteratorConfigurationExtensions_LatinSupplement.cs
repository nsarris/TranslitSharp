namespace TranslitSharp
{
    public static partial class TransliteratorConfigurationExtensions
    {        
        public static TransliteratorConfiguration ConfigureLatin1SupplementToLatin(this TransliteratorConfiguration configuration)
            => configuration
            .AddCharacterMaps(
                ("Á", "A"),
                ("Â", "A"),
                ("Ã", "A"),
                ("Ä", "A"),
                ("Å", "A"),
                ("Æ", "A"),
                ("Ç", "C"),
                ("È", "E"),
                ("É", "E"),
                ("Ê", "E"),
                ("Ë", "E"),
                ("Ì", "I"),
                ("Í", "I"),
                ("Î", "I"),
                ("Ï", "I"),
                ("Ð", "E"),
                ("Ñ", "N"),
                ("Ò", "O"),
                ("Ó", "O"),
                ("Ô", "O"),
                ("Õ", "O"),
                ("Ö", "O"),
                ("Ø", "O"),
                ("Ù", "U"),
                ("Ú", "U"),
                ("Û", "U"),
                ("Ü", "U"),
                ("Ý", "Y"),
                ("Þ", "P"))
            .AddCharacterMaps(
                ("ß", "S", map => map.AddAllCasePermutations = false));
    }
}
