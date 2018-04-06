namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// A range searcher for ANSI-style SQL identifiers
    /// </summary>
    public sealed class AnsiSqlIdentifier : StringWithNoEscape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiSqlIdentifier"/> class.
        /// </summary>
        public AnsiSqlIdentifier()
            : base("\"")
        {
        }
    }
}
