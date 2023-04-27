namespace Results.Immutable.Metadata;

public record Success(
        string Message,
        ImmutableList<Success> InnerSuccesses)
{
    public Success(string message)
        : this(
            message,
            ImmutableList<Success>.Empty)
    {
    }

    public Success(string message, Success innerSuccess)
        : this(
            message,
            ImmutableList<Success>.Empty.Add(innerSuccess))
    {
    }
}