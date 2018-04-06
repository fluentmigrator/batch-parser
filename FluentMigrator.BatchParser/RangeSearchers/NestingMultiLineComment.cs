using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// An example implementation of a nested multi-line comment (e.g. <c>/* comment /* nested */ */</c>)
    /// </summary>
    public sealed class NestingMultiLineComment : IRangeSearcher
    {
        private readonly Regex _startCodeRegex;
        private readonly Regex _endCodeRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="NestingMultiLineComment"/> class.
        /// </summary>
        public NestingMultiLineComment()
        {
            var startCode = "/*";
            var endCode = "*/";

            StartCodeLength = startCode.Length;
            EndCodeLength = endCode.Length;

            var startCodePattern = Regex.Escape(startCode);
            var endCodePattern = Regex.Escape(endCode);

            _startCodeRegex = new Regex(startCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
            _endCodeRegex = new Regex(endCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public int StartCodeLength { get; }

        /// <inheritdoc />
        public int EndCodeLength { get; }

        /// <inheritdoc />
        public bool IsComment => true;

        /// <inheritdoc />
        public int FindStartCode(ILineReader reader)
        {
            var match = _startCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return -1;
            return match.Index;
        }

        /// <inheritdoc />
        public EndCodeSearchResult FindEndCode(ILineReader reader)
        {
            var matchStart = _startCodeRegex.Match(reader.Line, reader.Index);
            var matchEnd = _endCodeRegex.Match(reader.Line, reader.Index);
            if (!matchEnd.Success && !matchStart.Success)
                return null;
            if (!matchStart.Success)
                return matchEnd.Index;
            if (!matchEnd.Success)
                return new EndCodeSearchResult(matchStart.Index, this);
            if (matchStart.Index < matchEnd.Index)
                return new EndCodeSearchResult(matchStart.Index, this);
            return matchEnd.Index;
        }
    }
}
