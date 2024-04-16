namespace Results.Immutable;

public static class TaskOfOptionExtensions
{
    /// <summary>
    ///     Projects this <paramref name="asyncOption" />'s
    ///     <see cref="Option{T}" /> into an <see cref="Option{T}" />
    ///     of <typeparamref name="TOut" /> by applying provided <paramref name="selector" />.
    /// </summary>
    /// <typeparam name="T">Generic type of the <see cref="Option{T}" />.</typeparam>
    /// <typeparam name="TOut">Generic type of the resulting <see cref="Option{T}" />.</typeparam>
    /// <param name="asyncOption">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Option{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="selector">
    ///     Delegate to execute.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous operation,
    ///     wrapping the <see cref="Option{T} " /> of <typeparamref name="TOut" />.
    /// </returns>
    public static async ValueTask<Option<TOut>> Select<T, TOut>(
        this ValueTask<Option<T>> asyncOption,
        Func<T, TOut> selector) =>
        await asyncOption is { Some.Value: var value, } ? Option.Some(selector(value)) : Option.None<TOut>();

    /// <summary>
    ///     Asynchronously projects this <paramref name="asyncOption" />'s
    ///     <see cref="Option{T}" /> into an <see cref="Option{T}" />
    ///     of <typeparamref name="TOut" /> by applying provided <paramref name="asyncSelector" />.
    /// </summary>
    /// <typeparam name="T">Generic type of the <see cref="Option{T}" />.</typeparam>
    /// <typeparam name="TOut">Generic type of the resulting <see cref="Option{T}" />.</typeparam>
    /// <param name="asyncOption">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Option{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="selector">
    ///     Asynchronous projecting function.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous projection
    ///     of the underlying <see cref="Option{T}" />.
    /// </returns>
    public static async ValueTask<Option<TOut>> SelectAsync<T, TOut>(
        this ValueTask<Option<T>> asyncOption,
        Func<T, ValueTask<TOut>> asyncSelector) =>
        await asyncOption is { Some.Value: var value, } ? Option.Some(await asyncSelector(value)) : Option.None<TOut>();

    /// <summary>
    ///     Projects the <paramref name="asyncOption" /> and flattens
    ///     the <see cref="Option{T}" /> obtained from executing
    ///     the <paramref name="combinator" />.
    /// </summary>
    /// <typeparam name="T">Generic type of the initial <see cref="Option{T}" />.</typeparam>
    /// <typeparam name="TOut">Generic type of the final <see cref="Option{T}" />.</typeparam>
    /// <param name="asyncOption">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Option{T}" />
    ///     to project.
    /// </param>
    /// <param name="combinator">
    ///     Delegate used to project the <see cref="Option{T}" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />,
    ///     representing the result of an asynchronous operation, which wraps
    ///     the projected <see cref="Option{T}" /> of
    ///     <typeparamref name="TOut" />.
    /// </returns>
    public static async ValueTask<Option<TOut>> SelectMany<T, TOut>(
        this ValueTask<Option<T>> asyncOption,
        Func<T, Option<TOut>> combinator) =>
        await asyncOption is { Some.Value: var value, } ? combinator(value) : Option.None<TOut>();

    /// <summary>
    ///     Projects the <paramref name="asyncOption" />, flattens
    ///     the <see cref="Option{T}" /> obtained from executing
    ///     the <paramref name="intermediateCombinator" />
    ///     and projects both values using <paramref name="selector" />.
    /// </summary>
    /// <typeparam name="T">Generic type of the initial <see cref="Option{T}" />.</typeparam>
    /// <typeparam name="TIntermediate">Generic type of the intermediate <see cref="Option{T}" />.</typeparam>
    /// <typeparam name="TOut">Generic type of the final <see cref="Option{T}" />.</typeparam>
    /// <param name="asyncOption">
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous operaion,
    ///     wrapping the <see cref="Option{T}" />.
    /// </param>
    /// <param name="intermediateCombinator">
    ///     Delegate used to obtain the intermediate <see cref="Option{T}" />.
    /// </param>
    /// <param name="selector">
    ///     Selectof for the final value.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, reresenting the result of an asynchronous operation,
    ///     wrapping the <see cref="Option{T}" /> of <typeparamref name="TOut" />.
    /// </returns>
    public static async ValueTask<Option<TOut>> SelectMany<T, TIntermediate, TOut>(
        this ValueTask<Option<T>> asyncOption,
        Func<T, Option<TIntermediate>> intermediateCombinator,
        Func<T, TIntermediate, TOut> selector) =>
        await asyncOption is { Some.Value: var value, } &&
        intermediateCombinator(value) is { Some.Value: var intermediate, }
            ? Option.Some(selector(value, intermediate))
            : Option.None<TOut>();

    /// <summary>
    ///     Projects the <paramref name="asyncOption" /> asynchronously
    ///     and flattens the <see cref="Option{T}" /> obtained from executing
    ///     the <paramref name="asyncCombinator" />.
    /// </summary>
    /// <typeparam name="T">Generic type of the initial <see cref="Option{T}" />.</typeparam>
    /// <typeparam name="TOut">Generic type of the final <see cref="Option{T}" />.</typeparam>
    /// <param name="asyncOption">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Option{T}" />
    ///     to project.
    /// </param>
    /// <param name="asyncCombinator">
    ///     Asynchronous delegate used to project the <see cref="Option{T}" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />,
    ///     representing the result of an asynchronous operation, which wraps
    ///     the projected <see cref="Option{T}" /> of
    ///     <typeparamref name="TOut" />.
    /// </returns>
    public static async ValueTask<Option<TOut>> SelectManyAsync<T, TOut>(
        this ValueTask<Option<T>> asyncOption,
        Func<T, ValueTask<Option<TOut>>> asyncCombinator) =>
        await asyncOption is { Some.Value: var value, } ? await asyncCombinator(value) : Option.None<TOut>();

    /// <summary>
    ///     Projects the <see cref="Option{T}" />
    ///     of this <paramref name="task" /> asynchronously,
    ///     using the provided <paramref name="intermediateSelector" />
    ///     to obtain intermediate <see cref="Option{T}" /> of
    ///     <typeparamref name="TIntermediate" /> and
    ///     <paramref name="resultSelector" /> to project resulting
    ///     values into <typeparamref name="TNew" />.
    /// </summary>
    /// <typeparam name="T">
    ///     Generic type of the initial <see cref="Option{T}" />.
    /// </typeparam>
    /// <typeparam name="TIntermediate">
    ///     Generic type of the intermediate <see cref="Option{T}" />.
    /// </typeparam>
    /// <typeparam name="TOut">
    ///     Generic type of the final <see cref="Option{T}" />.
    /// </typeparam>
    /// <param name="asyncOption">
    ///     A <see cref="ValueTask{T}" /> of <see cref="Option{T}" />,
    ///     representing the result of an asynchronous operation.
    /// </param>
    /// <param name="asyncCombinator">
    ///     Asynchronous delegate used to obtain the intermediate value.
    /// </param>
    /// <param name="selector">
    ///     Delegate used to project both
    ///     <typeparamref name="T" /> and <typeparamref name="TIntermediate" />
    ///     values into <typeparamref name="TNew" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of
    ///     an asynchronous operation, wrapping the <see cref="Option{T}" />
    ///     of <typeparamref name="TOut" />.
    /// </returns>
    public static async ValueTask<Option<TOut>> SelectManyAsync<T, TIntermediate, TOut>(
        this ValueTask<Option<T>> asyncOption,
        Func<T, ValueTask<Option<TIntermediate>>> asyncCombinator,
        Func<T, TIntermediate, TOut> selector) =>
        await asyncOption is { Some.Value: var value, } &&
        await asyncCombinator(value) is { Some.Value: var intermediate, }
            ? Option.Some(selector(value, intermediate))
            : Option.None<TOut>();
}