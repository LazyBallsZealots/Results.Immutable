namespace Results.Immutable;

public readonly partial struct Result<T>
{
    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="selector" />
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="selector">Bind delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Result{T}" />, wrapping <typeparamref name="TNew" />
    ///     instance.
    /// </returns>
    /// <remarks>
    ///     This overload disregards the <see cref="OptionFactories" /> of this
    ///     <see cref="Result{T}" />.
    /// </remarks>
    public Result<TNew> Select<TNew>(Func<T, TNew> selector) =>
        Some is var (v) ? new(selector(v)) : new Result<TNew>(errors);
}