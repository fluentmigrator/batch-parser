namespace FluentMigrator.BatchParser.RangeSearchers
{
    public sealed class SqlString : CharWithEscapeByDuplication
    {
        public SqlString()
            : base('\'')
        {
        }
    }
}
