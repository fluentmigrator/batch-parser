using System;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// Event arguments for SQL text to be collected
    /// </summary>
    internal class SqlBatchCollectorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBatchCollectorEventArgs"/> class.
        /// </summary>
        /// <param name="sqlContent">The SQL text to be collected</param>
        /// <param name="isEndOfLine"><c>true</c> when a new line character should be appended</param>
        public SqlBatchCollectorEventArgs([NotNull] string sqlContent, bool isEndOfLine = false)
        {
            SqlContent = sqlContent;
            IsEndOfLine = isEndOfLine;
        }

        /// <summary>
        /// Gets the SQL text to be collected
        /// </summary>
        [NotNull]
        public string SqlContent { get; }

        /// <summary>
        /// Gets a value indicating whether a new line character should be appended
        /// </summary>
        public bool IsEndOfLine { get; }
    }
}
