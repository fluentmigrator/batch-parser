using System.Collections.Generic;

using FluentMigrator.BatchParser.RangeSearchers;
using FluentMigrator.BatchParser.SpecialTokenSearchers;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public class SqlServerBatchParser : SqlBatchParser
    {
        [NotNull, ItemNotNull]
        private static readonly IEnumerable<IRangeSearcher> _rangeSearchers = new IRangeSearcher[]
        {
            new MultiLineComment(),
            new SingleLineComment(),
            new SqlServerIdentifier(),
            new SqlString(),
        };

        [NotNull, ItemNotNull]
        private static readonly IEnumerable<ISpecialTokenSearcher> _specialTokenSearchers = new ISpecialTokenSearcher[]
        {
            new GoSearcher(),
        };

        public SqlServerBatchParser(string newLine = null)
            : base(_rangeSearchers, _specialTokenSearchers, newLine)
        {
        }
    }
}
