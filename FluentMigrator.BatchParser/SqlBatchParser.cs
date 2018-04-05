using System;
using System.Collections.Generic;
using System.IO;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public class SqlBatchParser
    {
        [NotNull]
        [ItemNotNull]
        private readonly IEnumerable<IRangeSearcher> _rangeSearchers;

        [NotNull]
        [ItemNotNull]
        private readonly IEnumerable<ISpecialTokenSearcher> _specialTokenSearchers;

        [NotNull]
        private readonly string _newLine;

        public SqlBatchParser(
            [NotNull, ItemNotNull] IEnumerable<IRangeSearcher> rangeSearchers,
            [NotNull, ItemNotNull] IEnumerable<ISpecialTokenSearcher> specialTokenSearchers,
            string newLine = null)
        {
            _rangeSearchers = rangeSearchers;
            _specialTokenSearchers = specialTokenSearchers;
            _newLine = newLine ?? Environment.NewLine;
        }

        public event EventHandler<SpecialTokenEventArgs> SpecialToken;
        public event EventHandler<SqlTextEventArgs> SqlText;

        public void Process(ITextSource source, bool stripComments = false)
        {
            var output = new StringWriter()
            {
                NewLine = _newLine,
            };

            var context = new SearchContext(_rangeSearchers, _specialTokenSearchers, stripComments);
            context.BatchSql += (sender, evt) =>
            {
                output.Write(evt.SqlContent);
                if (evt.IsEndOfLine)
                    output.WriteLine();
            };

            context.SpecialToken += (sender, evt) =>
            {
                output.Flush();
                var sqlText = output.ToString();
                OnSqlText(new SqlTextEventArgs(sqlText));
                OnSpecialToken(evt);
                output = new StringWriter();
            };

            var reader = source.CreateReader();
            if (reader == null)
                return;

            var status = SearchStatus.Init(context, reader);
            do
            {
                status = status.Process();
            }
            while (status != null);

            var remaining = output.ToString();
            if (!string.IsNullOrWhiteSpace(remaining))
            {
                OnSqlText(new SqlTextEventArgs(remaining));
            }
        }

        protected virtual void OnSpecialToken(SpecialTokenEventArgs e)
        {
            SpecialToken?.Invoke(this, e);
        }

        protected virtual void OnSqlText(SqlTextEventArgs e)
        {
            SqlText?.Invoke(this, e);
        }
    }
}
