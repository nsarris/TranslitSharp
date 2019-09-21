using System.Collections.Generic;
using System.Collections;

namespace TranslitSharp
{
    public class StringCharCollection : ICharCollection
    {
        private readonly string @string;

        public StringCharCollection(string @string)
        {
            this.@string = @string;
        }

        public char this[int index] => @string[index];

        public int Length => @string.Length;

        public IEnumerator<char> GetEnumerator() => @string.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string ToString()
        {
            return @string;
        }
    }
}
