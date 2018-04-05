namespace FluentMigrator.BatchParser
{
    public class StartCodeResultInfo
    {
        public StartCodeResultInfo(IRangeSearcher rangeSearcher, int index)
        {
            RangeSearcher = rangeSearcher;
            Index = index;
        }

        public IRangeSearcher RangeSearcher { get; }
        public int Index { get; }
    }
}
