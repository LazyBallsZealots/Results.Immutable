namespace Results.Immutable;

public readonly partial struct Option<T>
{
    public Option<TOut> Select<TOut>(Func<T, TOut> selector) =>
        Some is var (value) ? selector(value) : Option.None<TOut>();

    /// <summary>
    ///     Projects the possible value to a new <see cref="Option{T}" />.
    /// </summary>
    /// <param name="selector">
    ///     A transform function to apply to the possible value.
    /// </param>
    /// <typeparam name="TOut">
    ///     Generic type of the resulting <see cref="Option{T}" />.
    /// </typeparam>
    /// <returns>
    ///     An <see cref="Option{T}" />, which contains possibly the transformed value.
    /// </returns>
    public Option<TOut> SelectMany<TOut>(Func<T, Option<TOut>> selector) =>
        Some is var (value) &&
        selector(value) is { IsSome: true, } r
            ? r
            : Option.None<TOut>();

    /// <summary>
    ///     Projects the possible value to a new <see cref="Option{T}" />.
    /// </summary>
    /// <typeparam name="T1">
    ///     Generic type of the combined <see cref="Option{T}" />.
    /// </typeparam>
    /// <typeparam name="TOut">
    ///     Generic type of the resulting <see cref="Option{T}" />.
    /// </typeparam>
    /// <param name="combinator">
    ///     A transform function to apply to the possible value of this <see cref="Option{T}" />.
    /// </param>
    /// <param name="selector">
    ///     A transform function to apply to the possible value of the combined <see cref="Option{T}" />.
    /// </param>
    /// <returns>
    ///     An <see cref="Option{T}" />, which contains possibly the transformed value.
    /// </returns>
    public Option<TOut> SelectMany<T1, TOut>(
        Func<T, Option<T1>> combinator,
        Func<T, T1, TOut> selector) =>
        Some is var (value) &&
        combinator(value)
            .Some is var (value1)
            ? selector(value, value1)
            : Option.None<TOut>();

    /// <summary>
    ///     Filters the possible value by a predicate.
    /// </summary>
    /// <param name="predicate">A function to test each possible value.</param>
    /// <returns>
    ///     An <see cref="Option{T}" /> with the possible value if the predicate is true,
    ///     otherwise it is returned as empty.
    /// </returns>
    public Option<T> Where(Func<T, bool> predicate) =>
        Some is var (value) && predicate(value) ? this : Option.None<T>();
}