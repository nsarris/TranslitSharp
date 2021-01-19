using System;
using System.Collections.Generic;
using System.Text;

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
        public void Should_Transliterate_Gyrillic_To_Latin(string input, string expected)
        {
            var transliterator = new Transliterator(x => x
                .ConfigureCyrillicToLatin());

            var result = transliterator.Transliterate(input);
            Assert.Equal(expected, result);
        }
    }
}
