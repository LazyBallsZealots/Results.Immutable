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
    public static async Task<Result<TNew>> Select<T, TNew>(
        this Task<Result<T>> task,
        Func<T, TNew> selector) =>
        (await task).Select(selector);

    /// <summary>
    ///     Projects the <see cref="Result{T}" />
    ///     of this <paramref name="task" />
    ///     using a provided <paramref name="resultSelector" />,
    ///     flattening the structure.
    /// </summary>
    /// <typeparam name="T">Generic type of the <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNew">Generic type of the new <see cref="Result{T}" />.</typeparam>
    /// <param name="task">
    ///     A <see cref="Task{T}" /> of <see cref="Result{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="resultSelector">
    ///     Delegate to execute.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous operation,
    ///     wrapping the <see cref="Result{T}" /> of <typeparamref name="TNew" />.
    /// </returns>
    public static async Task<Result<TNew>> SelectMany<T, TNew>(
        this Task<Result<T>> task,
        Func<T, Result<TNew>> resultSelector) =>
        (await task).SelectMany(resultSelector);

    public static async Task<Result<TNew>> SelectMany<T, TNew>(
        this Task<Result<T>> task,
        Func<T, Task<Result<TNew>>> asyncResultSelector) =>
        await task is { } result && result is { Some.Value: var value, }
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
    public static async Task<Result<TNew>> SelectMany<T, TIntermediate, TNew>(
        this Task<Result<T>> task,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TNew> resultSelector) =>
        (await task).SelectMany(intermediateSelector, resultSelector);

    public static async Task<Result<TNew>> SelectMany<T, TIntermediate, TNew>(
        this Task<Result<T>> task,
        Func<T, Task<Result<TIntermediate>>> asyncIntermediateSelector,
        Func<T, TIntermediate, TNew> resultSelector)
    {
        if (await task is { } result && result is not { Some.Value: var value, })
        {
            return Result.Fail<TNew>(result.Errors);
        }

        return await asyncIntermediateSelector(value) is { } intermediateResult &&
            intermediateResult is { Some.Value: var intermediateValue, }
                ? Result.Ok(resultSelector(value, intermediateValue))
                : Result.Fail<TNew>(intermediateResult.Errors);
    }
}