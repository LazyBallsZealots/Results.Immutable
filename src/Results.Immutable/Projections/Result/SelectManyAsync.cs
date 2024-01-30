namespace Results.Immutable;

public readonly partial struct Result<T>
{
    /// <summary>
    ///     Projects the possible value of the <see cref="Result{T}" /> into a new <see cref="Result{T}" />,
    ///     flattening the structure.
    ///     If the result contains errors, the errors are propagated.
    /// </summary>
    /// <param name="selector">A transform to apply to the value of the result and the intermediate value.</param>
    /// <typeparam name="TNew">The type of the final value.</typeparam>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous operation, containing the
    ///     transformed value.
    /// </returns>
    public ValueTask<Result<TNew>> SelectManyAsync<TNew>(Func<T, ValueTask<Result<TNew>>> asyncSelector) =>
        Some is not var (v) ? new(new Result<TNew>(errors)) : asyncSelector(v);

    /// <summary>
    ///     Projects the possible value of the <see cref="Result{T}" /> into a new <see cref="Result{T}" />,
    ///     flattening the structure.
    ///     If the result contains errors, the errors are propagated. Also, errors returned by the intermediate
    ///     selector are propagated.
    /// </summary>
    /// <param name="intermediateSelector">An asynchronous transform to apply to the value of the result.</param>
    /// <param name="resultSelector">A transform to apply to the value of the result and the intermediate value.</param>
    /// <typeparam name="TIntermediate">The type of the intermediate value.</typeparam>
    /// <typeparam name="TNew">The type of the final value.</typeparam>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous operation, containing the
    ///     <see cref="Result{T}" /> with the transformed value.
    /// </returns>
    public async ValueTask<Result<TNew>> SelectManyAsync<TIntermediate, TNew>(
        Func<T, ValueTask<Result<TIntermediate>>> intermediateAsyncSelector,
        Func<T, TIntermediate, TNew> resultSelector)
    {
        if (Some is not var (value))
        {
            return new(errors);
        }

        return await intermediateAsyncSelector(value) is { } intermediateResult &&
            intermediateResult is { Some.Value: var intermediateValue, }
                ? new(resultSelector(value, intermediateValue))
                : new(intermediateResult.errors);
    }
}