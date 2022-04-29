namespace TranslitSharp
{
    public static partial class TransliteratorConfigurationExtensions
    {
        private static TransliteratorConfiguration ConfigureCyrillicToLatinBase(this TransliteratorConfiguration configuration)
            => configuration
            .AddCharacterMaps(
                ("А", "A"),
                ("Б", "B"),
                ("В", "V"),
                ("Г", "G"),
                ("Д", "D"),
                ("Е", "E"),
                ("И", "I"),
                ("Й", "J"),
                ("К", "K"),
                ("Л", "L"),
                ("М", "M"),
                ("Н", "N"),
                ("О", "O"),
                ("П", "P"),
                ("Р", "R"),
                ("С", "S"),
                ("Т", "T"),
                ("У", "U"),
                ("Ф", "F"),
                ("Х", "H"),
                ("Ц", "C"),
                ("Ы", "Y"));


        public static TransliteratorConfiguration ConfigureCyrillicToLatin(this TransliteratorConfiguration configuration)
            => configuration
            .ConfigureCyrillicToLatinBase()
            .AddCharacterMaps(
                ("Ґ", "G"),
                ("Ѓ", "G"),
                ("Ђ", "D"),
                ("Ё", "E"),
                ("Є", "E"),
                ("Ж", "Z"),
                ("З", "Z"),
                ("Ѕ", "Z"),
                ("I", "I"),
                ("Ї", "I"),
                ("Ј", "J"),
                ("Љ", "L"),
                ("Њ", "N"),
                ("Ќ", "K"),
                ("Ћ", "C"),
                ("Ў", "U"),
                ("Ч", "C"),
                ("Џ", "D"),
                ("Ш", "S"),
                ("Щ", "S"),
                ("Ѣ", "E"),
                ("Э", "E"),
                ("Ю", "U"),
                ("Я", "A"),
                ("Ѫ", "A"),
                ("Ѳ", "F"),
                ("Ѵ", "Y"));

        public static TransliteratorConfiguration ConfigureCyrillicToUnicode(this TransliteratorConfiguration configuration)
            => configuration
            .ConfigureCyrillicToLatinBase()
            .AddCharacterMaps(
                ("Ґ", "G̀"),
                ("Ѓ", "Ǵ"),
                ("Ђ", "Đ"),
                ("Ё", "Ë"),
                ("Є", "Ê"),
                ("Ж", "Ž"),
                ("З", "Z"),
                ("Ѕ", "Ẑ"),
                ("I", "Ì"),
                ("Ї", "Ï"),
                ("Ј", "J̌"),
                ("Љ", "L̂"),
                ("Њ", "N̂"),
                ("Ќ", "Ḱ"),
                ("Ћ", "Ć"),
                ("Ў", "Ŭ"),
                ("Ч", "Č"),
                ("Џ", "D̂"),
                ("Ш", "Š"),
                ("Щ", "Ŝ"),
                ("Ѣ", "Ě"),
                ("Э", "È"),
                ("Ю", "Û"),
                ("Я", "Â"),
                ("Ѫ", "Ǎ"),
                ("Ѳ", "F̀"),
                ("Ѵ", "Ỳ"));
    }
}
