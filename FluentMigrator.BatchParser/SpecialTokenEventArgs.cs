using System;

namespace FluentMigrator.BatchParser
{
    public class SpecialTokenEventArgs : EventArgs
    {
        public SpecialTokenEventArgs(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}
