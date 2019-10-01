using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TranslitSharp.Tests
{
    public class CoreTests
    {
        [Theory]
        [InlineData("123€123", "123€123", null)]
        [InlineData("123€123", "123?123", "?")]
        [InlineData("123€123", "123!123", "!")]
        public void Should_Transliterate_With_Specified_Invalid_Ascii_Character_Replacement(string input, string expected, string replacementChar)
        {
            var transliterator = new Transliterator(x => x
                .ReplaceRemainingNonAsciiCharacters(replacementChar));

            var result = transliterator.Transliterate(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123€123", "123E123")]
        public void Should_Transliterate_With_Custom_Map(string input, string expected)
        {
            var transliterator = new Transliterator(x => x
                .AddCharacterMap("€", "E", map =>
                {
                    map.AddAllCasePermutations = false;
                }));

            var result = transliterator.Transliterate(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("ΣΑΛΑΜΙ ΑΕΡΟΣ", "SALAMI AEROS", CaseConversion.None)]
        [InlineData("ΣΑΛΑΜΙ ΑΕΡΟΣ", "SALAMI AEROS", CaseConversion.ToUpper)]
        [InlineData("ΣΑΛΑΜΙ ΑΕΡΟΣ", "salami aeros", CaseConversion.ToLower)]
        [InlineData("σαλάμι αέρος", "salami aeros", CaseConversion.None)]
        [InlineData("σαλάμι αέρος", "SALAMI AEROS", CaseConversion.ToUpper)]
        [InlineData("σαλάμι αέρος", "salami aeros", CaseConversion.ToLower)]
        public void Should_Transliterate_With_CaseConversion(string input, string expected, CaseConversion caseConversion)
        {
            var transliterator = new Transliterator(x => x
               .ConfigureGreekToLatin());

            var result = transliterator.Transliterate(input, new TransliterationOptions(caseConversion: caseConversion));
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("€υρώ", "?yro")]
        public void Should_Transliterate_And_Replace_Non_ASCII(string input, string expected)
        {
            var transliterator = new Transliterator(x => x
                .ConfigureGreekToLatin()
                .ReplaceRemainingNonAsciiCharacters());

            var result = transliterator.Transliterate(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Should_Throw_Exception_On_Duplicate()
        {
            Assert.Throws<DuplicateTokenException>(() =>
                new Transliterator(x => x
                    .AddCharacterMap("A", "1")
                    .AddCharacterMap("A", "2"))
                );
        }

        [Fact]
        public void Should_Take_First_On_Duplicate()
        {
            var transliterator = new Transliterator(x => x
                .OnDuplicateToken(DuplicateTokenBehaviour.TakeFirst)
                .AddCharacterMap("A", "1")
                .AddCharacterMap("A", "2"));

            var result = transliterator.Transliterate("AA");
            Assert.Equal("11", result);
        }

        [Fact]
        public void Should_Take_Last_On_Duplicate()
        {
            var transliterator = new Transliterator(x => x
                .OnDuplicateToken(DuplicateTokenBehaviour.TakeLast)
                .AddCharacterMap("A", "1")
                .AddCharacterMap("A", "2"));

            var result = transliterator.Transliterate("AA");
            Assert.Equal("22", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        [InlineData("")]
        public void Should_Handle_Null_EmptyString_WhiteSpace(string input)
        {
            var transliterator = new Transliterator();
            var result = transliterator.Transliterate(input);
            Assert.Equal(input, result);
        }
    }
}
