namespace Results.Immutable;

/// <summary>
///     A specialized <see cref="Error" /> that is originated from an <see cref="Exception" />.
///     It is useful for wrapping a .NET <see cref="Exception" /> into an <see cref="Error" />.
/// </summary>
/// <param name="CausedBy">The <see cref="Exception" /> that caused this <see cref="ExceptionalError" />.</param>
/// <param name="InnerErrors">Another <see cref="Error" /> that also caused this <see cref="ExceptionalError" />.</param>
/// <returns></returns>
public sealed record ExceptionalError(Exception CausedBy, ImmutableList<Error> InnerErrors)
    : Error(
        CausedBy.Message,
        InnerErrors)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ExceptionalError" /> class.
    /// </summary>
    /// <param name="exception">The <see cref="Exception" /> that caused this <see cref="ExceptionalError" />.</param>
    public ExceptionalError(Exception exception)
        : this(exception, ImmutableList<Error>.Empty)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ExceptionalError" /> class.
    /// </summary>
    /// <param name="exception">The <see cref="Exception" /> that caused this <see cref="ExceptionalError" />.</param>
    /// <param name="innerError">A <see cref="Error" /> that also caused this <see cref="ExceptionalError" />.</param>
    public ExceptionalError(Exception exception, Error innerError)
        : this(exception, ImmutableList.Create(innerError))
    {
    }
}