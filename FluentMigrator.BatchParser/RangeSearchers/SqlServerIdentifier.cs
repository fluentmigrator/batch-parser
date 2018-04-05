namespace FluentMigrator.BatchParser.RangeSearchers
{
    public class SqlServerIdentifier : CharWithEscapeByDuplication
    {
        public SqlServerIdentifier()
            : base('[', ']')
        {
        }
    }
}
