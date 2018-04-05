﻿using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public interface IRangeSearcher
    {
        int StartCodeLength { get; }
        int EndCodeLength { get; }
        int FindStartCode([NotNull] ILineReader reader);
        EndCodeSearchResult FindEndCode([NotNull] ILineReader reader);
    }
}