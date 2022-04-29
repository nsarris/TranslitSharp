using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TranslitSharp
{
    public class Transliterator
    {
        private sealed class TokenMapGroup
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
            if (string.IsNullOrEmpty(text)) return text;

            var currentTokenIndex = 0;
            var stringBuilder = new StringBuilder(options?.EstimatedLength ?? text.Length);
            var hasChanges = false;
            
            while (currentTokenIndex < text.Length)
            {
                var match = false;

                foreach (var token in tokenMapsPerLength.Where(x => currentTokenIndex + x.Length <= text.Length))
                {
                    var currentTokenHashCode = text.GetHashCodeFromCharacters(currentTokenIndex, token.Length);
                    
                    if (token.TokenMap.TryGetValue(currentTokenHashCode, out var replacement))
                    {
                        var tokenCharacters = text.Substring(currentTokenIndex, token.Length);
                        var nextTokenIndex = currentTokenIndex + token.Length;
                        var isLastToken = nextTokenIndex == text.Length;

                        var context = new TransliterationContext(tokenCharacters, token.Length, currentTokenIndex, nextTokenIndex, replacement, options, text, isLastToken);

                        var handler = replacement.Handler ?? TransliterationTokenHandler.Instance;

                        var replacementToken = handler.Handle(context);

                        AppendString(stringBuilder, replacementToken, ref hasChanges);

                        if (!hasChanges)
                            hasChanges = !tokenCharacters.Equals(replacementToken);

                        currentTokenIndex = nextTokenIndex;
                        match = true;
                        break;
                    }
                }

                if (!match)
                {
                    AppendCharacter(stringBuilder, text[currentTokenIndex++].ToCase(options?.CaseConversion), ref hasChanges);
                }
            }

            return hasChanges ? stringBuilder.ToString() : text;
        }

        private void AppendString(StringBuilder stringBuilder, string characters, ref bool hasChanges)
        {
            for(var i = 0; i < characters.Length; i++)
                AppendCharacter(stringBuilder, characters[i], ref hasChanges);
        }

        private void AppendCharacter(StringBuilder stringBuilder, char character, ref bool hasChanges)
        {
            if (ShouldReplaceNonAsciiCharacter(character))
            {
                if (!hasChanges)
                    hasChanges = configuration.NonAsciiCharacterReplacement.Length != 1
                        || configuration.NonAsciiCharacterReplacement[0] != character;

                stringBuilder.Append(configuration.NonAsciiCharacterReplacement);
            }
            else
                stringBuilder.Append(character);
        }

        private bool ShouldReplaceNonAsciiCharacter(char character)
        {
            return configuration.NonAsciiCharacterReplacement is not null
                && !character.IsAscii(configuration.ExcludeExtendedAsciiCharacters);
        }
    }
}

