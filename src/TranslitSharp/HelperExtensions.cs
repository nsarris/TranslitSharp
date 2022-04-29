using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslitSharp
{
    internal static class HelperExtensions
    {
        public static List<string> Permute(this string s)
        {
            List<string> listPermutations = new List<string>();

            char[] array = s.ToLower().ToCharArray();
            int iterations = (1 << array.Length) - 1;

            for (int i = 0; i <= iterations; i++)
            {
                for (int j = 0; j < array.Length; j++)
                    array[j] = (i & (1 << j)) != 0
                                  ? char.ToUpper(array[j])
                                  : char.ToLower(array[j]);
                listPermutations.Add(new string(array));
            }

            return listPermutations.Distinct().ToList();
        }

        private const int hash = 5381;
        private const int seed = 1566083941;

        public static int GetHashCodeFromCharacters(this string text)
        {
            return GetHashCodeFromCharacters(text, 0, text.Length);
        }

        public static int GetHashCodeFromCharacters(this string text, int startIndex, int length)
        {
            if (length <= 0) return 0;
            if (length == 1) return text[startIndex].GetHashCode();

            var hash1 = hash;
            var hash2 = hash;

            for (var i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                    hash1 = ((hash1 << 5) + hash1) ^ text[startIndex + i];
                else
                    hash2 = ((hash2 << 5) + hash2) ^ text[startIndex + i];
            }

            return hash1 + (hash2 * seed);
        }

        public static bool IsAscii(this char c, bool excludeExtendedAscii) 
            => excludeExtendedAscii ? c < 128 : (c >> 8) == 0;

        public static char ToCase(this char c, CaseConversion? caseConversion)
            => caseConversion switch
            {
                null => c,
                CaseConversion.None => c,
                CaseConversion.ToUpper => char.ToUpper(c),
                CaseConversion.ToLower => char.ToLower(c),
                _ => throw new ArgumentOutOfRangeException(nameof(caseConversion))
            };
    }
}
