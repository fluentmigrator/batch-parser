using FluentMigrator.BatchParser.Sources;

namespace FluentMigrator.BatchParser.Tests
{
    public class LinesSourceTests : SourceTestsBase
    {
        public override ITextSource CreateSource(string content)
        {
            string[] lines;
            if (content.Length == 0)
            {
                lines = new string[0];
            }
            else if (content.EndsWith('\n'))
            {
                lines = content.Substring(0, content.Length - 1).Split('\n');
            }
            else
            {
                lines = content.Split('\n');
            }

            return new LinesSource(lines);
        }
    }
}
