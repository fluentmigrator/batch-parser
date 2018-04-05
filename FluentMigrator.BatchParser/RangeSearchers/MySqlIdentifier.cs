namespace FluentMigrator.BatchParser.RangeSearchers
{
    public sealed class MySqlIdentifier : CharWithEscapeByDuplication
    {
        public MySqlIdentifier()
            : base('`')
        {
        }
    }
}
