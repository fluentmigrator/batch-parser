using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    public class StringWithNoEscape : IRangeSearcher
    {
        private readonly Regex _startCodeRegex;
        private readonly Regex _endCodeRegex;

        public StringWithNoEscape(string startAndEndCode, bool isComment = false)
        {
            IsComment = isComment;
            StartCodeLength = EndCodeLength = startAndEndCode.Length;

            var codePattern = Regex.Escape(startAndEndCode);

            _startCodeRegex = _endCodeRegex = new Regex(codePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public StringWithNoEscape(string startCode, string endCode, bool isComment = false)
        {
            IsComment = isComment;
            StartCodeLength = startCode.Length;
            EndCodeLength = endCode.Length;

            var startCodePattern = Regex.Escape(startCode);
            var endCodePattern = Regex.Escape(endCode);

            _startCodeRegex = new Regex(startCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
            _endCodeRegex = new Regex(endCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        public int StartCodeLength { get; }
        public int EndCodeLength { get; }

        public bool IsComment { get; }

        public int FindStartCode(ILineReader reader)
        {
            var match = _startCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return -1;
            return match.Index;
        }

        public EndCodeSearchResult FindEndCode(ILineReader reader)
        {
            var match = _endCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return null;
            return match.Index;
        }
    }
}
