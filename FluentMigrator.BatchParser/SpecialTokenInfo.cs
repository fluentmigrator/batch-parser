using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// Provides special information about the found token
    /// </summary>
    public class SpecialTokenInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialTokenInfo"/> class.
        /// </summary>
        /// <param name="index">The index to the first character that is assigned to the <paramref name="token"/></param>
        /// <param name="length">The content length that is assigned to the <paramref name="token"/></param>
        /// <param name="token">The found token</param>
        /// <remarks>
        /// The <paramref name="index"/> may not point to the real token text and the <paramref name="length"/> might be longer
        /// than the <paramref name="token"/> itself. This is usually the case when the token should be the only text on the line,
        /// but is instead surrounded by whitespace.
        /// </remarks>
        public SpecialTokenInfo(int index, int length, [NotNull] string token)
        {
            Index = index;
            Length = length;
            Token = token;
        }

        /// <summary>
        /// Gets the index to the first character that is assigned to the <see cref="Token"/>
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the content length that is assigned to the <see cref="Token"/>
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the found token
        /// </summary>
        [NotNull]
        public string Token { get; }
    }
}
