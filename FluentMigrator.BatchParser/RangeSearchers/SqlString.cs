namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// A ANSI SQL string (<c>'string'</c>)
    /// </summary>
    public sealed class SqlString : CharWithEscapeByDuplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlString"/> class.
        /// </summary>
        public SqlString()
            : base('\'')
        {
        }
    }
}
