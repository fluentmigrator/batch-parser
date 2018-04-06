using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// The result of a <see cref="IRangeSearcher.FindEndCode"/> operation
    /// </summary>
    public class EndCodeSearchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndCodeSearchResult"/> class.
        /// </summary>
        /// <param name="index">The index into the <see cref="ILineReader"/> where the end code was found</param>
        public EndCodeSearchResult(int index)
        {
            Index = index;
            NestedRangeSearcher = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndCodeSearchResult"/> class.
        /// </summary>
        /// <param name="index">The index into the <see cref="ILineReader"/> where the nested start code was found</param>
        /// <param name="nestedRangeSearcher">The searcher to be used to find the end of the nested range</param>
        public EndCodeSearchResult(int index, [NotNull] IRangeSearcher nestedRangeSearcher)
        {
            Index = index;
            NestedRangeSearcher = nestedRangeSearcher;
        }

        /// <summary>
        /// Gets a value indicating whether this is a nested range
        /// </summary>
        public bool IsNestedStart => NestedRangeSearcher != null;

        /// <summary>
        /// Gets the index into the previously tested <see cref="ILineReader"/> of the end code or nested start code
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the searcher to be used to find the end of the nested range
        /// </summary>
        [CanBeNull]
        public IRangeSearcher NestedRangeSearcher { get; }

        /// <summary>
        /// Operator to convert an index of the end code into a <see cref="EndCodeSearchResult"/>
        /// </summary>
        /// <param name="index">The index into the <see cref="ILineReader"/> of the end code</param>
        public static implicit operator EndCodeSearchResult(int index)
        {
            if (index == -1)
                return null;
            return new EndCodeSearchResult(index);
        }
    }
}
