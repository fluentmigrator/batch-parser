using System;
using System.IO;
using System.Text;

using FluentMigrator.BatchParser.RangeSearchers;
using FluentMigrator.BatchParser.Sources;

using NUnit.Framework;

namespace FluentMigrator.BatchParser.Tests
{
    public class RangeSearcherTests
    {
        [TestCase(typeof(AnsiSqlIdentifier), 1, 1)]
        [TestCase(typeof(MySqlIdentifier), 1, 1)]
        [TestCase(typeof(SqlServerIdentifier), 1, 1)]
        [TestCase(typeof(SqlString), 1, 1)]
        [TestCase(typeof(MultiLineComment), 2, 2)]
        [TestCase(typeof(SingleLineComment), 2, 0)]
        public void TestConfiguration(Type type, int startLength, int endLength)
        {
            var instance = Activator.CreateInstance(type);
            Assert.IsInstanceOf<IRangeSearcher>(instance);
            var rangeSearcher = (IRangeSearcher)instance;
            Assert.AreEqual(startLength, rangeSearcher.StartCodeLength);
            Assert.AreEqual(endLength, rangeSearcher.EndCodeLength);
        }

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
        public void TestSqlServerIdentifiers(string input, string expected)
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
        [TestCase("  'qweqwe'", "qweqwe")]
        [TestCase("  'qwe''qweqwe'  ", "qwe''qweqwe")]
        public void TestSqlString(string input, string expected)
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

        [TestCase("  'qweqwe")]
        public void TestIncompleteSqlString(string input)
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
            Assert.IsNull(endInfo);
        }

        [Test]
        public void TestMissingSqlString()
        {
            var source = new LinesSource(new[] { string.Empty });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);

            var rangeSearcher = new SqlString();
            var startIndex = rangeSearcher.FindStartCode(reader);
            Assert.AreEqual(-1, startIndex);
        }

        [TestCase("  /* blah */  ", " blah ")]
        [TestCase("  /* blah /* blubb */  ", " blah /* blubb ")]
        public void TestMultiLineCommentWithSingleLine(string input, string expected)
        {
            var source = new TextReaderSource(new StringReader(input));
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);

            var rangeSearcher = new MultiLineComment();
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

        [TestCase("/** blah\n * blubb\n*/  ", "* blah\n * blubb\n")]
        public void TestMultiLineCommentWithMultipleLines(string input, string expected)
        {
            using (var source = new TextReaderSource(new StringReader(input), true))
            {
                var reader = source.CreateReader();
                Assert.IsNotNull(reader);

                var foundStart = false;
                var content = new StringBuilder();
                var writer = new StringWriter(content)
                {
                    NewLine = "\n",
                };

                var rangeSearcher = new MultiLineComment();
                while (reader != null)
                {
                    if (!foundStart)
                    {
                        var startIndex = rangeSearcher.FindStartCode(reader);
                        if (startIndex == -1)
                        {
                            reader = reader.Advance(reader.Length);
                            continue;
                        }

                        foundStart = true;
                        reader = reader.Advance(startIndex + rangeSearcher.StartCodeLength);
                        Assert.IsNotNull(reader);
                    }

                    var endInfo = rangeSearcher.FindEndCode(reader);
                    if (endInfo == null)
                    {
                        writer.WriteLine(reader.ReadString(reader.Length));
                        reader = reader.Advance(reader.Length);
                        continue;
                    }

                    var contentLength = endInfo.Index - reader.Index;
                    writer.Write(reader.ReadString(contentLength));
                    reader = reader.Advance(contentLength + rangeSearcher.EndCodeLength);
                    foundStart = false;
                }

                Assert.IsFalse(foundStart);
                Assert.AreEqual(expected, content.ToString());
            }
        }

        [TestCase("   -- qweqwe", " qweqwe")]
        [TestCase("   -- qwe\nqwe", " qwe")]
        public void TestSingleLineComment(string input, string expected)
        {
            var source = new TextReaderSource(new StringReader(input));
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);

            var foundStart = false;
            var content = new StringBuilder();
            var writer = new StringWriter(content)
            {
                NewLine = "\n",
            };

            var rangeSearcher = new SingleLineComment();
            while (reader != null)
            {
                if (!foundStart)
                {
                    var startIndex = rangeSearcher.FindStartCode(reader);
                    if (startIndex == -1)
                    {
                        reader = reader.Advance(reader.Length);
                        continue;
                    }

                    foundStart = true;
                    reader = reader.Advance(startIndex + rangeSearcher.StartCodeLength);
                    Assert.IsNotNull(reader);
                }

                var endInfo = rangeSearcher.FindEndCode(reader);
                Assert.IsNotNull(endInfo);

                var contentLength = endInfo.Index - reader.Index;
                writer.Write(reader.ReadString(contentLength));
                reader = reader.Advance(contentLength + rangeSearcher.EndCodeLength);
                foundStart = false;
            }

            Assert.IsFalse(foundStart);
            Assert.AreEqual(expected, content.ToString());
        }
    }
}
