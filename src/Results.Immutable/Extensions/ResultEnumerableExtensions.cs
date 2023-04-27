namespace Results.Immutable;

/// <summary>
///     Contains extension methods for <see cref="IEnumerable{T}" />
///     of <see cref="Result{T}" />.
/// </summary>
public static class ResultEnumerableExtensions
{
    /// <inheritdoc cref="Result.Merge{T,TResult}" />
    public static Result<Unit> Merge(this IEnumerable<Result<Unit>> results) =>
        Result.Merge(results.ToArray());

    /// <inheritdoc cref="Result.Transpose{T}(IReadOnlyCollection{Result{T}})" />
    public static Result<ImmutableList<T>> Transpose<T>(this IEnumerable<Result<T>> results) =>
        Result.Transpose(results.ToArray());
}
