namespace TranslitSharp
{
    public static partial class TransliteratorConfigurationExtensions
    {        
        public static TransliteratorConfiguration ConfigureLatin1SupplementToLatin(this TransliteratorConfiguration configuration)
            => configuration
                .AddCharacterMap("À", "A")
                .AddCharacterMap("Á", "A")
                .AddCharacterMap("Â", "A")
                .AddCharacterMap("Ã", "A")
                .AddCharacterMap("Ä", "A")
                .AddCharacterMap("Å", "A")
                .AddCharacterMap("Æ", "A")
                .AddCharacterMap("Ç", "C")
                .AddCharacterMap("È", "E")
                .AddCharacterMap("É", "E")
                .AddCharacterMap("Ê", "E")
                .AddCharacterMap("Ë", "E")
                .AddCharacterMap("Ì", "I")
                .AddCharacterMap("Í", "I")
                .AddCharacterMap("Î", "I")
                .AddCharacterMap("Ï", "I")
                .AddCharacterMap("Ð", "E")
                .AddCharacterMap("Ñ", "N")
                .AddCharacterMap("Ò", "O")
                .AddCharacterMap("Ó", "O")
                .AddCharacterMap("Ô", "O")
                .AddCharacterMap("Õ", "O")
                .AddCharacterMap("Ö", "O")
                .AddCharacterMap("Ø", "O")
                .AddCharacterMap("Ù", "U")
                .AddCharacterMap("Ú", "U")
                .AddCharacterMap("Û", "U")
                .AddCharacterMap("Ü", "U")
                .AddCharacterMap("Ý", "Y")
                .AddCharacterMap("Þ", "P")
                .AddCharacterMap("ß", "S", map => map.AddAllCasePermutations = false)                
            ;
    }
}
