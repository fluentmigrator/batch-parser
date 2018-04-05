using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    internal sealed class SearchContext
    {
        public SearchContext(
            [NotNull, ItemNotNull] IEnumerable<IRangeSearcher> rangeHandlers,
            [NotNull, ItemNotNull] IEnumerable<ISpecialTokenSearcher> specialTokenSearchers,
            bool stripComments)
        {
            StripComments = stripComments;
            SpecialTokenSearchers = specialTokenSearchers as IList<ISpecialTokenSearcher> ?? specialTokenSearchers.ToList().AsReadOnly();
            RangeSearchers = rangeHandlers as IList<IRangeSearcher> ?? rangeHandlers.ToList().AsReadOnly();
        }

        [NotNull, ItemNotNull]
        public IList<ISpecialTokenSearcher> SpecialTokenSearchers { get; }

        [NotNull, ItemNotNull]
        public IList<IRangeSearcher> RangeSearchers { get; }

        public bool StripComments { get; }

        public event EventHandler<SqlBatchCollectorEventArgs> BatchSql;
        public event EventHandler<SpecialTokenEventArgs> SpecialToken;

        internal void OnBatchSql([NotNull] SqlBatchCollectorEventArgs e)
        {
            BatchSql?.Invoke(this, e);
        }

        internal void OnSpecialToken([NotNull] SpecialTokenEventArgs e)
        {
            SpecialToken?.Invoke(this, e);
        }
    }
}
