using System;
using System.Collections.Generic;
using System.Linq;

namespace TranslitSharp
{

    public class TransliteratorConfiguration
    {
        private static readonly string defaultAsciiReplacementChar = new System.Text.EncoderReplacementFallback().DefaultString;

        internal List<CharacterMap> CustomMaps { get; } = new List<CharacterMap>();
        internal string NonAsciiCharacterReplacement { get; set; }
        internal bool ExcludeExtendedAsciiCharacters { get; set; }
        internal DuplicateTokenBehaviour DuplicateTokenBehaviour { get; set; }

        public TransliteratorConfiguration OnDuplicateToken(DuplicateTokenBehaviour behaviour)
        {
            DuplicateTokenBehaviour = behaviour;
            return this;
        }

        public TransliteratorConfiguration ReplaceRemainingNonAsciiCharacters()
        {
            NonAsciiCharacterReplacement = defaultAsciiReplacementChar;
            return this;
        }

        public TransliteratorConfiguration ReplaceRemainingNonAsciiCharacters(string replacement)
        {
            NonAsciiCharacterReplacement = replacement;
            return this;
        }
        public TransliteratorConfiguration ReplaceRemainingNonExtendedAsciiCharacters()
        {
            NonAsciiCharacterReplacement = defaultAsciiReplacementChar;
            ExcludeExtendedAsciiCharacters = true;
            return this;
        }

        public TransliteratorConfiguration ReplaceRemainingNonExtendedAsciiCharacters(string replacement)
        {
            NonAsciiCharacterReplacement = replacement;
            ExcludeExtendedAsciiCharacters = true;
            return this;
        }

        public TransliteratorConfiguration AddCharacterMap(string token, string replacementToken, Action<CustomMapOptions> configure = null)
        {
            var options = new CustomMapOptions();
            configure?.Invoke(options);

            CustomMaps.AddRange((options.AddAllCasePermutations ?
                token.Permute() : new List<string>() { token })
            .Select(x => new CharacterMap(x, replacementToken, options.Handler)));
            
            return this;
        }

        public TransliteratorConfiguration AddCharacterMaps(params (string token, string replacementToken, Action<CustomMapOptions> configure)[] customMaps)
        {
            foreach (var (token, replacementToken, configure) in customMaps)
                this.AddCharacterMap(token, replacementToken, configure);
            return this;
        }

        public TransliteratorConfiguration AddCharacterMaps(params (string token, string replacementToken)[] customMaps)
        {
            foreach (var (token, replacementToken) in customMaps)
                this.AddCharacterMap(token, replacementToken);
            return this;
        }
    }
}
