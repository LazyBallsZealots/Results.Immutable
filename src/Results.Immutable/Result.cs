using Results.Immutable.Metadata;

namespace Results.Immutable;

/// <summary>
///     A structure representing the result of an operation.
/// </summary>
/// <typeparam name="T">Type of the value associated with this <see cref="Result{T}" />.</typeparam>
public readonly partial struct Result<T> : IEquatable<Result<T>>
{
    private readonly ImmutableList<Error>? errors;

    /// <summary>
    ///     The possible value of the this <see cref="Result{T}" />.
    /// </summary>
    public Some<T>? Some { get; } = null;

    /// <summary>
    ///     Initializes a new failed instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    public Result()
    {
        errors = ImmutableList.Create(new Error("Constructed result"));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="value">
    ///     A value.
    /// </param>
    internal Result(T value)
        : this(new Some<T>(value))
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="someValue">
    ///     An <see cref="Option{T}" />.
    /// </param>
    internal Result(Some<T>? someValue)
    {
        Some = someValue;
        errors = null;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="errors">
    ///     A list of <see cref="Error" />s.
    /// </param>
    internal Result(ImmutableList<Error>? errors)
    {
        this.errors = errors;
        Some = null;
    }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a failed operation.
    ///     A failed operation does not contain a value, and may contain errors.
    /// </summary>
    public bool IsErrored => Some is null;

    /// <inheritdoc cref="Result{T}.IsErrored" />
    public bool HasFailed => Some is null;

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a successful operation.
    ///     An OK operation contains a value, no errors are present.
    /// </summary>
    public bool IsOk => Some is not null;

    /// <inheritdoc cref="Result{T}.IsOk" />
    public bool HasSucceeded => Some is not null;

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of <see cref="Error" />s
    ///     associated with this <see cref="Result{T}" />.
    /// </summary>
    /// <remarks>
    ///     This list will be empty for successful results.
    /// </remarks>
    public ImmutableList<Error> Errors => errors ?? ImmutableList<Error>.Empty;

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with an <see cref="Error" />,
    ///     containing provided <paramref name="errorMessage" />.
    /// </summary>
    /// <param name="errorMessage">
    ///     Message to provide to an <see cref="Error" />.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with an <see cref="Error" />,
    ///     containing provided <paramref name="errorMessage" />.
    /// </returns>
    public Result<T> AddError(string errorMessage) => AddError(new Error(errorMessage));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with a provided <paramref name="error" />.
    /// </summary>
    /// <param name="error">
    ///     An <see cref="Error" /> to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="error" />.
    /// </returns>
    public Result<T> AddError(Error error) =>
        AddErrors(
            new[]
            {
                error,
            });

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with <see cref="Error" />s
    ///     built from provided <paramref name="errorMessages" />.
    /// </summary>
    /// <param name="errorMessages">
    ///     A collection of error messages to build <see cref="Error" />
    ///     instances from.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with <see cref="Error" />s,
    ///     built from <paramref name="errorMessages" />.
    /// </returns>
    public Result<T> AddErrors(IEnumerable<string> errorMessages) =>
        AddErrors(errorMessages.Select(static errorMessage => new Error(errorMessage)));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with provided <paramref name="newErrors" />.
    /// </summary>
    /// <param name="newErrors">
    ///     A collection of <see cref="Error" />s to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="newErrors" />.
    /// </returns>
    public Result<T> AddErrors(IEnumerable<Error> newErrors) => new(Errors.AddRange(newErrors));

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T}" /> instance contains a <typeparamref name="TError" />
    ///     matching provided <paramref name="predicate" />.
    /// </summary>
    /// <typeparam name="TError">
    ///     Generic type of the error.
    /// </typeparam>
    /// <param name="predicate">
    ///     A <see cref="Predicate{T}" /> to match.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Errors" />
    ///     match provided <paramref name="predicate" />,
    ///     otherwise - <see langword="false" />.
    /// </returns>
    public bool HasError<TError>(Func<TError, bool> predicate)
        where TError : Error =>
        Errors.OfType<TError>()
            .Any(predicate);

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T}" /> contains a <typeparamref name="TError" />.
    /// </summary>
    /// <typeparam name="TError">
    ///     Generic type of the error.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Errors" /> of the type <see cref="TError" />.
    /// </returns>
    public bool HasError<TError>()
        where TError : Error =>
        Errors.OfType<TError>()
            .Any();

    /// <summary>
    ///     Executes provided <paramref name="onOk" /> or <paramref name="onErrors" /> depending on
    ///     the success state of this <see cref="Result{T}" /> and returns the returned value of the executed function.
    /// </summary>
    /// <param name="onOk">A function to invoke when this <see cref="Result{T}" /> is successful.</param>
    /// <param name="onErrors">A function to invoke when this <see cref="Result{T}" /> is errored.</param>
    /// <typeparam name="TNew">The type of the returned value of the executed functions.</typeparam>
    /// <returns>The returned value of the executed function.</returns>
    public TNew Match<TNew>(Func<T, TNew> onOk, Func<ImmutableList<Error>, TNew> onErrors)
    {
        if (Some is var (x))
        {
            return onOk(x);
        }

        return onErrors(Errors);
    }

    /// <summary>
    ///     Executes provided <paramref name="onOk" /> or <paramref name="onErrors" /> depending on
    ///     the success state of this <see cref="Result{T}" />.
    /// </summary>
    /// <param name="onOk">An action to invoke when this <see cref="Result{T}" /> is successful.</param>
    /// <param name="onErrors">An action to invoke when this <see cref="Result{T}" /> is errored.</param>
    public void Match(Action<T> onOk, Action<ImmutableList<Error>> onErrors)
    {
        if (Some is var (x))
        {
            onOk(x);
        }
        else
        {
            onErrors(Errors);
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is Result<T> other &&
        other.Some == Some &&
        other.Errors.SequenceEqual(Errors);

    /// <inheritdoc />
    public bool Equals(Result<T> other) => Equals(other as object);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Some, Errors);

    public static bool operator ==(Result<T> left, Result<T> right) => Equals(left, right);

    public static bool operator !=(Result<T> left, Result<T> right) => !Equals(left, right);
}