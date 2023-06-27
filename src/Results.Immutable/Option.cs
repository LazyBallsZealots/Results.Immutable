using System.Runtime.CompilerServices;

namespace Results.Immutable;

/// <typeparam name="T">Generic type of the value.</typeparam>
/// <inheritdoc cref="OptionFactories" />
public readonly partial struct Option<T>: IEquatable<Option<T>>
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
    ///     In case of <see cref="Option{T}.IsNone" />, returns the default value of <typeparamref name="T" />.
    ///     This property is <strong>unsafe</strong> to use then <typeparamref name="T" /> is a nullable reference type,
    ///     and non-nullable value types.
    ///     It is not possible to determine whether the value is null or None.
    /// </summary>
    /// <example>
    ///     This is a safe usage.
    ///     <code>
    ///         var option = Option.Some("hello");
    ///         option.ValueOrDefault; // "hello"
    ///     </code>
    /// </example>
    /// <example>
    ///     These are not a safe usage.
    ///     <code>
    ///         var option = Option.Some&lt;string&gt;(null);
    ///         option.ValueOrDefault; // null, same as Option.None&lt;string&gt;()
    ///         var option2 = Option.None&lt;int&gt;();
    ///         option2.ValueOrDefault; // 0
    ///     </code>
    /// </example>
    /// <value>The value associated with this <see cref="Option{T}" /> or default value of <typeparamref name="T" />.</value>
    public T? ValueOrDefault => Some is var (value) ? value : default;

    public static implicit operator Option<T>(T value) => new(value);

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Option<T> other) => Nullable.Equals(Some, other.Some);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

    public override int GetHashCode() => Some.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);
}