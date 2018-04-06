using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// Interface to search for content with a given start- and end code
    /// </summary>
    public interface IRangeSearcher
    {
        /// <summary>
        /// Gets the length of the start code
        /// </summary>
        int StartCodeLength { get; }

        /// <summary>
        /// Gets the length of the end code
        /// </summary>
        int EndCodeLength { get; }

        /// <summary>
        /// Is this range a comment?
        /// </summary>
        bool IsComment { get; }

        /// <summary>
        /// Gets the index into the <paramref name="reader"/> where the start code was found
        /// </summary>
        /// <param name="reader">The reader where the start code is searched</param>
        /// <returns><c>-1</c> when the start code couldn't be found</returns>
        int FindStartCode([NotNull] ILineReader reader);

        /// <summary>
        /// Search for an end code
        /// </summary>
        /// <param name="reader">The reader where the end code is searched</param>
        /// <returns><c>null</c> when the end code couldn't be found (or a nested start code)</returns>
        [CanBeNull]
        EndCodeSearchResult FindEndCode([NotNull] ILineReader reader);
    }
}
