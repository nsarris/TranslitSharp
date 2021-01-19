using System;
using System.Globalization;

using Xunit;

namespace TranslitSharp.Tests
{
    public class CyrillicTests
    {
        [Theory]
        [InlineData("это кириллица", "eto kirillica")]
        [InlineData("Афина", "Afina")]
        [InlineData("Атина", "Atina")]
        [InlineData("кой кой е", "koj koj e")]
        [InlineData("ЎЌЉЪ", "UKLЪ")]
        [InlineData("суЇш ыхћ", "suIs yhc")]
        [InlineData("А", "A")]
        [InlineData("Б", "B")]
        [InlineData("В", "V")]
        [InlineData("Г", "G")]
        [InlineData("Д", "D")]
        [InlineData("Е", "E")]
        [InlineData("И", "I")]
        [InlineData("Й", "J")]
        [InlineData("К", "K")]
        [InlineData("Л", "L")]
        [InlineData("М", "M")]
        [InlineData("Н", "N")]
        [InlineData("О", "O")]
        [InlineData("П", "P")]
        [InlineData("Р", "R")]
        [InlineData("С", "S")]
        [InlineData("Т", "T")]
        [InlineData("У", "U")]
        [InlineData("Ф", "F")]
        [InlineData("Х", "H")]
        [InlineData("Ц", "C")]
        [InlineData("Ы", "Y")]
        [InlineData("Ї", "I")]
        public void Should_Transliterate_Cyrillic_To_Latin(string input, string expected)
        {
            var transliterator = new Transliterator(x => x
                .ConfigureCyrillicToLatin());

            var result = transliterator.Transliterate(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("это кириллица", "èto kirillica")]
        [InlineData("Афина", "Afina")]
        [InlineData("Атина", "Atina")]
        [InlineData("кой кой е", "koj koj e")]
        [InlineData("ЎЌЉЪ", "ŬḰL̂Ъ")]
        [InlineData("суЇш ыхћ", "suÏš yhć")]
        [InlineData("А", "A")]
        [InlineData("Б", "B")]
        [InlineData("В", "V")]
        [InlineData("Г", "G")]
        [InlineData("Д", "D")]
        [InlineData("Е", "E")]
        [InlineData("И", "I")]
        [InlineData("Й", "J")]
        [InlineData("К", "K")]
        [InlineData("Л", "L")]
        [InlineData("М", "M")]
        [InlineData("Н", "N")]
        [InlineData("О", "O")]
        [InlineData("П", "P")]
        [InlineData("Р", "R")]
        [InlineData("С", "S")]
        [InlineData("Т", "T")]
        [InlineData("У", "U")]
        [InlineData("Ф", "F")]
        [InlineData("Х", "H")]
        [InlineData("Ц", "C")]
        [InlineData("Ы", "Y")]
        [InlineData("I", "Ì")]
        public void Should_Transliterate_Cyrillic_To_Unicode(string input, string expected)
        {
            var transliterator = new Transliterator(x => x
                .ConfigureCyrillicToUnicode());

            var result = transliterator.Transliterate(input);
            Assert.Equal(expected, result);
        }
    }
}
