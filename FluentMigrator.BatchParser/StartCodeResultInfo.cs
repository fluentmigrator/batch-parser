using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public class StartCodeResultInfo
    {
        public StartCodeResultInfo([NotNull] IRangeSearcher rangeSearcher, int index)
        {
            RangeSearcher = rangeSearcher;
            Index = index;
        }

        [NotNull]
        public IRangeSearcher RangeSearcher { get; }
        public int Index { get; }
    }
}
