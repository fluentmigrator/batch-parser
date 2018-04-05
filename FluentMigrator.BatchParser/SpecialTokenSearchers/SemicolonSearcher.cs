using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.SpecialTokenSearchers
{
    public class SemicolonSearcher : ISpecialTokenSearcher
    {
        private static readonly Regex _regex = new Regex(";", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public SpecialTokenInfo Find(ILineReader reader)
        {
            var match = _regex.Match(reader.Line);
            if (!match.Success)
                return null;

            return new SpecialTokenInfo(match.Index, match.Length, match.Value);
        }
    }
}
