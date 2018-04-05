namespace FluentMigrator.BatchParser.RangeSearchers
{
    public class SqlString : CharWithEscapeByDuplication
    {
        public SqlString()
            : base('\'')
        {
        }
    }
}
