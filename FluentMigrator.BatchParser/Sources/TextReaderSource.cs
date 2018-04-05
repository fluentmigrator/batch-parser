using System;
using System.IO;

using JetBrains.Annotations;

namespace FluentMigrator.BatchParser.Sources
{
    public class TextReaderSource : ITextSource, IDisposable
    {
        private readonly TextReader _reader;
        private readonly bool _isOwner;

        public TextReaderSource([NotNull] TextReader reader)
            : this(reader, false)
        {
        }

        public TextReaderSource([NotNull] TextReader reader, bool takeOwnership)
        {
            _reader = reader;
            _isOwner = takeOwnership;
        }

        public ILineReader CreateReader()
        {
            var currentLine = _reader.ReadLine();
            if (currentLine == null)
            {
                return null;
            }

            return new LineReader(_reader, currentLine, 0);
        }

        public void Dispose()
        {
            if (_isOwner)
            {
                _reader.Dispose();
            }
        }

        private class LineReader : ILineReader
        {
            [NotNull]
            private readonly TextReader _reader;

            public LineReader([NotNull] TextReader reader, [NotNull] string currentLine, int index)
            {
                _reader = reader;
                Line = currentLine;
                Index = index;
            }

            public string Line { get; }

            public int Index { get; }

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
                        var line = _reader.ReadLine();
                        if (line == null)
                            return null;
                        currentIndex = 0;
                        currentLine = line;
                        remaining = currentLine.Length;
                    } while (length >= remaining && length != 0);
                }

                currentIndex += length;
                return new LineReader(_reader, currentLine, currentIndex);
            }
        }
    }
}
