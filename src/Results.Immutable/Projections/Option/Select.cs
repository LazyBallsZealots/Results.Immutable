namespace Results.Immutable;

public readonly partial record struct Option<T>
{
    /// <summary>
    ///     Projects this <see cref="Option{T}" />
    ///     to an <see cref="Option{T}" /> of <typeparamref name="TOut" />
    ///     using provided <paramref name="selector" />.
    /// </summary>
    /// <typeparam name="TOut">Generic type of the resulting <see cref="Option{T}" />.</typeparam>
    /// <param name="selector">Value selector.</param>
    /// <returns>
    ///     An <see cref="Option.Some{T}(T)" /> of <typeparamref name="TOut" />
    ///     if this <see cref="Option{T}.IsSome" />, <see cref="Option.None{T}" />
    ///     of <typeparamref name="TOut" /> otherwise.
    /// </returns>
    public Option<TOut> Select<TOut>(Func<T, TOut> selector) =>
        Some is var (value) ? Option.Some(selector(value)) : Option.None<TOut>();

    /// <summary>
    ///     Asynchronously projects this <see cref="Option{T}" />
    ///     into an <see cref="Option{T}" /> of <typeparamref name="TOut" />
    ///     using provided <paramref name="asyncSelector" />.
    /// </summary>
    /// <typeparam name="TOut">Generic type of the resulting <see cref="Option{T}" />.</typeparam>
    /// <param name="asyncSelector">
    ///     Async value selector.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous operation,
    ///     wrapping an <see cref="Option{T}" /> of <typeparamref name="TOut" />; returns
    ///     <see cref="Option.Some{T}(T)" /> if this <see cref="Option{T}.IsSome" />,
    ///     <see cref="Option.None{T}" /> otherwise.
    /// </returns>
    public async ValueTask<Option<TOut>> SelectAsync<TOut>(Func<T, ValueTask<TOut>> asyncSelector) =>
        this is { Some.Value: var value, } ? Option.Some(await asyncSelector(value)) : Option.None<TOut>();
}