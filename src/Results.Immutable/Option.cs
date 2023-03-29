namespace Results.Immutable;

/// <summary>
///     Represents an optional value.
/// </summary>
public readonly record struct Option
{
    /// <summary>
    ///     Represents the <paramref name="value" />
    ///     as an option.
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
}

public static class OptionExt
{
    public static T? Get<T>(this Option<T> option) where T : class
    {
        return option.ValueOrDefault;
    }
    
    public static T? GetValue<T>(this Option<T> option) where T : struct
    {
        return option.IsSome ? option.ValueOrDefault : null;
    }
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
        _value = value;
        IsSome = true;
    }
    
    public Some<T>? Some => IsSome ? new Some<T>(_value) : null;

    /// <summary>
    ///     Gets the <see langword="bool" /> indicator,
    ///     which states whether this <see cref="Option{T}" /> wraps an actual value.
    /// </summary>
    /// <seealso cref="IsNone" />
    public bool IsSome { get; } = false;

    /// <summary>
    ///     Gets the <see langword="bool" /> indicator,
    ///     which states whether this <see cref="Option{T}" /> does not have any value.
    /// </summary>
    /// <seealso cref="IsSome" />
    public bool IsNone => !IsSome;

    /// <summary>
    ///     Gets the value associated with this <see cref="Option{T}" />.
    /// </summary>
    private readonly T _value = default!;

    public static implicit operator Option<T>(T value) => new(value);

    public T? ValueOrDefault => IsSome ? _value : default;

    public T GetOr(T @default) => IsSome ? _value : @default;
    
    public T GetOrElse(Func<T> fn) => IsSome ? _value : fn();

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
    public TResult Match<TResult>(Func<T?, TResult> matchSome, Func<TResult> matchNone) =>
        IsSome ? matchSome(_value) : matchNone();

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
    public void Match(Action<T?> matchSome, Action matchNone)
    {
        if (IsSome)
        {
            matchSome(_value);
        }
        else
        {
            matchNone();
        }
    }
}
}