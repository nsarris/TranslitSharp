using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TranslitSharp
{
    public class StringBuilderCharCollection : ICharCollection
    {
        private readonly StringBuilder stringBuilder;

        public StringBuilderCharCollection(StringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;
        }

        public char this[int index] => stringBuilder[index];

        public int Length => stringBuilder.Length;

        public IEnumerator<char> GetEnumerator() 
        {
            for (var i = 0; i < stringBuilder.Length; i++)
                yield return stringBuilder[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }
}
