namespace Results.Immutable;

public readonly partial struct Result<T>
{
    /// <summary>
    ///     Filters the possible value by a predicate, returning a failed result if the predicate is false,
    ///     otherwise the original <see cref="Result{T}" /> is returned.
    /// </summary>
    /// <param name="predicate">A function to test the <see cref="Result{T}.Some" /> value if available.</param>
    /// <param name="error">The error to return if the predicate is false.</param>
    /// <returns>
    ///     The result after the filtering.
    /// </returns>
    public Result<T> Where(Func<T, bool> predicate, Error error) =>
        (Some is var (value) && predicate(value)) || Some is null ? this : new(ImmutableList.Create(error));

    /// <summary>
    ///     Filters the possible value by a predicate, returning a failed result if the predicate is false,
    ///     otherwise the original <see cref="Result{T}" /> is returned.
    /// </summary>
    /// <param name="predicate">A function to test the <see cref="Result{T}.Some" /> value if available.</param>
    /// <param name="error">A function to create the error to return if the predicate is false.</param>
    /// <returns>
    ///     The result after the filtering.
    /// </returns>
    public Result<T> Where(Func<T, bool> predicate, Func<Error> errorFactory) =>
        (Some is var (value) && predicate(value)) || Some is null ? this : new(ImmutableList.Create(errorFactory()));

    /// <summary>
    ///     Filters the possible value by a predicate, returning a failed result if the predicate is false,
    ///     otherwise the original <see cref="Result{T}" /> is returned.
    /// </summary>
    /// <param name="predicate">A function to test the <see cref="Result{T}.Some" /> value if available.</param>
    /// <param name="error">The error to return if the predicate is false.</param>
    /// <returns>
    ///     A ValueTask containing filtered result.
    /// </returns>
    public async ValueTask<Result<T>> WhereAsync(Func<T, ValueTask<bool>> predicate, Error error) =>
        (Some is var (value) && await predicate(value)) || Some is null ? this : new(ImmutableList.Create(error));

    /// <summary>
    ///     Filters the possible value by a predicate, returning a failed result if the predicate is false,
    ///     otherwise the original <see cref="Result{T}" /> is returned.
    /// </summary>
    /// <param name="predicate">A function to test the <see cref="Result{T}.Some" /> value if available.</param>
    /// <param name="error">A function to create the error to return if the predicate is false.</param>
    /// <returns>
    ///     A ValueTask containing filtered result.
    /// </returns>
    public async ValueTask<Result<T>> WhereAsync(Func<T, ValueTask<bool>> predicate, Func<Error> errorFactory) =>
        (Some is var (value) && await predicate(value)) || Some is null
            ? this
            : new(ImmutableList.Create(errorFactory()));
}