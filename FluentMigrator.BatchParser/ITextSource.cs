using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public interface ITextSource
    {
        [CanBeNull]
        ILineReader CreateReader();
    }
}
