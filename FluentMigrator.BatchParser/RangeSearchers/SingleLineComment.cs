using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    public sealed class SingleLineComment : IRangeSearcher
    {
        private static readonly Regex _startCodeRegex = new Regex("--", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public int StartCodeLength => 2;
        public int EndCodeLength => 0;

        public bool IsComment => true;

        public int FindStartCode(ILineReader reader)
        {
            var match = _startCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return -1;
            return match.Index;
        }

        public EndCodeSearchResult FindEndCode(ILineReader reader)
        {
            return reader.Line.Length;
        }
    }
}
