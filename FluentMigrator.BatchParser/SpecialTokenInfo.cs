using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public class SpecialTokenInfo
    {
        public SpecialTokenInfo(int index, int length, [NotNull] string token)
        {
            Index = index;
            Length = length;
            Token = token;
        }

        public int Index { get; }
        public int Length { get; }

        [NotNull]
        public string Token { get; }
    }
}
