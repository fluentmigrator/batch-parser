using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// Read content from a line and advance the internal index
    /// </summary>
    public interface ILineReader
    {
        /// <summary>
        /// Gets the current line
        /// </summary>
        [NotNull]
        string Line { get; }

        /// <summary>
        /// Gets the current index into the line
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the remaining length
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Reads a string with the given <paramref name="length"/> from the <see cref="Line"/>
        /// </summary>
        /// <param name="length">The length of the string to read from the <see cref="Line"/></param>
        /// <returns>The read string</returns>
        [NotNull]
        string ReadString(int length);

        /// <summary>
        /// Creates a new <see cref="ILineReader"/> while moving the internal <see cref="Index"/> by the given <paramref name="length"/>
        /// </summary>
        /// <param name="length">The number of characters to move the internal <see cref="Index"/></param>
        /// <returns>A new line reader with the new index</returns>
        [CanBeNull]
        ILineReader Advance(int length);
    }
}
