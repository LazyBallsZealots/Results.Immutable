namespace Results.Immutable.Metadata;

/// <summary>
///     Represents an error caused by an exception.
/// </summary>
/// <param name="Message">
///     The exception message.
/// </param>
public sealed record ExceptionalError(Exception CausedBy, ImmutableList<Error> InnerErrors)
    : Error(
        CausedBy.Message,
        InnerErrors)
{
    public ExceptionalError(Exception exception)
        : this(exception, ImmutableList<Error>.Empty)
    {
    }

    public ExceptionalError(Exception exception, Error innerError)
        : this(exception, ImmutableList.Create(innerError))
    {
    }

}
