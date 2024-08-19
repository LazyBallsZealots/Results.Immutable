using System.Text.Json.Serialization;

namespace Results.Immutable;

/// <summary>
///     A specialized <see cref="Error" /> that is originated from an <see cref="Exception" />.
///     It is useful for wrapping a .NET <see cref="Exception" /> into an <see cref="Error" />.
/// </summary>
public sealed record ExceptionalError : Error
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ExceptionalError" /> class.
    /// </summary>
    /// <param name="causedBy">The <see cref="Exception" /> that caused this error.</param>
    /// <param name="innerErrors">A list of errors that caused this error.</param>
    [JsonConstructor]
    public ExceptionalError(Exception causedBy, ImmutableList<Error> innerErrors)
        : base(causedBy.Message, innerErrors)
    {
        CausedBy = causedBy;
    }

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

    /// <summary>
    ///     Gets the <see cref="Exception" /> that caused this <see cref="ExceptionalError" />.
    /// </summary>
    public Exception CausedBy { get; init; }
}