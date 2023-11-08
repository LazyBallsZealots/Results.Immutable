namespace Results.Immutable;

/// <summary>
///     A piece of information that describes an expected failure in an operation.
/// </summary>
public record Error(
    string Message,
    ImmutableList<Error> InnerErrors)
{
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
    ///     Creates a new <see cref="Error" /> with provided <paramref name="message" />.
    /// </summary>
    /// <param name="message">The message that describes the <see cref="Error" />.</param>
    public static Error Create(string message) => new(message);

    /// <summary>
    ///     With a new <see cref="Error" /> that will be added to the existing list of inner <see cref="Error" />s.
    /// </summary>
    /// <param name="message">A new message that describes the <see cref="Error" />.</param>
    public Error WithRootCause(Error error) =>
        this with
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
    public Error WithRootCauses(IEnumerable<Error> errors) =>
        this with
        {
            InnerErrors = InnerErrors.AddRange(errors),
        };
}