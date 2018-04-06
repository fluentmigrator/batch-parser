namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// An SQL server style quoted identifer (<c>[identifier]</c>)
    /// </summary>
    public sealed class SqlServerIdentifier : CharWithEscapeByDuplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerIdentifier"/> class.
        /// </summary>
        public SqlServerIdentifier()
            : base('[', ']')
        {
        }
    }
}
