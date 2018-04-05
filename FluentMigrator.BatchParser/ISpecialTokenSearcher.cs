using JetBrains.Annotations;

namespace FluentMigrator.BatchParser
{
    public interface ISpecialTokenSearcher
    {
        [CanBeNull]
        SpecialTokenInfo Find([NotNull] ILineReader reader);
    }
}
