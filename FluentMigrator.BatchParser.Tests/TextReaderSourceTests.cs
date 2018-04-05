using System.IO;

using FluentMigrator.BatchParser.Sources;

namespace FluentMigrator.BatchParser.Tests
{
    public class TextReaderSourceTests : SourceTestsBase
    {
        public override ITextSource CreateSource(string content)
        {
            return new TextReaderSource(new StringReader(content));
        }
    }
}
