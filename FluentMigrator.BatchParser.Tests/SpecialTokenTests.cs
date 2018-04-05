﻿using FluentMigrator.BatchParser.Sources;
using FluentMigrator.BatchParser.SpecialTokenSearchers;

using NUnit.Framework;

namespace FluentMigrator.BatchParser.Tests
{
    public class SpecialTokenTests
    {
        [TestCase(";")]
        [TestCase(" ; ")]
        [TestCase(" ;")]
        [TestCase("; ")]
        public void TestIfSemicolonExists(string input)
        {
            var source = new LinesSource(new[] { input });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            var tokenSearcher = new SemicolonSearcher();
            var result = tokenSearcher.Find(reader);
            Assert.IsNotNull(result);
            Assert.Greater(result.Index, -1);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(";", result.Token);
        }

        [Test]
        public void TestIfSemicolonMissing()
        {
            var source = new LinesSource(new[] { string.Empty });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            var tokenSearcher = new SemicolonSearcher();
            var result = tokenSearcher.Find(reader);
            Assert.IsNull(result);
        }

        [TestCase("GO", "GO")]
        [TestCase(" GO ", "GO")]
        [TestCase(" GO", "GO")]
        [TestCase("GO ", "GO")]
        [TestCase("GO 123", "GO 123")]
        [TestCase("  GO 123  ", "GO 123")]
        [TestCase("  gO 123  ", "gO 123")]
        [TestCase("  gO 123", "gO 123")]
        [TestCase("gO 123", "gO 123")]
        public void TestIfGoExists(string input, string expected)
        {
            var source = new LinesSource(new[] { input });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            var tokenSearcher = new GoSearcher();
            var result = tokenSearcher.Find(reader);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Index);
            Assert.AreEqual(input.Length, result.Length);
            Assert.AreEqual(expected, result.Token);
        }

        [TestCase("x GO")]
        [TestCase("GO x")]
        [TestCase("GO 123 123")]
        public void TestIfGoMissing(string input)
        {
            var source = new LinesSource(new[] { string.Empty });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            var tokenSearcher = new GoSearcher();
            var result = tokenSearcher.Find(reader);
            Assert.IsNull(result);
        }

        [Test]
        public void TestIfGoMissingIfReaderNotAtBeginOfLine()
        {
            var source = new LinesSource(new[] { " GO" });
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            reader = reader.Advance(1);
            Assert.IsNotNull(reader);
            var tokenSearcher = new GoSearcher();
            var result = tokenSearcher.Find(reader);
            Assert.IsNull(result);
        }
    }
}
