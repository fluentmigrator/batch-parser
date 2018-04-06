using System;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// Event arguments for the SQL text collected by the <see cref="SqlBatchParser"/>
    /// </summary>
    public class SqlTextEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTextEventArgs"/> class.
        /// </summary>
        /// <param name="sqlText">The collected SQL text</param>
        public SqlTextEventArgs([NotNull] string sqlText)
        {
            SqlText = sqlText;
        }

        /// <summary>
        /// Gets the collected SQL text
        /// </summary>
        [NotNull]
        public string SqlText { get; }
    }
}
