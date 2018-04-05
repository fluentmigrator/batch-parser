﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    internal class SearchStatus
    {
        [NotNull]
        private readonly SearchContext _context;

        [NotNull]
        private readonly ILineReader _reader;

        [NotNull, ItemNotNull]
        private readonly Stack<IRangeSearcher> _activeRanges;

        [CanBeNull]
        private readonly SpecialTokenInfo _foundToken;

        private SearchStatus(
            [NotNull] SearchContext context,
            [NotNull] ILineReader reader,
            [NotNull, ItemNotNull]Stack<IRangeSearcher> activeRanges,
            [CanBeNull] SpecialTokenInfo foundToken)
        {
            _context = context;
            _reader = reader;
            _activeRanges = activeRanges;
            _foundToken = foundToken;
        }

        [NotNull]
        public static SearchStatus Init([NotNull] SearchContext context, [NotNull] ILineReader reader)
        {
            return new SearchStatus(context, reader, new Stack<IRangeSearcher>(), null);
        }

        [CanBeNull]
        public SearchStatus Process()
        {
            if (_activeRanges.Count == 0)
                return FindTokenOrRangeStart();

            return FindRangeEnd();
        }

        [CanBeNull]
        private SearchStatus FindRangeEnd()
        {
            Debug.Assert(
                _activeRanges.Count != 0 || _foundToken == null,
                "Operation can only be performed when a range is active and no token has been found");

            var searcher = _activeRanges.Peek();
            var result = searcher.FindEndCode(_reader);
            if (result == null)
            {
                var reader = WriteSql(_reader);
                if (reader == null)
                    throw new InvalidOperationException($"Missing end of range ({searcher.GetType().Name})");
                return new SearchStatus(_context, reader, _activeRanges, null);
            }

            var nextReader = WriteSql(_reader, result.Index + searcher.EndCodeLength);
            if (nextReader == null)
                return null;

            _activeRanges.Pop();
            return new SearchStatus(_context, nextReader, _activeRanges, null);
        }

        [CanBeNull]
        private SearchStatus FindTokenOrRangeStart()
        {
            Debug.Assert(_activeRanges.Count == 0, "Operation can only be performed when no range is active");

            var rangeStart = FindRangeStart(_reader, _context.RangeSearchers);
            var tokenInfo = FindToken(_reader, _context.SpecialTokenSearchers);
            if (tokenInfo != null)
            {
                if (rangeStart == null || rangeStart.Index > tokenInfo.Index)
                {
                    var reader = WriteSql(_reader, tokenInfo.Index, tokenInfo.Length);
                    _context.OnSpecialToken(new SpecialTokenEventArgs(tokenInfo.Token));
                    if (reader == null)
                        return null;
                    return new SearchStatus(_context, reader, _activeRanges, tokenInfo);
                }
            }
            else if (rangeStart == null)
            {
                var reader = WriteSql(_reader);
                if (reader == null)
                    return null;
                return new SearchStatus(_context, reader, _activeRanges, null);
            }

            var nextReader = WriteSql(_reader, rangeStart.Index + rangeStart.Searcher.StartCodeLength);
            if (nextReader == null)
                throw new InvalidOperationException($"Missing end of range ({rangeStart.Searcher.GetType().Name})");

            _activeRanges.Push(rangeStart.Searcher);
            return new SearchStatus(_context, nextReader, _activeRanges, null);
        }

        [CanBeNull]
        private ILineReader WriteSql([NotNull] ILineReader reader)
        {
            var content = reader.ReadString(reader.Length);
            _context.OnBatchSql(new SqlBatchCollectorEventArgs(content, true));
            return reader.Advance(reader.Length);
        }

        [CanBeNull]
        private ILineReader WriteSql([NotNull] ILineReader reader, int itemIndex, int skipLength = 0)
        {
            var readLength = itemIndex - reader.Index;
            var content = reader.ReadString(readLength);
            if (!string.IsNullOrEmpty(content))
            {
                var isEndOfLine = (readLength + skipLength) == reader.Length;
                _context.OnBatchSql(new SqlBatchCollectorEventArgs(content, isEndOfLine));
            }

            return reader.Advance(readLength + skipLength);
        }

        private SpecialTokenInfo FindToken([NotNull] ILineReader reader, [NotNull, ItemNotNull] IEnumerable<ISpecialTokenSearcher> searchers)
        {
            SpecialTokenInfo result = null;
            foreach (var searcher in searchers)
            {
                var searcherResult = searcher.Find(reader);
                if (searcherResult != null && (result == null || result.Index > searcherResult.Index))
                {
                    result = searcherResult;
                }
            }

            return result;
        }

        [CanBeNull]
        private static RangeStart FindRangeStart([NotNull] ILineReader reader, [NotNull, ItemNotNull] IEnumerable<IRangeSearcher> searchers)
        {
            RangeStart result = null;
            foreach (var searcher in searchers)
            {
                var index = searcher.FindStartCode(reader);
                if (index != -1 && (result == null || result.Index > index))
                {
                    result = new RangeStart(searcher, index);
                }
            }

            return result;
        }

        private class RangeStart
        {
            public RangeStart([NotNull] IRangeSearcher searcher, int index)
            {
                Searcher = searcher;
                Index = index;
            }

            [NotNull]
            public IRangeSearcher Searcher { get; }

            public int Index { get; }
        }
    }
}