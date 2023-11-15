using Results.Immutable.Metadata;

namespace Results.Immutable;

public interface IResult
{
    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="IResult{T}" />
    ///     represents a failed operation.
    ///     A failed operation does not contain a value, and may contain errors.
    /// </summary>
    public bool IsErrored { get; }

    /// <inheritdoc cref="IResult{T}.IsErrored" />
    public bool HasFailed { get; }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="IResult{T}" />
    ///     represents a successful operation.
    ///     An OK operation contains a value, no errors are present.
    /// </summary>
    public bool IsOk { get; }

    /// <inheritdoc cref="IResult{T}.IsOk" />
    public bool HasSucceeded { get; }

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of <see cref="Error" />s
    ///     associated with this <see cref="IResult{T}" />.
    /// </summary>
    /// <remarks>
    ///     This list will be empty for successful results.
    /// </remarks>
    public ImmutableList<Error> Errors { get; }
}

/// <summary>
///     A structure representing the result of an operation.
///     The usage of <see cref="Result{T}" /> is generally recommended.
///     This interface is only useful for boxing the <see cref="IResult{T}" />.
/// </summary>
/// <typeparam name="T">Type of the value associated with this <see cref="IResult{T}" />.</typeparam>
public interface IResult<T> : IResult
{
    /// <summary>
    ///     The possible value of the this <see cref="IResult{T}" />.
    /// </summary>
    public Some<T>? Some { get; }
}