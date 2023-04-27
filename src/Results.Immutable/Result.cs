using Results.Immutable.Metadata;

namespace Results.Immutable;

/// <summary>
///     A structure representing the result of an operation.
/// </summary>
/// <typeparam name="T">Type of the value associated with this <see cref="Result{T}" />.</typeparam>
public readonly partial struct Result<T> : IEquatable<Result<T>>
{
    private readonly ImmutableList<Error>? errors;
    private readonly ImmutableList<Success>? successes;

    private readonly Some<T>? some;

    public Some<T>? Some => some;

    /// <summary>
    /// Initializes a new failed instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    public Result()
    {
        errors = ImmutableList.Create(new Error("Constructed result"));
        some = null;
        successes = null;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="option">
    ///     A value.
    /// </param>
    /// <param name="successes">
    ///     A list of <see cref="Success" />s.
    /// </param>
    internal Result(
        T value,
        ImmutableList<Success>? successes) : this(new Some<T>(value), successes)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="someValue">
    ///     An <see cref="Option{T}" />.
    /// </param>
    /// <param name="successes">
    ///     A list of <see cref="Success" />s.
    /// </param>
    internal Result(
        Some<T>? someValue,
        ImmutableList<Success>? successes)
    {
        this.errors = null;
        this.successes = successes;
        this.some = someValue;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="errors">
    ///     A list of <see cref="Error" />s.
    /// </param>
    /// <param name="successes">
    ///     A list of <see cref="Success" />s.
    /// </param>
    internal Result(
        ImmutableList<Error>? errors,
        ImmutableList<Success>? successes)
    {
        this.errors = errors;
        this.successes = successes;
        this.some = null;
    }

    private Result(
        Some<T>? some,
        ImmutableList<Error>? errors,
        ImmutableList<Success>? successes)
    {
        this.errors = errors;
        this.successes = successes;
        this.some = some;
    }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a failed operation.
    /// 
    ///     A failed operation does not contain a value, and may contain errors.
    /// </summary>
    public bool IsErrored => some is null;

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a successful operation.
    /// 
    ///     An OK operation contains a value, no errors are present.
    /// </summary>
    public bool IsOk => some is not null;

    /// <inheritdoc />
    public ImmutableList<Error> Errors => errors ?? ImmutableList<Error>.Empty;

    /// <inheritdoc />
    public ImmutableList<Success> Successes => successes ?? ImmutableList<Success>.Empty;

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

    ///     Creates a new <see cref="Result{T}" /> with a provided <paramref name="error" />.
    /// </summary>
    /// <param name="error">
    ///     An <see cref="Error" /> to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="error" />.
    /// </returns>
    public Result<T> AddError(Error error) => AddErrors(new[] { error });

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
        AddErrors(errorMessages.Select(errorMessage => new Error(errorMessage)));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with provided <paramref name="errors" />.
    /// </summary>
    /// <param name="errors">
    ///     A collection of <see cref="Error" />s to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="errors" />.
    /// </returns>
    public Result<T> AddErrors(IEnumerable<Error> errors) => new Result<T>(Errors.AddRange(errors), null);

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with a provided <paramref name="success" />.
    /// </summary>
    /// <param name="success">
    ///     A <see cref="Success" /> to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="success" />.
    /// </returns>
    public Result<T> AddSuccess(Success success) => AddSuccesses(new[] { success });

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with provided <paramref name="successes" />.
    /// </summary>
    /// <param name="successes">
    ///     A collection of <see cref="Success" />es to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="successes" />.
    /// </returns>
    public Result<T> AddSuccesses(IEnumerable<Success> successes) =>
        new Result<T>(some, Errors, Successes.AddRange(successes));

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
        Errors.OfType<TError>().Any(predicate);

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T} /> contains a <typeparamref name="TError" />.
    /// </summary>
    /// <typeparam name="TError">
    ///     Generic type of the error.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Errors" />
    ///     match provided <paramref name="predicate" />,
    ///     otherwise - <see langword="false" />.
    /// </returns>
    public bool HasError<TError>()
        where TError : Error =>
        Errors.OfType<TError>().Any();

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T}" /> instance contains a <typeparamref name="TSuccess" />
    ///     matching provided <paramref name="predicate" />.
    /// </summary>
    /// <typeparam name="TSuccess">
    ///     Generic type of the success.
    /// </typeparam>
    /// <param name="predicate">
    ///     A <see cref="Predicate{T}" /> to match.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Successes" />
    ///     match provided <paramref name="predicate" />,
    ///     otherwise - <see langword="false" />.
    /// </returns>
    public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate)
        where TSuccess : Success =>
        Successes.OfType<TSuccess>().Any(predicate);

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T}" /> contains a <typeparamref name="TSuccess" />.
    /// </summary>
    /// <typeparam name="TSuccess">
    ///     Generic type of the success.
    /// </typeparam>
    /// <param name="predicate">
    ///     A <see cref="Predicate{T}" /> to match.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Successes" />
    ///     match provided <paramref name="predicate" />,
    ///     otherwise - <see langword="false" />.
    /// </returns>
    public bool HasSuccess<TSuccess>()
        where TSuccess : Success =>
        Successes.OfType<TSuccess>().Any();


    public TNew Match<TNew>(Func<T, TNew> onOk, Func<ImmutableList<Error>, TNew> onError)
    {
        if (some is var (x))
        {
            return onOk(x);
        }
        else
        {
            return onError(Errors);
        }
    }

    public TNew Match<TNew>(Func<T, ImmutableList<Success>, TNew> onOk, Func<ImmutableList<Error>, ImmutableList<Success>, TNew> onError)
    {
        if (some is var (x))
        {
            return onOk(x, Successes);
        }
        else
        {
            return onError(Errors, Successes);
        }
    }

    public void Match(Action<T> onOk, Action<ImmutableList<Error>> onError)
    {
        if (some is var (x))
        {
            onOk(x);
        }
        else
        {
            onError(Errors);
        }
    }

    public void Match(Action<T, ImmutableList<Success>> onOk, Action<ImmutableList<Error>, ImmutableList<Success>> onError)
    {
        if (some is var (x))
        {
            onOk(x, Successes);
        }
        else
        {
            onError(Errors, Successes);
        }
    }

    public override bool Equals(object? obj) => obj is Result<T> other &&
            other.some == some &&
            other.Errors.SequenceEqual(Errors) &&
            other.Successes.SequenceEqual(Successes);

    public bool Equals(Result<T> other) => Equals(other as object);

    public override int GetHashCode() => HashCode.Combine(some, Errors, Successes);

    public static bool operator ==(Result<T> left, Result<T> right) => Equals(left, right);

    public static bool operator !=(Result<T> left, Result<T> right) => !Equals(left, right);
}