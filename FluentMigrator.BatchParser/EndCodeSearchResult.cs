namespace FluentMigrator.BatchParser
{
    public class EndCodeSearchResult
    {
        public EndCodeSearchResult(int index)
        {
            Index = index;
            NestedRangeSearcher = null;
        }

        public EndCodeSearchResult(int index, IRangeSearcher nestedRangeSearcher)
        {
            Index = index;
            NestedRangeSearcher = nestedRangeSearcher;
        }

        public bool IsNestedStart => NestedRangeSearcher != null;

        public int Index { get; }

        public IRangeSearcher NestedRangeSearcher { get; }

        public static implicit operator EndCodeSearchResult(int index)
        {
            if (index == -1)
                return null;
            return new EndCodeSearchResult(index);
        }
    }
}
