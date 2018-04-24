#region License
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Text.RegularExpressions;

namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// A single line comment starting with a pound sign (<c># comment</c>)
    /// </summary>
    public sealed class PoundSignSingleLineComment : IRangeSearcher
    {
        private readonly Regex _startCodeRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="PoundSignSingleLineComment"/> class.
        /// </summary>
        public PoundSignSingleLineComment()
        {
            var startCode = "#";
            _startCodeRegex = new Regex(Regex.Escape(startCode), RegexOptions.CultureInvariant | RegexOptions.Compiled);
            StartCodeLength = startCode.Length;
        }

        /// <inheritdoc />
        public int StartCodeLength { get; }

        /// <inheritdoc />
        public int EndCodeLength => 0;

        /// <inheritdoc />
        public bool IsComment => true;

        /// <inheritdoc />
        public int FindStartCode(ILineReader reader)
        {
            if (reader.Index != 0)
                return -1;
            var match = _startCodeRegex.Match(reader.Line, reader.Index);
            if (!match.Success)
                return -1;
            var skippedText = reader.ReadString(match.Index - reader.Index);
            if (!string.IsNullOrWhiteSpace(skippedText))
                return -1;
            return match.Index;
        }

        /// <inheritdoc />
        public EndCodeSearchResult FindEndCode(ILineReader reader)
        {
            return reader.Line.Length;
        }
    }
}
