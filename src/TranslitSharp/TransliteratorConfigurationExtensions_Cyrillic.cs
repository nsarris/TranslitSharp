﻿namespace TranslitSharp
{
    public static partial class TransliteratorConfigurationExtensions
    {
        public static TransliteratorConfiguration ConfigureCyrillicToLatin(this TransliteratorConfiguration configuration)
            => configuration
            .AddCharacterMap("А", "A")
            .AddCharacterMap("Б", "B")
            .AddCharacterMap("В", "V")
            .AddCharacterMap("Г", "G")
            .AddCharacterMap("Ґ", "G")
            .AddCharacterMap("Д", "D")
            .AddCharacterMap("Ѓ", "G")
            .AddCharacterMap("Ђ", "D")
            .AddCharacterMap("Е", "E")
            .AddCharacterMap("Ё", "E")
            .AddCharacterMap("Є", "E")
            .AddCharacterMap("Ж", "Z")
            .AddCharacterMap("З", "Z")
            .AddCharacterMap("Ѕ", "Z")
            .AddCharacterMap("И", "I")
            .AddCharacterMap("I", "I")
            .AddCharacterMap("Ї", "I")
            .AddCharacterMap("Й", "J")
            .AddCharacterMap("Ј", "J")
            .AddCharacterMap("К", "K")
            .AddCharacterMap("Л", "L")
            .AddCharacterMap("Љ", "L")
            .AddCharacterMap("М", "M")
            .AddCharacterMap("Н", "N")
            .AddCharacterMap("Њ", "N")
            .AddCharacterMap("О", "O")
            .AddCharacterMap("П", "P")
            .AddCharacterMap("Р", "R")
            .AddCharacterMap("С", "S")
            .AddCharacterMap("Т", "T")
            .AddCharacterMap("Ќ", "K")
            .AddCharacterMap("Ћ", "C")
            .AddCharacterMap("У", "U")
            .AddCharacterMap("Ў", "U")
            .AddCharacterMap("Ф", "F")
            .AddCharacterMap("Х", "h")
            .AddCharacterMap("Ц", "C")
            .AddCharacterMap("Ч", "C")
            .AddCharacterMap("Џ", "D")
            .AddCharacterMap("Ш", "S")
            .AddCharacterMap("Щ", "S")
            .AddCharacterMap("Ы", "Y")
            .AddCharacterMap("Ѣ", "E")
            .AddCharacterMap("Э", "E")
            .AddCharacterMap("Ю", "U")
            .AddCharacterMap("Я", "A")
            .AddCharacterMap("Ѫ", "A")
            .AddCharacterMap("Ѳ", "F")
            .AddCharacterMap("Ѵ", "Y")
            ;
    }
}
