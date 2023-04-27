namespace Results.Immutable.Metadata;

public record Error(
        string Message,
        ImmutableList<Error> InnerErrors)
{

    public Error(string message, Error innerError)
        : this(
            message,
            ImmutableList.Create(innerError))
    {
    }

    public Error(string message)
        : this(
            message,
            ImmutableList<Error>.Empty)
    {
    }
}

