using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// Ranges with multi-character start and end tokens (e.g. <c>/* */</c>)
    /// </summary>
    /// <remarks>
    /// Escaping is not supported.
    /// </remarks>
    public class StringWithNoEscape : IRangeSearcher
    {
        private readonly Regex _startCodeRegex;
        private readonly Regex _endCodeRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWithNoEscape"/> class.
        /// </summary>
        /// <param name="startAndEndCode">The start- and end code string</param>
        /// <param name="isComment">Is this range a comment?</param>
        public StringWithNoEscape(string startAndEndCode, bool isComment = false)
        {
            IsComment = isComment;
            StartCodeLength = EndCodeLength = startAndEndCode.Length;

            var codePattern = Regex.Escape(startAndEndCode);

            _startCodeRegex = _endCodeRegex = new Regex(codePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWithNoEscape"/> class.
        /// </summary>
        /// <param name="startCode">The start code</param>
        /// <param name="endCode">The end code</param>
        /// <param name="isComment">Is this range a comment?</param>
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

        /// <inheritdoc />
        public int StartCodeLength { get; }

        /// <inheritdoc />
        public int EndCodeLength { get; }

        /// <inheritdoc />
        public bool IsComment { get; }

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
            var match = _endCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return null;
            return match.Index;
        }
    }
}
