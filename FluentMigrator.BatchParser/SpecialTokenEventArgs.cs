using System;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public class SpecialTokenEventArgs : EventArgs
    {
        public SpecialTokenEventArgs([NotNull] string token)
        {
            Token = token;
        }

        [NotNull]
        public string Token { get; }
    }
}
