namespace Results.Immutable;

public readonly partial record struct Option<T>
{
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
}