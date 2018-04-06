using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// A single line comment starting with two dashes (<c>-- comment</c>)
    /// </summary>
    public sealed class SingleLineComment : IRangeSearcher
    {
        private static readonly Regex _startCodeRegex = new Regex("--", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <inheritdoc />
        public int StartCodeLength => 2;

        /// <inheritdoc />
        public int EndCodeLength => 0;

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
            return reader.Line.Length;
        }
    }
}
