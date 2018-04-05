﻿using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.SpecialTokenSearchers
{
    public class GoSearcher : ISpecialTokenSearcher
    {
        private static readonly Regex _regex = new Regex(@"^\s*(?<statement>GO(\s+(?<count>\d)+)?)\s*$", RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public SpecialTokenInfo Find(ILineReader reader)
        {
            if (reader.Index != 0)
                return null;

            var match = _regex.Match(reader.Line);
            if (!match.Success)
                return null;

            return new SpecialTokenInfo(0, reader.Line.Length, match.Groups["statement"].Value);
        }
    }
}