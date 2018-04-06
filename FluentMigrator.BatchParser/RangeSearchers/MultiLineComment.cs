namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// The default multi-line comment (<c>/* comment */</c>)
    /// </summary>
    public sealed class MultiLineComment : StringWithNoEscape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineComment"/> class.
        /// </summary>
        public MultiLineComment()
            : base("/*", "*/", true)
        {
        }
    }
}
