using System;
using Xunit;

namespace TranslitSharp.Tests
{
    public class GreekTests
    {
        [Theory]
        [InlineData("ΝΙΚΟΣ ΓΚΑΛΗΣ", "NIKOS GKALIS")]
        [InlineData("Βαγγέλης", "Vangelis")]
        [InlineData("Σπιτική τυρόπιτα", "Spitiki tyropita")]
        [InlineData("Αυτός", "Aftos")]
        [InlineData("Αύξηση", "Afxisi")]
        [InlineData("Αυλή", "Avli")]
        [InlineData("Ευδαιμονία", "Evdaimonia")]
        [InlineData("Μπάλα", "Bala")]
        [InlineData("μπάλα", "bala")]
        [InlineData("Κουμπούρι", "Koumpouri")]
        [InlineData("Μπουμπούκι", "Boumpouki")]
        [InlineData("Ψάρι", "Psari")]
        [InlineData("ψάρι", "psari")]
        [InlineData("ΨΑΡΙ", "PSARI")]
        [InlineData("ΨΑρι", "PSAri")]
        [InlineData("ΠΑΠΑΔΟΠΟΥΛΟΣ", "PAPADOPOULOS")]
        public void Should_Transliterate_Greek_To_Latin(string input, string expected)
        {
            var transliterator = new Transliterator(x => x
                .ConfigureGreekToLatin());

            var result = transliterator.Transliterate(input);
            Assert.Equal(expected, result);
        }

        
    }
}
