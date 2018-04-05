using FluentMigrator.BatchParser.RangeSearchers;
using FluentMigrator.BatchParser.Sources;

using NUnit.Framework;

namespace FluentMigrator.BatchParser.Tests
{
    public class RangeSearcherTests
    {
        [TestCase("  \"qweqwe\"  ", "qweqwe")]
        [TestCase(@"  ""qwe\""qweqwe""  ", "qwe\\")]
        public void TestAnsiSqlIdentifiers(string input, string expected)
        {
            var source = new LinesSource(new[] { input });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);

            var rangeSearcher = new AnsiSqlIdentifier();
            var startIndex = rangeSearcher.FindStartCode(reader);
            Assert.AreNotEqual(-1, startIndex);

            reader = reader.Advance(startIndex + rangeSearcher.StartCodeLength);
            Assert.IsNotNull(reader);

            var endInfo = rangeSearcher.FindEndCode(reader);
            Assert.IsNotNull(endInfo);
            Assert.IsFalse(endInfo.IsNestedStart);
            var endIndex = endInfo.Index;
            var result = reader.ReadString(endIndex - reader.Index);

            Assert.AreEqual(expected, result);
        }

        [TestCase("  `qweqwe`  ", "qweqwe")]
        [TestCase("  `qwe``qweqwe`  ", "qwe``qweqwe")]
        public void TestMySqlIdentifiers(string input, string expected)
        {
            var source = new LinesSource(new[] { input });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);

            var rangeSearcher = new MySqlIdentifier();
            var startIndex = rangeSearcher.FindStartCode(reader);
            Assert.AreNotEqual(-1, startIndex);

            reader = reader.Advance(startIndex + rangeSearcher.StartCodeLength);
            Assert.IsNotNull(reader);

            var endInfo = rangeSearcher.FindEndCode(reader);
            Assert.IsNotNull(endInfo);
            Assert.IsFalse(endInfo.IsNestedStart);
            var endIndex = endInfo.Index;
            var result = reader.ReadString(endIndex - reader.Index);

            Assert.AreEqual(expected, result);
        }

        [TestCase("  [qweqwe]  ", "qweqwe")]
        [TestCase("  [qwe]]qweqwe]  ", "qwe]]qweqwe")]
        public void TesSqlServerIdentifiers(string input, string expected)
        {
            var source = new LinesSource(new[] { input });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);

            var rangeSearcher = new SqlServerIdentifier();
            var startIndex = rangeSearcher.FindStartCode(reader);
            Assert.AreNotEqual(-1, startIndex);

            reader = reader.Advance(startIndex + rangeSearcher.StartCodeLength);
            Assert.IsNotNull(reader);

            var endInfo = rangeSearcher.FindEndCode(reader);
            Assert.IsNotNull(endInfo);
            Assert.IsFalse(endInfo.IsNestedStart);
            var endIndex = endInfo.Index;
            var result = reader.ReadString(endIndex - reader.Index);

            Assert.AreEqual(expected, result);
        }

        [TestCase("  'qweqwe'  ", "qweqwe")]
        [TestCase("  'qwe''qweqwe'  ", "qwe''qweqwe")]
        public void TesSqlString(string input, string expected)
        {
            var source = new LinesSource(new[] { input });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);

            var rangeSearcher = new SqlString();
            var startIndex = rangeSearcher.FindStartCode(reader);
            Assert.AreNotEqual(-1, startIndex);

            reader = reader.Advance(startIndex + rangeSearcher.StartCodeLength);
            Assert.IsNotNull(reader);

            var endInfo = rangeSearcher.FindEndCode(reader);
            Assert.IsNotNull(endInfo);
            Assert.IsFalse(endInfo.IsNestedStart);
            var endIndex = endInfo.Index;
            var result = reader.ReadString(endIndex - reader.Index);

            Assert.AreEqual(expected, result);
        }
    }
}
