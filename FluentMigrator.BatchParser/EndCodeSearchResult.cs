using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public class EndCodeSearchResult
    {
        public EndCodeSearchResult(int index)
        {
            Index = index;
            NestedRangeSearcher = null;
        }

        public EndCodeSearchResult(int index, [CanBeNull] IRangeSearcher nestedRangeSearcher)
        {
            Index = index;
            NestedRangeSearcher = nestedRangeSearcher;
        }

        public bool IsNestedStart => NestedRangeSearcher != null;

        public int Index { get; }

        [CanBeNull]
        public IRangeSearcher NestedRangeSearcher { get; }

        public static implicit operator EndCodeSearchResult(int index)
        {
            if (index == -1)
                return null;
            return new EndCodeSearchResult(index);
        }
    }
}
