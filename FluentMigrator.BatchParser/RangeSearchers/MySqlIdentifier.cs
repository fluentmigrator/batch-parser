namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// The MySQL identifier quotes using backticks
    /// </summary>
    public sealed class MySqlIdentifier : CharWithEscapeByDuplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlIdentifier"/> class.
        /// </summary>
        public MySqlIdentifier()
            : base('`')
        {
        }
    }
}
