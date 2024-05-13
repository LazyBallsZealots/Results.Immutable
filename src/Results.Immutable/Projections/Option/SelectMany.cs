namespace Results.Immutable;

public readonly partial record struct Option<T>
{
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
            ? Option.Some(selector(value, value1))
            : Option.None<TOut>();

    /// <summary>
    ///     Projects the possible value of the <see cref="Option{T}" /> into a new <see cref="Option{T}" />,
    ///     flattening the structure.
    ///     If the option contains errors, the errors are propagated.
    /// </summary>
    /// <param name="selector">A transform to apply to the value of the option and the intermediate value.</param>
    /// <typeparam name="TNew">The type of the final value.</typeparam>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the option of an asynchronous operation, containing the
    ///     transformed value.
    /// </returns>
    public ValueTask<Option<TNew>> SelectManyAsync<TNew>(Func<T, ValueTask<Option<TNew>>> asyncSelector) =>
        Some is var (v) ? asyncSelector(v) : new(new Option<TNew>());

    /// <summary>
    ///     Projects the possible value of the <see cref="Option{T}" /> into a new <see cref="Option{T}" />,
    ///     flattening the structure.
    ///     If the option contains errors, the errors are propagated. Also, errors returned by the intermediate
    ///     selector are propagated.
    /// </summary>
    /// <param name="intermediateSelector">An asynchronous transform to apply to the value of the option.</param>
    /// <param name="optionSelector">A transform to apply to the value of the option and the intermediate value.</param>
    /// <typeparam name="TIntermediate">The type of the intermediate value.</typeparam>
    /// <typeparam name="TNew">The type of the final value.</typeparam>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the option of an asynchronous operation, containing the
    ///     <see cref="Option{T}" /> with the transformed value.
    /// </returns>
    public async ValueTask<Option<TNew>> SelectManyAsync<TIntermediate, TNew>(
        Func<T, ValueTask<Option<TIntermediate>>> intermediateAsyncSelector,
        Func<T, TIntermediate, TNew> optionSelector)
    {
        if (Some is not var (value))
        {
            return new();
        }

        return await intermediateAsyncSelector(value) is { } intermediateOption &&
            intermediateOption is { Some.Value: var intermediateValue, }
                ? new(optionSelector(value, intermediateValue))
                : new();
    }

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