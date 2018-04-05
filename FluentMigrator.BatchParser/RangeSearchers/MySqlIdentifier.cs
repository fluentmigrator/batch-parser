namespace FluentMigrator.BatchParser.RangeSearchers
{
    public class MySqlIdentifier : CharWithEscapeByDuplication
    {
        public MySqlIdentifier()
            : base('`')
        {
        }
    }
}
