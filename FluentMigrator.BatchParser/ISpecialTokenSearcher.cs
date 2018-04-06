using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// Searches for special tokens (e.g. <c>GO</c>)
    /// </summary>
    public interface ISpecialTokenSearcher
    {
        /// <summary>
        /// Search for the special token in the given <paramref name="reader"/>
        /// </summary>
        /// <param name="reader">The reader used to search the token</param>
        /// <returns><c>null</c> when the token couldn't be found</returns>
        [CanBeNull]
        SpecialTokenInfo Find([NotNull] ILineReader reader);
    }
}
