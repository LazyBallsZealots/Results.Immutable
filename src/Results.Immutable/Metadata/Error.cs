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

    public Error WithRootCause(Error error) =>
        this with
        {
            InnerErrors = InnerErrors.Add(error),
        };

    public Error WithRootCauses(IEnumerable<Error> errors) =>
        this with
        {
            InnerErrors = InnerErrors.AddRange(errors),
        };
}