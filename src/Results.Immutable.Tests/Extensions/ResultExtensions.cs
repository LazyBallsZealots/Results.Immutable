namespace Results.Immutable.Tests.Extensions;

internal static class ResultExtensions
{
    public static bool ValueMatches<T>(
        this Result<T> result,
        Predicate<T?> predicate) =>
        result is { Option.ValueOrDefault: var value, } && predicate(value);
}