using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public interface ILineReader
    {
        [NotNull]
        string Line { get; }
        int Index { get; }
        int Length { get; }

        [NotNull]
        string ReadString(int length);

        [CanBeNull]
        ILineReader Advance(int length);
    }
}
