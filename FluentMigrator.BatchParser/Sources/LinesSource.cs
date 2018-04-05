using System.Collections.Generic;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser.Sources
{
    public class LinesSource : ITextSource
    {
        [NotNull]
        private readonly IEnumerable<string> _batchSource;

        public LinesSource([NotNull] IEnumerable<string> batchSource)
        {
            _batchSource = batchSource;
        }

        public ILineReader CreateReader()
        {
            var enumerator = _batchSource.GetEnumerator();
            if (!enumerator.MoveNext())
                return null;
            return new LineReader(enumerator, 0);
        }

        private class LineReader : ILineReader
        {
            [NotNull]
            private readonly IEnumerator<string> _enumerator;

            public LineReader([NotNull] IEnumerator<string> enumerator, int index)
            {
                _enumerator = enumerator;
                Index = index;
            }

            public string Line => _enumerator.Current;

            public int Index { get; }

            public int Length => Line.Length - Index;

            public string ReadString(int length)
            {
                return Line.Substring(Index, length);
            }

            public ILineReader Advance(int length)
            {
                var currentLine = Line;
                var currentIndex = Index;
                var remaining = currentLine.Length - currentIndex;

                if (length >= remaining)
                {
                    do
                    {
                        length -= remaining;
                        if (!_enumerator.MoveNext())
                            return null;
                        currentIndex = 0;
                        currentLine = _enumerator.Current;
                        remaining = currentLine.Length;
                    } while (length >= remaining && length != 0);
                }

                currentIndex += length;
                return new LineReader(_enumerator, currentIndex);
            }
        }
    }
}
