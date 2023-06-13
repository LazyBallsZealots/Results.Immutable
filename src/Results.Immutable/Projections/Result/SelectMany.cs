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
    /// <returns>A result contained the transformed value.</returns>
    public Result<TNew> SelectMany<TNew>(Func<T, Result<TNew>> selector) =>
        Some is not var (v) ? new(errors) : selector(v);

    /// <summary>
    ///     Projects the possible value of the <see cref="Result{T}" /> into a new <see cref="Result{T}" />,
    ///     flattening the structure.
    ///     If the result contains errors, the errors are propagated. Also, errors returned by the intermediate
    ///     selector are propagated.
    /// </summary>
    /// <param name="intermediateSelector">A transform to apply to the value of the result.</param>
    /// <param name="resultSelector">A transform to apply to the value of the result and the intermediate value.</param>
    /// <typeparam name="TIntermediate">The type of the intermediate value.</typeparam>
    /// <typeparam name="TNew">The type of the final value.</typeparam>
    /// <returns>A result contained the transformed value.</returns>
    public Result<TNew> SelectMany<TIntermediate, TNew>(
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TNew> resultSelector)
    {
        if (Some is not var (value))
        {
            return new(errors);
        }

        var intermediateResult = intermediateSelector(value);
        return intermediateResult.Some is not var (intermediate)
            ? new(intermediateResult.errors)
            : new(resultSelector(value, intermediate));
    }
}