namespace FluentMigrator.BatchParser
{
    public class SpecialTokenInfo
    {
        public SpecialTokenInfo(int index, int length, string token)
        {
            Index = index;
            Length = length;
            Token = token;
        }

        public int Index { get; }
        public int Length { get; }
        public string Token { get; }
    }
}
