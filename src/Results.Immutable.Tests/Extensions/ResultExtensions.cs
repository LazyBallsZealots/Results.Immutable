namespace Results.Immutable.Tests.Extensions;

internal static class ResultExtensions
{
    public static bool ValueMatches<T>(
        this Result<T> result,
        Predicate<T> predicate) =>
        result.Some is var (x) && predicate(x);
}