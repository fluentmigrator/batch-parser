using System;

namespace FluentMigrator.BatchParser
{
    public class BatchSqlEventArgs : EventArgs
    {
        public BatchSqlEventArgs(string sqlContent, bool isEndOfLine = false)
        {
            SqlContent = sqlContent;
            IsEndOfLine = isEndOfLine;
        }

        public BatchSqlEventArgs()
        {
            SqlContent = string.Empty;
            IsEndOfLine = true;
        }

        public string SqlContent { get; }
        public bool IsEndOfLine { get; }
    }
}
