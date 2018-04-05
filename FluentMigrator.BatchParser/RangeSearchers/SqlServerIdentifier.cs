namespace FluentMigrator.BatchParser.RangeSearchers
{
    public sealed class SqlServerIdentifier : CharWithEscapeByDuplication
    {
        public SqlServerIdentifier()
            : base('[', ']')
        {
        }
    }
}
