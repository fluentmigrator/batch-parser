using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// An interface to create a reader that is used to read the SQL script lines
    /// </summary>
    public interface ITextSource
    {
        /// <summary>
        /// Creates a reader
        /// </summary>
        /// <returns><c>null</c> when no content was available</returns>
        [CanBeNull]
        ILineReader CreateReader();
    }
}
