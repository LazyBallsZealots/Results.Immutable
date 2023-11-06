namespace Results.Immutable.Extensions;

/// <summary>
///     Contains extension methods for <see cref="Task{T}" />
///     of <see cref="Result{T}" />.
/// </summary>
public static class TaskOfResultExtensions
{
    /// <summary>
    ///     Projects this <paramref name="task" />'s
    ///     <see cref="Result{T}" /> into a <see cref="Result{T}" />
    ///     of <typeparamref name="TNew" /> by applying provided
    ///     <paramref name="selector" />.
    /// </summary>
    /// <typeparam name="T">Generic type of the <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNew">Generic type of the new <see cref="Result{T}" />.</typeparam>
    /// <param name="task">
    ///     A <see cref="Task{T}" /> of <see cref="Result{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="selector">
    ///     Delegate to execute.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous operation,
    ///     wrapping the <see cref="Result{T}" /> of <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> Select<T, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, TNew> selector) =>
        await task is var result && result is { Some.Value: var value, }
            ? new(selector(value))
            : Result.Fail<TNew>(result.Errors);

    /// <summary>
    ///     Projects the <see cref="Result{T}" />
    ///     of this <paramref name="task" />
    ///     using a provided <paramref name="asyncResultSelector" />,
    ///     flattening the structure.
    /// </summary>
    /// <typeparam name="T">Generic type of the <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNew">Generic type of the new <see cref="Result{T}" />.</typeparam>
    /// <param name="task">
    ///     A <see cref="Task{T}" /> of <see cref="Result{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="asyncResultSelector">
    ///     Delegate to execute.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous operation,
    ///     wrapping the <see cref="Result{T}" /> of <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> SelectMany<T, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, ValueTask<Result<TNew>>> asyncResultSelector) =>
        await task is var result && result is { Some.Value: var value, }
            ? await asyncResultSelector(value)
            : Result.Fail<TNew>(result.Errors);

    /// <summary>
    ///     Projects the <see cref="Result{T}" /> into a new <see cref="Result{T}" />
    ///     of <typeparamref name="TNew" />, flattening the structure.
    /// </summary>
    /// <typeparam name="T">Generic type of the initial <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate value.</typeparam>
    /// <typeparam name="TNew">The type of the final value.</typeparam>
    /// <param name="task">
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping the <see cref="Result{T}" />.
    /// </param>
    /// <param name="intermediateSelector">
    ///     Selector used to determine the intermediate <see cref="Result{T}" />.
    /// </param>
    /// <param name="resultSelector">
    ///     Selector for the final result value.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping the <see cref="Result{T}" /> of <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> SelectMany<T, TIntermediate, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, ValueTask<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TNew> resultSelector)
    {
        if (await task is var result && result is { Some.Value: var value, })
        {
            return await intermediateSelector(value) is var intermediateResult &&
                intermediateResult is { Some.Value: var intermediate, }
                    ? Result.Ok(resultSelector(value, intermediate))
                    : Result.Fail<TNew>(intermediateResult.Errors);
        }

        return Result.Fail<TNew>(result.Errors);
    }
}