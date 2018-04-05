using System;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    internal class SqlBatchCollectorEventArgs : EventArgs
    {
        public SqlBatchCollectorEventArgs([NotNull] string sqlContent, bool isEndOfLine = false)
        {
            SqlContent = sqlContent;
            IsEndOfLine = isEndOfLine;
        }

        [NotNull]
        public string SqlContent { get; }
        public bool IsEndOfLine { get; }
    }
}
