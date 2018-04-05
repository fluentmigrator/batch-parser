using System;

using NUnit.Framework;

namespace FluentMigrator.BatchParser.Tests
{
    public abstract class SourceTestsBase
    {
        public abstract ITextSource CreateSource(string content);

        [TestCase("")]
        [TestCase("a", "a")]
        [TestCase("a\n", "a")]
        [TestCase("a\nb", "a", "b")]
        [TestCase("a\nb\n", "a", "b")]
        [TestCase("a\n\nc", "a", "", "c")]
        [TestCase("\nb\n\nd", "", "b", "", "d")]
        public void TestInputs(string content, params string[] lines)
        {
            var source = CreateSource(content);
            Assert.IsNotNull(source);
            var reader = source.CreateReader();

            foreach (var line in lines)
            {
                Assert.NotNull(reader);
                Assert.AreEqual(line, reader.Line);
                Assert.AreEqual(0, reader.Index);

                var nextReader = reader.Advance(reader.Line.Length);
                Assert.AreNotSame(reader, nextReader);
                reader = nextReader;
            }

            Assert.IsNull(reader);
        }

        [Test]
        public void TestReadTooMuch()
        {
            var source = CreateSource("asdasdasd");
            Assert.IsNotNull(source);
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            Assert.AreEqual("asdasdasd", reader.Line);
            Assert.AreEqual(0, reader.Index);
            Assert.Throws<ArgumentOutOfRangeException>(() => reader.ReadString(100));
        }

        [Test]
        public void TestFullLineAdvance()
        {
            var source = CreateSource("asdasdasd");
            Assert.IsNotNull(source);
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            Assert.AreEqual("asdasdasd", reader.Line);
            Assert.AreEqual(0, reader.Index);
            var newReader = reader.Advance(reader.Line.Length);
            Assert.AreNotSame(reader, newReader);
            Assert.IsNull(newReader);
        }

        [Test]
        public void TestPartialAdvance()
        {
            var source = CreateSource("asdasdasd");
            Assert.IsNotNull(source);
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            Assert.AreEqual("asd", reader.ReadString(3));
            var newReader = reader.Advance(1);
            Assert.AreNotSame(reader, newReader);
            Assert.IsNotNull(newReader);
            reader = newReader;
            Assert.AreEqual("sda", reader.ReadString(3));
            Assert.AreEqual(1, reader.Index);
        }

        [Test]
        public void TestOverlappingAdvanceOneLine()
        {
            var source = CreateSource("asd\nqwe");
            Assert.IsNotNull(source);
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            reader = reader.Advance(4);
            Assert.IsNotNull(reader);
            Assert.AreEqual("we", reader.ReadString(2));
            Assert.AreEqual(1, reader.Index);
        }

        [Test]
        public void TestOverlappingAdvanceTwoLine()
        {
            var source = CreateSource("asd\n\nqwe");
            Assert.IsNotNull(source);
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            reader = reader.Advance(4);
            Assert.IsNotNull(reader);
            Assert.AreEqual("we", reader.ReadString(2));
            Assert.AreEqual(1, reader.Index);
        }

        [Test]
        public void TestNonOverlappingAdvanceTwoLine()
        {
            var source = CreateSource("asd\n\nqwe");
            Assert.IsNotNull(source);
            var reader = source.CreateReader();
            Assert.IsNotNull(reader);
            reader = reader.Advance(3);
            Assert.IsNotNull(reader);
            Assert.AreEqual(string.Empty, reader.Line);
            Assert.AreEqual(0, reader.Index);
            reader = reader.Advance(0);
            Assert.IsNotNull(reader);
            Assert.AreEqual("qwe", reader.Line);
            Assert.AreEqual(0, reader.Index);
        }
    }
}
