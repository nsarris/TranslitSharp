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
                TransileratedCapitalizeFirst = replacementToken.Substring(0, 1).ToUpper() + replacementToken.Substring(1).ToLower();
                TransliteratedLower = replacementToken.ToLower();
                TransliteratedUpper = replacementToken.ToUpper();
            }
        }

        public string Token { get; }

        public string TransileratedCapitalizeFirst { get; }
        public string TransliteratedUpper { get; }
        public string TransliteratedLower { get; }

        internal TransliterationTokenHandler Handler { get; }
        
        public string GetTransliterated(TokenCase letterCasing)
        {
            switch (letterCasing)
            {
                case TokenCase.Sentence: return TransileratedCapitalizeFirst;
                case TokenCase.Upper: return TransliteratedUpper;
                case TokenCase.Lower: return TransliteratedLower;
                default: return null;
            }
        }
    }
}
