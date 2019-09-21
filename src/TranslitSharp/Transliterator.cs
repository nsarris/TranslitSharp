using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TranslitSharp
{
    public class Transliterator
    {
        private class TokenMapGroup
        {
            public int Length { get; set; }
            public IReadOnlyDictionary<int, CharacterMap> TokenMap { get; set; }
        }

        private readonly List<TokenMapGroup> tokenMapsPerLength;
        private readonly TransliteratorConfiguration configuration;

        public Transliterator() : this(null) { }

        public Transliterator(Action<TransliteratorConfiguration> configure)
        {
            configuration = new TransliteratorConfiguration();

            configure?.Invoke(configuration);

            tokenMapsPerLength = BuildStringTokenMap(configuration.CustomMaps);
        }

        private List<TokenMapGroup> BuildStringTokenMap(IEnumerable<CharacterMap> maps)
        {
            return maps
                .Select(x => new KeyValuePair<string, CharacterMap>(x.Token, x))
                .GroupBy(x => x.Key.Length)
                .Select(x => new TokenMapGroup
                {
                    Length = x.Key,
                    TokenMap = ResolveDuplicates(x).ToDictionary(m => m.Key.GetHashCodeFromCharacters(), m => m.Value)
                })
                .OrderByDescending(x => x.Length)
                .ToList();
        }

        private IEnumerable<KeyValuePair<string, CharacterMap>> ResolveDuplicates(IEnumerable<KeyValuePair<string, CharacterMap>> maps)
        {
            var groups = maps.GroupBy(x => x.Key).ToList();

            if (configuration.DuplicateTokenBehaviour == DuplicateTokenBehaviour.ThrowException)
            {
                var duplicates = groups.Where(x => x.Count() > 1).ToList();
                if (duplicates.Any())
                    throw new DuplicateTokenException(duplicates.Select(x => x.Key));

                return maps;
            }
            else if (configuration.DuplicateTokenBehaviour == DuplicateTokenBehaviour.TakeLast)
            {
                return groups.Select(x => new KeyValuePair<string, CharacterMap>(x.Key, x.Last().Value));
            }
            else
            {
                return groups.Select(x => new KeyValuePair<string, CharacterMap>(x.Key, x.First().Value));
            }
        }

        public string Transliterate(string text, TransliterationOptions options = null)
        {
            ICharCollection currentText = new StringCharCollection(text);

            foreach (var token in tokenMapsPerLength)
            {
                var stringBuilder = new StringBuilder(currentText.Length);

                var lastTokenIndex = currentText.Length - token.Length;
                var currentTokenIndex = 0;
                var hasChanges = false;

                while (currentTokenIndex <= lastTokenIndex)
                {
                    var tokenCharacters = currentText.Skip(currentTokenIndex).Take(token.Length);
                    var currentTokenHashCode = tokenCharacters.GetHashCodeFromCharacters(token.Length);
                    var isLastToken = currentTokenIndex == lastTokenIndex;

                    if (token.TokenMap.TryGetValue(currentTokenHashCode, out var replacement))
                    {
                        var nextTokenIndex = currentTokenIndex + token.Length;

                        var context = new TransliterationContext(tokenCharacters, token.Length, currentTokenIndex, nextTokenIndex, replacement, options, currentText, isLastToken);

                        var handler = replacement.Handler ?? TransliterationTokenHandler.Instance;

                        var replacementToken = handler.Handle(context);

                        stringBuilder.Append(replacementToken);

                        hasChanges = true;
                        currentTokenIndex = nextTokenIndex;
                    }
                    else
                    {
                        if (isLastToken)
                            stringBuilder.AppendChars(currentText.Skip(currentTokenIndex), currentText.Length - currentTokenIndex);
                        else
                            stringBuilder.Append(currentText[currentTokenIndex]);

                        currentTokenIndex++;
                    }
                }

                if (hasChanges)
                    currentText = new StringBuilderCharCollection(stringBuilder);
            }

            return ReplaceNonAsciiCharacters(currentText, configuration.NonAsciiCharacterReplacement);
        }

        private string ReplaceNonAsciiCharacters(ICharCollection text, string replacement)
        {
            if (replacement == null)
                return text.ToString();

            var stringBuilder = new StringBuilder(text.Length);

            foreach(var c in text)
            {
                if (c.IsAscii())
                    stringBuilder.Append(c);
                else
                    stringBuilder.Append(replacement);
            }

            return stringBuilder.ToString();
        }
    }
}

