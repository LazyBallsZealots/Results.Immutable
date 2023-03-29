namespace Results.Immutable;

public readonly partial record struct Result<T>
{
    /// <summary>
    ///     Combines the results and uses <paramref name="selector" />
    ///     to project the combined values to a new <see cref="Result{T}" />.
    /// </summary>
    /// <typeparam name="T1">Generic type of the combined result.</typeparam>
    /// <typeparam name="TOut">Generic type of the final result.</typeparam>
    /// <param name="combinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T1" /> from existing result.
    /// </param>
    /// <param name="selector">
    ///     Result selector for the final <see cref="Result{T}" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result{T}" /> wrapping <typeparamref name="TOut" />,
    ///     which combines all intermediate results using
    ///     <see cref="Select{TNew}(Func{Option{T}, Result{TNew}})" />.
    /// </returns>
    public Result<TOut> SelectMany<T1, TOut>(
        Func<Option<T>, Result<T1>> combinator,
        Func<Option<T>, Option<T1>, Result<TOut>> selector) =>
        Select<TOut>(
            option => combinator(option)
                .Select(secondValue => selector(option, secondValue)));

    /// <typeparam name="T2">Generic type of the combined result.</typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="Immutable.Result{T}" />
    ///     of <typeparamref name="T1" /> from existing result.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="Immutable.Result{T}" />
    ///     of <typeparamref name="T2" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,TOut}" />
    public Result<TOut> SelectMany<T1, T2, TOut>(
        Func<Option<T>, Result<T1>> firstCombinator,
        Func<Option<T>, Result<T2>> secondCombinator,
        Func<Option<T>, Option<T1>, Option<T2>, Result<TOut>> selector) =>
        Select<TOut>(
            option => firstCombinator(option)
                .Select(
                    secondValue => secondCombinator(option)
                        .Select(
                            finalValue => selector(
                                option,
                                secondValue,
                                finalValue))));

    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="Immutable.Result{T}" />
    ///     of <typeparamref name="T3" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, TOut>(
        Func<Option<T>, Result<T1>> firstCombinator,
        Func<Option<T>, Result<T2>> secondCombinator,
        Func<Option<T>, Result<T3>> thirdCombinator,
        Func<Option<T>, Option<T1>, Option<T2>, Option<T3>, Result<TOut>> selector) =>
        Select<TOut>(
            option => firstCombinator(option)
                .Select(
                    secondValue => secondCombinator(option)
                        .Select(
                            thirdValue => thirdCombinator(option)
                                .Select(
                                    finalValue => selector(
                                        option,
                                        secondValue,
                                        thirdValue,
                                        finalValue)))));

    /// <param name="fourthCombinator">
    ///     Delegate used to obtain a <see cref="Immutable.Result{T}" />
    ///     of <typeparamref name="T4" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, T3, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, T4, TOut>(
        Func<Option<T>, Result<T1>> firstCombinator,
        Func<Option<T>, Result<T2>> secondCombinator,
        Func<Option<T>, Result<T3>> thirdCombinator,
        Func<Option<T>, Result<T4>> fourthCombinator,
        Func<Option<T>, Option<T1>, Option<T2>, Option<T3>, Option<T4>, Result<TOut>> selector) =>
        Select(
            option => firstCombinator(option)
                .Select(
                    secondValue => secondCombinator(option)
                        .Select(
                            thirdValue => thirdCombinator(option)
                                .Select(
                                    fourthValue => fourthCombinator(option)
                                        .Select(
                                            finalValue => selector(
                                                option,
                                                secondValue,
                                                thirdValue,
                                                fourthValue,
                                                finalValue))))));

    /// <param name="fifthCombinator">
    ///     Delegate used to obtain a <see cref="Immutable.Result{T}" />
    ///     of <typeparamref name="T5" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, T3, T4, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, T4, T5, TOut>(
        Func<Option<T>, Result<T1>> firstCombinator,
        Func<Option<T>, Result<T2>> secondCombinator,
        Func<Option<T>, Result<T3>> thirdCombinator,
        Func<Option<T>, Result<T4>> fourthCombinator,
        Func<Option<T>, Result<T5>> fifthCombinator,
        Func<Option<T>, Option<T1>, Option<T2>, Option<T3>, Option<T4>, Option<T5>, Result<TOut>> selector) =>
        Select(
            option => firstCombinator(option)
                .Select(
                    secondValue => secondCombinator(option)
                        .Select(
                            thirdValue => thirdCombinator(option)
                                .Select(
                                    fourthValue => fourthCombinator(option)
                                        .Select(
                                            fifthValue => fifthCombinator(option)
                                                .Select(
                                                    finalValue => selector(
                                                        option,
                                                        secondValue,
                                                        thirdValue,
                                                        fourthValue,
                                                        fifthValue,
                                                        finalValue)))))));
}