using System.Text.Json.Serialization;

namespace Results.Immutable;

/// <summary>
///     A piece of information that describes an expected failure in an operation.
/// </summary>
public record Error
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> class.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="innerErrors">A list of errors that caused this error.</param>
    [JsonConstructor]
    public Error(string message, ImmutableList<Error> innerErrors)
    {
        Message = message;
        InnerErrors = innerErrors;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> class.
    /// </summary>
    /// <param name="message">A message that describes the <see cref="Error" />.</param>
    /// <param name="innerError">Another <see cref="Error" /> that caused this <see cref="Error" /> to occur.</param>
    public Error(string message, Error innerError)
        : this(
            message,
            ImmutableList.Create(innerError))
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> class.
    /// </summary>
    /// <param name="message">A message that describes the <see cref="Error" />.</param>
    public Error(string message)
        : this(
            message,
            ImmutableList<Error>.Empty)
    {
    }

    /// <summary>
    ///     Gets the error message.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    ///     Gets the list of errors causing this one to occur.
    /// </summary>
    public ImmutableList<Error> InnerErrors { get; init; }

    /// <summary>
    ///     With a new <see cref="Error" /> that will be added to the existing list of inner <see cref="Error" />s.
    /// </summary>
    /// <param name="error">Another error that caused this error to occur.</param>
    public Error WithRootCause(Error error) =>
        this with
        {
            InnerErrors = InnerErrors.Add(error),
        };

    /// <summary>
    ///     With new <see cref="Error" />s that will be added to the existing list of inner <see cref="Error" />s.
    /// </summary>
    /// <param name="errors">
    ///     A collection of errors that caused this error to occur.
    /// </param>
    public Error WithRootCauses(IEnumerable<Error> errors) =>
        this with
        {
            InnerErrors = InnerErrors.AddRange(errors),
        };
}

/// <summary>
///     A piece of information that describes an expected failure in an operation.
///     This class is intended to be used as base class for new error types.
/// </summary>
/// <typeparam name="TError">The type of the inheriting error.</typeparam>
/// <inheritdoc />
public abstract record Error<TError>(
    string Message,
    ImmutableList<Error> InnerErrors) : Error(Message, InnerErrors)
    where TError : Error<TError>
{
    /// <summary>
    ///     With a new <see cref="Error" /> that will be added to the existing list of inner <see cref="Error" />s.
    /// </summary>
    /// <param name="error">A new message that describes the <see cref="Error" />.</param>
    public new TError WithRootCause(Error error) =>
        (TError)this with
        {
            InnerErrors = InnerErrors.Add(error),
        };

    /// <summary>
    ///     With new <see cref="Error" />s that will be added to the existing list of inner <see cref="Error" />s.
    /// </summary>
    /// <param name="errors">
    ///     A collection of new <see cref="Error" />s that will be added to the existing list of inner
    ///     <see cref="Error" />s.
    /// </param>
    public new TError WithRootCauses(IEnumerable<Error> errors) =>
        (TError)this with
        {
            InnerErrors = InnerErrors.AddRange(errors),
        };
}