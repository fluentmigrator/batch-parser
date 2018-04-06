using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.SpecialTokenSearchers
{
    /// <summary>
    /// Searches for a semicolon
    /// </summary>
    /// <remarks>
    /// This special token searcher might be used to separate SQL statements in a batch.
    /// </remarks>
    public class SemicolonSearcher : ISpecialTokenSearcher
    {
        private static readonly Regex _regex = new Regex(";", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <inheritdoc />
        public SpecialTokenInfo Find(ILineReader reader)
        {
            var match = _regex.Match(reader.Line);
            if (!match.Success)
                return null;

            return new SpecialTokenInfo(match.Index, match.Length, match.Value);
        }
    }
}
