using System.Collections.Generic;

namespace TranslitSharp
{
    public interface ICharCollection : IEnumerable<char>
    {
        char this[int index] { get; }
        int Length { get; }
    }
}
