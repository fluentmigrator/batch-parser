namespace FluentMigrator.BatchParser.RangeSearchers
{
    public sealed class MultiLineComment : StringWithNoEscape
    {
        public MultiLineComment()
            : base("/*", "*/", true)
        {
        }
    }
}
