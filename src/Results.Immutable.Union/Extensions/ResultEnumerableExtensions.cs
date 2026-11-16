using System.Collections.Immutable;

namespace Results.Immutable;

public static class ResultEnumerableExtensions
{
    extension(IEnumerable<Result<Unit>> results)
    {
        public Result<Unit> Merge() => Result.Merge([.. results,]);
    }

    extension<T>(IEnumerable<Result<T>> results)
    {
        public Result<ImmutableList<T>> Transpose() => Result.Transpose(results);
    }
}