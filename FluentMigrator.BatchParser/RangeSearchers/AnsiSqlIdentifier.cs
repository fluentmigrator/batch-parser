namespace FluentMigrator.BatchParser.RangeSearchers
{
    public class AnsiSqlIdentifier : StringWithNoEscape
    {
        public AnsiSqlIdentifier()
            : base("\"")
        {
        }
    }
}
