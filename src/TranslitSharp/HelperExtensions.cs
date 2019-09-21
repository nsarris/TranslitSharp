using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslitSharp
{
    internal static class HelperExtensions
    {
        public static StringBuilder AppendChars(this StringBuilder stringBuilder, IEnumerable<char> characters, int length = 0)
        {
            if (length > 0)
                stringBuilder.EnsureCapacity(stringBuilder.Capacity + length);

            foreach (var c in characters)
                stringBuilder.Append(c);
            return stringBuilder;
        }

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
            return listPermutations;
        }

        public static int GetHashCodeFromCharacters(this string text)
        {
            return GetHashCodeFromCharacters(text, text.Length);
        }

        private const int hash = 17;
        private const int seed = 31;
        private const int first_hash_seed = hash * seed;

        public static int GetHashCodeFromCharacters(this IEnumerable<char> characters, int length)
        {
            if (length <= 0) return 0;
            if (length == 1) return characters.First().GetHashCode();
            if (length == 2) return seed * (first_hash_seed + characters.First().GetHashCode()) + characters.Last().GetHashCode();

            int hash1 = 5381;
            int hash2 = hash1;

            int i = 0;
            foreach (var character in characters)
            {
                if (i == 0)
                    hash1 = ((hash1 << 5) + hash1) ^ character;
                else
                    hash2 = ((hash2 << 5) + hash2) ^ character;

                if (++i == 2) i = 0;
            }

            return hash1 + (hash2 * 1566083941);

            //unchecked
            //{
            //    int hash = 17;
            //    foreach (var character in characters)
            //        hash = 31 * hash + character.GetHashCode();
            //    return hash;
            //}
        }

        public static bool IsAscii(this char c) => (c >> 8) == 0;
    }
}
