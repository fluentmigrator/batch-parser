using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentMigrator.BatchParser
{
    public sealed class SearchContext
    {
        public event EventHandler<BatchSqlEventArgs> BatchSql;
        public event EventHandler<SpecialTokenEventArgs> SpecialToken;

        public SearchContext(
            IEnumerable<IRangeSearcher> rangeHandlers,
            IEnumerable<ISpecialTokenSearcher> specialTokenSearchers)
        {
            SpecialTokenSearchers = specialTokenSearchers as IList<ISpecialTokenSearcher> ?? specialTokenSearchers.ToList().AsReadOnly();
            RangeHandlers = rangeHandlers as IList<IRangeSearcher> ?? rangeHandlers.ToList().AsReadOnly();
        }

        public IList<ISpecialTokenSearcher> SpecialTokenSearchers { get; }
        public IList<IRangeSearcher> RangeHandlers { get; }

        internal void OnBatchSql(BatchSqlEventArgs e)
        {
            BatchSql?.Invoke(this, e);
        }

        internal void OnSpecialToken(SpecialTokenEventArgs e)
        {
            SpecialToken?.Invoke(this, e);
        }
    }
}
