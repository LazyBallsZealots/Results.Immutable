namespace Results.Immutable.Tests.ResultFactoriesTests;

internal sealed record RootError(
        string Message,
        ImmutableList<Error> Errors)
    : Error(
        Message,
        Errors)
{
    public RootError(string message)
        : this(
            message,
            ImmutableList<Error>.Empty)
    {
    }
}