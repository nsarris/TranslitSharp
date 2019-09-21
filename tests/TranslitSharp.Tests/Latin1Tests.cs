using System;
using Xunit;

namespace TranslitSharp.Tests
{
    public class Latin1Tests
    {
        [Theory]
        [InlineData("Jalapeño", "Jalapeno")]
        [InlineData("Über", "Uber")]
        public void Should_Transliterate_And_Latin1Supplement_To_Latin(string input, string expected)
        {
            var transliterator = new Transliterator(x => x
                .ConfigureLatin1SupplementToLatin());

            var result = transliterator.Transliterate(input, new TransliterationOptions());
            Assert.Equal(expected, result);
        }
    }
}
