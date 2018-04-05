using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    public class CharWithEscapeByDuplication : IRangeSearcher
    {
        private readonly char _endChar;
        private readonly Regex _startCodeRegex;
        private readonly Regex _endCodeRegex;

        public CharWithEscapeByDuplication(char startAndEndChar)
        {
            _endChar = startAndEndChar;
            var codePattern = Regex.Escape(startAndEndChar.ToString());
            _startCodeRegex = _endCodeRegex =
                new Regex(codePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public CharWithEscapeByDuplication(char startChar, char endChar)
        {
            _endChar = endChar;
            var startCodePattern = Regex.Escape(startChar.ToString());
            var endCodePattern = Regex.Escape(endChar.ToString());
            _startCodeRegex = new Regex(startCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
            _endCodeRegex = new Regex(endCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public int StartCodeLength => 1;
        public int EndCodeLength => 1;

        public int FindStartCode(ILineReader reader)
        {
            var match = _startCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return -1;
            return match.Index;
        }

        public EndCodeSearchResult FindEndCode(ILineReader reader)
        {
            var lastIndex = reader.Line.Length - 1;
            var startIndex = reader.Index;
            for (; ; )
            {
                var match = _endCodeRegex.Match(reader.Line, startIndex);
                if (!match.Success)
                    return null;

                var foundIndex = match.Index;
                if (foundIndex == lastIndex)
                    return foundIndex;

                if (reader.Line[foundIndex + 1] != _endChar)
                    return foundIndex;

                startIndex = foundIndex + 2;
            }
        }
    }
}
