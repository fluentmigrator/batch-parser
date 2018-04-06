using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// Utility class that handles single-character ranges (e.g. <c>'text'</c>) where the
    /// end characters might be duplicated to escape it.
    /// </summary>
    public class CharWithEscapeByDuplication : IRangeSearcher
    {
        private readonly char _endChar;
        private readonly Regex _startCodeRegex;
        private readonly Regex _endCodeRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharWithEscapeByDuplication"/> class.
        /// </summary>
        /// <param name="startAndEndChar">The character used for start and end</param>
        /// <param name="isComment">Is this a comment</param>
        public CharWithEscapeByDuplication(char startAndEndChar, bool isComment = false)
        {
            IsComment = isComment;
            _endChar = startAndEndChar;
            var codePattern = Regex.Escape(startAndEndChar.ToString());
            _startCodeRegex = _endCodeRegex =
                new Regex(codePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharWithEscapeByDuplication"/> class.
        /// </summary>
        /// <param name="startChar">The start character</param>
        /// <param name="endChar">The end character</param>
        /// <param name="isComment">Is this a comment</param>
        public CharWithEscapeByDuplication(char startChar, char endChar, bool isComment = false)
        {
            IsComment = isComment;
            _endChar = endChar;
            var startCodePattern = Regex.Escape(startChar.ToString());
            var endCodePattern = Regex.Escape(endChar.ToString());
            _startCodeRegex = new Regex(startCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
            _endCodeRegex = new Regex(endCodePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public int StartCodeLength => 1;

        /// <inheritdoc />
        public int EndCodeLength => 1;

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
