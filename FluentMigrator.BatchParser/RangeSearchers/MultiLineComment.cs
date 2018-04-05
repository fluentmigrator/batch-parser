namespace FluentMigrator.BatchParser.RangeSearchers
{
    public class MultiLineComment : StringWithNoEscape
    {
        public MultiLineComment()
            : base("/*", "*/", true)
        {
        }
    }
}
