﻿using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public interface IRangeSearcher
    {
        int StartCodeLength { get; }
        int EndCodeLength { get; }

        bool IsComment { get; }

        int FindStartCode([NotNull] ILineReader reader);

        [CanBeNull]
        EndCodeSearchResult FindEndCode([NotNull] ILineReader reader);
    }
}
