using System.Collections.Generic;

using FluentMigrator.BatchParser.RangeSearchers;
using FluentMigrator.BatchParser.SpecialTokenSearchers;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// A specialization of the <see cref="SqlBatchParser"/> for the Microsoft SQL Server
    /// </summary>
    /// <remarks>
    /// It uses the following range searchers: <see cref="MultiLineComment"/>, <see cref="SingleLineComment"/>, <see cref="SqlServerIdentifier"/>, <see cref="SqlString"/>
    /// and the following token searchers: <see cref="GoSearcher"/>.
    /// </remarks>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerBatchParser"/> class.
        /// </summary>
        /// <param name="newLine">The string used to write a new line sequence</param>
        public SqlServerBatchParser(string newLine = null)
            : base(_rangeSearchers, _specialTokenSearchers, newLine)
        {
        }
    }
}
