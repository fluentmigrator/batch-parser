namespace FluentMigrator.BatchParser.RangeSearchers
{
    public sealed class AnsiSqlIdentifier : StringWithNoEscape
    {
        public AnsiSqlIdentifier()
            : base("\"")
        {
        }
    }
}
