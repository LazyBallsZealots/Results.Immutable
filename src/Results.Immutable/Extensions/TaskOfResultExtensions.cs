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
    ///     A <see cref="ValueTask{T}" /> of <see cref="Result{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="selector">
    ///     Delegate to execute.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous operation,
    ///     wrapping the <see cref="Result{T}" /> of <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> Select<T, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, TNew> selector) =>
        await task is var result && result is { Some.Value: var value, }
            ? new(selector(value))
            : Result.Fail<TNew>(result.Errors);

    /// <summary>
    ///     Projects the result of this <paramref name="task" />
    ///     into a <see cref="Result{T} " /> of <typeparamref name="TNew" />
    ///     asynchronously by applying provided
    ///     <paramref name="asyncSelector" />.
    /// </summary>
    /// <typeparam name="T">
    ///     Generic type of the initial result.
    /// </typeparam>
    /// <typeparam name="TNew">
    ///     Generic type of the returned result.
    /// </typeparam>
    /// <param name="task">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Result{T}" />
    ///     to project.
    /// </param>
    /// <param name="asyncSelector">
    ///     Asynchronous projecting function.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result
    ///     of an asynchronous projection of the underlying
    ///     <see cref="Result{T}" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> SelectAsync<T, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, ValueTask<TNew>> asyncSelector)
    {
        var result = await task;
        return result is { Some.Value: var value, }
            ? Result.Ok(await asyncSelector(value))
            : Result.Fail<TNew>(result.Errors);
    }

    /// <summary>
    ///     Projects the <paramref name="task" /> and flattens
    ///     the <see cref="Result{T}" /> obtained from executing
    ///     the <paramref name="selector" />.
    /// </summary>
    /// <typeparam name="T">
    ///     Generic type of the initial result.
    /// </typeparam>
    /// <typeparam name="TNew">
    ///     Generic type of the final result.
    /// </typeparam>
    /// <param name="task">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Result{T}" />
    ///     to project.
    /// </param>
    /// <param name="selector">
    ///     Delgate used to project successful result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />,
    ///     representing the result of an asynchronous operation, which wraps
    ///     the projected <see cref="Result{T}" /> of
    ///     <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> SelectMany<T, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, Result<TNew>> selector)
    {
        var result = await task;

        return result is { Some.Value: var value, } ? selector(value) : Result.Fail<TNew>(result.Errors);
    }

    /// <summary>
    ///     Projects the <see cref="Result{T}" /> into a new <see cref="Result{T}" />
    ///     of <typeparamref name="TNew" />, flattening the structure.
    /// </summary>
    /// <typeparam name="T">Generic type of the initial <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate value.</typeparam>
    /// <typeparam name="TNew">The type of the final value.</typeparam>
    /// <param name="task">
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous
    ///     operation, wrapping the <see cref="Result{T}" />.
    /// </param>
    /// <param name="intermediateSelector">
    ///     Selector used to determine the intermediate <see cref="Result{T}" />.
    /// </param>
    /// <param name="resultSelector">
    ///     Selector for the final result value.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous
    ///     operation, wrapping the <see cref="Result{T}" /> of <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> SelectMany<T, TIntermediate, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TNew> resultSelector)
    {
        if (await task is var result && result is { Some.Value: var value, })
        {
            return intermediateSelector(value) is var intermediateResult &&
                intermediateResult is { Some.Value: var intermediate, }
                    ? Result.Ok(resultSelector(value, intermediate))
                    : Result.Fail<TNew>(intermediateResult.Errors);
        }

        return Result.Fail<TNew>(result.Errors);
    }

    /// <summary>
    ///     Projects the <see cref="Result{T}" />
    ///     of this <paramref name="task" /> asynchronously,
    ///     using a provided <paramref name="asyncResultSelector" />
    ///     and flattening the structure.
    /// </summary>
    /// <typeparam name="T">Generic type of the <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNew">Generic type of the new <see cref="Result{T}" />.</typeparam>
    /// <param name="task">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Result{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="asyncResultSelector">
    ///     Delegate to execute.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous operation,
    ///     wrapping the <see cref="Result{T}" /> of <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> SelectManyAsync<T, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, ValueTask<Result<TNew>>> asyncResultSelector) =>
        await task is var result && result is { Some.Value: var value, }
            ? await asyncResultSelector(value)
            : Result.Fail<TNew>(result.Errors);

    /// <summary>
    ///     Projects the <see cref="Result{T}" />
    ///     of this <paramref name="task" /> asynchronously,
    ///     using the provided <paramref name="intermediateSelector" />
    ///     to obtain intermediate <see cref="Result{T}" /> of
    ///     <typeparamref name="TIntermediate" /> and
    ///     <paramref name="resultSelector" /> to project resulting
    ///     values into <typeparamref name="TNew" />.
    /// </summary>
    /// <typeparam name="T">
    ///     Generic type of the initial <see cref="Result{T}" />.
    /// </typeparam>
    /// <typeparam name="TIntermediate">
    ///     Generic type of the intermediate <see cref="Result{T}" />.
    /// </typeparam>
    /// <typeparam name="TNew">
    ///     Generic type of the final <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="task">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Result{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="intermediateSelector">
    ///     Asynchronous delegate used to obtain the intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     Delegate used to project both
    ///     <typeparamref name="T" /> and <typeparamref name="TIntermediate" />
    ///     values into <typeparamref name="TNew" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of
    ///     an asynchronous operation, wrapping the <see cref="Result{T}" />
    ///     of <typeparamref name="TNew" />.
    /// </returns>
    public static async ValueTask<Result<TNew>> SelectManyAsync<T, TIntermediate, TNew>(
        this ValueTask<Result<T>> task,
        Func<T, ValueTask<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TNew> resultSelector)
    {
        var initialResult = await task;

        if (initialResult is { Some.Value: var value, })
        {
            var intermediateResult = await intermediateSelector(value);
            return intermediateResult is { Some.Value: var intermediate, }
                ? Result.Ok(resultSelector(value, intermediate))
                : Result.Fail<TNew>(intermediateResult.Errors);
        }

        return Result.Fail<TNew>(initialResult.Errors);
    }
}