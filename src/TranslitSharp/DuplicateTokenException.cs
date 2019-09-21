using System;
using System.Collections.Generic;
using System.Linq;

namespace TranslitSharp
{
    public class DuplicateTokenException : Exception
    {
        public DuplicateTokenException(IEnumerable<string> tokens)
            :base(GetMessage(tokens))
        {
            Tokens = tokens.ToList();
        }

        public IEnumerable<string> Tokens { get; }

        private static string GetMessage(IEnumerable<string> tokens)
        {
            return "Duplicate tokens found in configuration: " + Environment.NewLine
                + string.Join(Environment.NewLine, tokens.Select(x => $"{x} ( {string.Join(",", x.Select(c => $"U+{((int)c):X4}")) } )"));
        }
    }
}
