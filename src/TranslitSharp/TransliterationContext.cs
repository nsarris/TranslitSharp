using System;
using System.Collections.Generic;

namespace TranslitSharp
{
    public class TransliterationContext
    {
        public string CurrentToken { get; }
        public int CurrentTokenLength { get; }
        public int CurrentTokenIndex { get; }
        public int NextTokenIndex { get; }
        public bool IsFirstToken { get; }
        public bool IsLastToken { get; }
        public TransliterationOptions Options { get; }
        public string OriginalText { get; }
        public CharacterMap ReplacementToken { get; }
        public TokenCase CurrentTokenCase { get; }

        public TransliterationContext(
            string currentToken, 
            int currentTokenLength,
            int currentTokenIndex, 
            int nextTokenIndex,
            CharacterMap replacementToken,
            TransliterationOptions options,
            string originalText,
            bool isLastToken
            )
        {
            CurrentToken = currentToken;
            CurrentTokenLength = currentTokenLength;
            CurrentTokenIndex = currentTokenIndex;
            NextTokenIndex = nextTokenIndex;
            Options = options ?? new TransliterationOptions();
            OriginalText = originalText;
            ReplacementToken = replacementToken;

            IsFirstToken = CurrentTokenIndex == 0;
            IsLastToken = isLastToken;

            switch (Options.CaseConversion)
            {
                case CaseConversion.ToLower:
                    CurrentTokenCase = TokenCase.Lower;
                    break;
                case CaseConversion.ToUpper:
                    CurrentTokenCase = TokenCase.Upper;
                    break;
                default:
                    CurrentTokenCase = char.IsUpper(OriginalText[CurrentTokenIndex]) ? TokenCase.Upper : TokenCase.Lower;
                    var nextCase = IsLastToken || char.IsUpper(OriginalText[CurrentTokenIndex + 1]) ? TokenCase.Upper : TokenCase.Lower;

                    if (CurrentTokenCase == TokenCase.Upper && nextCase == TokenCase.Lower)
                        CurrentTokenCase = TokenCase.Sentence;
                    break;
            }
        }

        public bool IsStartOfWord()
            => IsFirstToken || char.IsWhiteSpace(OriginalText[CurrentTokenIndex - 1]);

        public bool IsEndOfWord()
            => IsLastToken || char.IsWhiteSpace(OriginalText[NextTokenIndex]);
    }
}
