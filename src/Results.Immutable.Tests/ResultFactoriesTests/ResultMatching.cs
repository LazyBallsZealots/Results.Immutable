namespace Results.Immutable.Tests.ResultFactoriesTests;

internal static class ResultMatching
{
    public static bool ValueIsAUnit(Result<Unit> result) => result is { Some.Value: _, };
}