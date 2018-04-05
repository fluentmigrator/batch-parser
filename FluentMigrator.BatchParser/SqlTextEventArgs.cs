using System;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public class SqlTextEventArgs : EventArgs
    {
        public SqlTextEventArgs([NotNull] string sqlText)
        {
            SqlText = sqlText;
        }

        [NotNull]
        public string SqlText { get; }
    }
}
