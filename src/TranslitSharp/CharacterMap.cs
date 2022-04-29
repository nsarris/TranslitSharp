using System;

namespace TranslitSharp
{
    public class CharacterMap
    {
        public CharacterMap(string token, string replacementToken, TransliterationTokenHandler handler = null)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));
            if (replacementToken is null) throw new ArgumentNullException(nameof(token));

            Token = token;
            Handler = handler;

            if (Token.ToLower() == token.ToUpper())
            {
                TransileratedCapitalizeFirst = TransliteratedLower = TransliteratedUpper = replacementToken;
            }
            else
            {
                TransliteratedLower = replacementToken.ToLower();
                TransliteratedUpper = replacementToken.ToUpper();
                TransileratedCapitalizeFirst = replacementToken.Length == 0 ? 
                    TransliteratedUpper :
                    replacementToken.Substring(0, 1).ToUpper() + replacementToken.Substring(1).ToLower();
            }
        }

        public string Token { get; }

        public string TransileratedCapitalizeFirst { get; }
        public string TransliteratedUpper { get; }
        public string TransliteratedLower { get; }

        internal TransliterationTokenHandler Handler { get; }

        public string GetTransliterated(TokenCase letterCasing) 
            => letterCasing switch
            {
                TokenCase.Sentence => TransileratedCapitalizeFirst,
                TokenCase.Upper => TransliteratedUpper,
                TokenCase.Lower => TransliteratedLower,
                _ => throw new ArgumentOutOfRangeException(nameof(letterCasing)),
            };
    }
}
