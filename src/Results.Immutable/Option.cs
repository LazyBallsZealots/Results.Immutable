namespace Results.Immutable;

/// <summary>
///     Represents an optional value.
/// </summary>
public readonly record struct Option
{
    /// <summary>
    ///     Represents the <paramref name="value" /> as an option.
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="value">Value to associate with the option/</param>
    /// <returns>A new instance of <see cref="Immutable.Option{T}" /> with a value.</returns>
    public static Option<T> Some<T>(T value) => new(value);

    /// <summary>
    ///     Represents the lack of value.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <returns>
    ///     A new instance of <see cref="Immutable.Option{T}" /> without value.
    /// </returns>
    public static Option<T> None<T>() => new();

    /// <summary>
    ///     Creates an <see cref="Option{T}" /> from a nullable reference.
    ///     In case of <see langword="null" />, returns <see cref="None{T}" />,
    ///     otherwise returns <see cref="Some{T}" />.
    /// </summary>
    /// <param name="value">A nullable reference.</param>
    /// <typeparam name="T">Type of the reference.</typeparam>
    /// <returns>The new <see cref="Option{T}" /> where the <typeparamref name="T" /> is no longer nullable.</returns>
    public static Option<T> SomeIfNotNull<T>(T? value)
        where T : class =>
        value is not null ? Some(value) : None<T>();

    /// <summary>
    ///     Creates an <see cref="Option{T}" /> from a <see cref="Nullable{T}" /> value.
    ///     In case of <see langword="null" />, returns <see cref="None{T}" />,
    ///     otherwise returns <see cref="Some{T}" />.
    /// </summary>
    /// <typeparam name="T">A type of the value.</typeparam>
    /// <param name="value">The nullable value.</param>
    /// <returns>The new <see cref="Option{T}" /> where the <typeparamref name="T" /> is no longer nullable.</returns>
    public static Option<T> SomeIfNotNull<T>(T? value)
        where T : struct =>
        value is not null ? Some(value.Value) : None<T>();
}

/// <typeparam name="T">Generic type of the value.</typeparam>
/// <inheritdoc cref="Option" />
public readonly partial record struct Option<T>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Option{T}" /> struct.
    /// </summary>
    /// <param name="value">Value to wrap as an option.</param>
    internal Option(T value)
    {
        Some = new Some<T>(value);
    }

    public Some<T>? Some { get; }

    /// <summary>
    ///     Gets the <see langword="bool" /> indicator,
    ///     which states whether this <see cref="Option{T}" /> wraps an actual value.
    /// </summary>
    /// <seealso cref="IsNone" />
    public bool IsSome => Some.HasValue;

    /// <summary>
    ///     Gets the <see langword="bool" /> indicator,
    ///     which states whether this <see cref="Option{T}" /> does not have any value.
    /// </summary>
    /// <seealso cref="IsSome" />
    public bool IsNone => !Some.HasValue;

    /// <summary>
    ///     Gets the value associated with this <see cref="Option{T}" />.
    ///     In case of <see cref="Option{T}.IsNone" />, returns <paramref name="fallback" />.
    /// </summary>
    /// <param name="fallback">A value to return in case of <see cref="Option{T}.IsNone" />.</param>
    /// <returns></returns>
    public T GetValueOr(T fallback) => Some is var (value) ? value : fallback;

    /// <summary>
    ///     Gets the value associated with this <see cref="Option{T}" />.
    ///     In case of <see cref="Option{T}.IsNone" />, calls <paramref name="fn" /> and returns its result.
    /// </summary>
    /// <param name="fn">A functions that returns a value in case of <see cref="Option{T}.IsNone" />.</param>
    /// <returns></returns>
    public T GetValueOrElse(Func<T> fn) => Some is var (value) ? value : fn();

    /// <summary>
    ///     Returns <paramref name="other" />, if this <see cref="Option{T}" /> has a value.
    ///     Otherwise, returns an empty <see cref="Option{T}" />.
    /// </summary>
    /// <param name="other"></param>
    /// <typeparam name="TOther"></typeparam>
    /// <returns></returns>
    public Option<TOther> And<TOther>(Option<TOther> other) => Some.HasValue ? other : Option.None<TOther>();

    /// <summary>
    ///     Returns this <see cref="Option{T}" />, if this <see cref="Option{T}" /> has a value.
    ///     Otherwise, returns <paramref name="other" />.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Option<T> Or(Option<T> other) => Some.HasValue ? this : other;

    /// <summary>
    ///     Projects the value of this <see cref="Option{T}" />
    ///     to a <typeparamref name="TResult" />.
    /// </summary>
    /// <typeparam name="TResult">Generic type of the match.</typeparam>
    /// <param name="matchSome">
    ///     A delegate accepting <typeparamref name="T" />,
    ///     which will be executed if this <see cref="Option{T}.IsSome" />
    ///     is <see langword="true" />.
    /// </param>
    /// <param name="matchNone">
    ///     A delegate which will be executed if
    ///     this <see cref="Option{T}.IsNone" /> is <see langword="true" />.
    /// </param>
    /// <returns>
    ///     An instance of <typeparamref name="TResult" />,
    ///     obtained by executing either <paramref name="matchSome" />
    ///     or <paramref name="matchNone" /> delegates.
    /// </returns>
    public TResult Match<TResult>(Func<T, TResult> matchSome, Func<TResult> matchNone) =>
        Some is var (value) ? matchSome(value) : matchNone();

    /// <summary>
    ///     Executes either <paramref name="matchSome" />
    ///     or <paramref name="matchNone" />, depending on whether
    ///     this <see cref="Option{T}.IsSome" /> or <see cref="Option{T}.IsNone" />.
    /// </summary>
    /// <param name="matchSome">
    ///     A delegate accepting <typeparamref name="T" />,
    ///     which will be executed if this <see cref="Option{T}.IsSome" />
    ///     is <see langword="true" />.
    /// </param>
    /// <param name="matchNone">
    ///     A delegate which will be executed if
    ///     this <see cref="Option{T}.IsNone" /> is <see langword="true" />.
    /// </param>
    public void Match(Action<T> matchSome, Action matchNone)
    {
        if (Some is var (value))
        {
            matchSome(value);
        }
        else
        {
            matchNone();
        }
    }

    /// <summary>
    ///     Returns a string representation of this <see cref="Option{T}" />.
    ///     Ideally, only for debug purposes or testing purposes.
    /// </summary>
    public override string ToString() =>
        Some is var (value)
            ? $"{nameof(Option)}<{typeof(T).Name}>.{nameof(Some)}({value})"
            : $"{nameof(Option)}<{typeof(T).Name}>.{nameof(Option.None)}";
}