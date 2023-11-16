namespace Results.Immutable;

/// <summary>
///     A value representing the result of an operation.
///     The usage of <see cref="Result{T}" /> is generally recommended.
///     This interface is only useful for boxing the <see cref="Result{T}" />
///     in a way that makes it content inaccessible.
/// </summary>
public interface IResult
{
    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="IResult{T}" />
    ///     represents a failed operation.
    ///     A failed operation does not contain a value, and may contain errors.
    /// </summary>
    public bool IsErrored { get; }

    /// <inheritdoc cref="IsErrored" />
    public bool HasFailed { get; }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="IResult{T}" />
    ///     represents a successful operation.
    ///     An OK operation contains a value, no errors are present.
    /// </summary>
    public bool IsOk { get; }

    /// <inheritdoc cref="IsOk" />
    public bool HasSucceeded { get; }

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of <see cref="Error" />s
    ///     associated with this <see cref="IResult{T}" />.
    /// </summary>
    /// <remarks>
    ///     This list will be empty for successful results.
    /// </remarks>
    public ImmutableList<Error> Errors { get; }

    /// <summary>
    ///     Creates a new failed <see cref="IResult" /> with the provided <paramref name="errors" />.
    ///     This is the same as <see cref="Result.Fail(IEnumerable{Error})" /> but losing the type information.
    /// </summary>
    /// <remarks>
    ///     It must return an object of the same type. It is safe to downcast.
    /// </remarks>
    /// <param name="errors">The new <see cref="ImmutableList{T}" /> of <see cref="Error" />s.</param>
    IResult WithErrors(ImmutableList<Error> errors);
}