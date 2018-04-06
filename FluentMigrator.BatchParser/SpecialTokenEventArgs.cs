using System;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    /// <summary>
    /// Event arguments for a special token
    /// </summary>
    public class SpecialTokenEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialTokenEventArgs"/> class.
        /// </summary>
        /// <param name="token">The found token</param>
        public SpecialTokenEventArgs([NotNull] string token)
        {
            Token = token;
        }

        /// <summary>
        /// Gets the found token
        /// </summary>
        [NotNull]
        public string Token { get; }
    }
}
