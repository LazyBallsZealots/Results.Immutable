namespace Results.Immutable;

public readonly partial struct Result<T>
{
    /// <summary>
    ///     Projects all errors associated with this <see cref="Result{T}" />
    ///     using provided <paramref name="errorSelector" />.
    /// </summary>
    /// <param name="errorSelector">Selector used for <see cref="Errors" /> projection.</param>
    /// <returns>
    ///     The same result if it <see cref="HasSucceeded" />, otherwise -
    ///     new <see cref="Result{T}" /> with transformed <see cref="Errors" />.
    /// </returns>
    public Result<T> SelectErrors(Func<Error, Error> errorSelector) =>
        this is { Some.Value: _, } ? this : new(ImmutableList.CreateRange(Errors.Select(errorSelector)));

    /// <summary>
    ///     Zips all errors associated with this <see cref="Result{T}" />
    ///     into one error using provided <paramref name="errorsCombinator" />.
    /// </summary>
    /// <typeparam name="TError">Generic type of the projected error.</typeparam>
    /// <param name="errorsCombinator">Combinator of the <see cref="Errors" />.</param>
    /// <returns>
    ///     The same result if it <see cref="HasSucceeded" />, otherwise -
    ///     new <see cref="Result{T}" /> with errors flattened into one.
    /// </returns>
    /// <remarks>
    ///     It is strongly recommended to leverage
    ///     <see cref="Error.WithRootCauses(IEnumerable{Error})" /> in the
    ///     <paramref name="errorsCombinator" />.
    /// </remarks>
    public Result<T> MergeErrorsWith<TError>(Func<ImmutableList<Error>, TError> errorsCombinator)
        where TError : Error =>
        this is { Some.Value: _, } ? this : new(ImmutableList.Create<Error>(errorsCombinator(Errors)));
}