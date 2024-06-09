namespace Results.Immutable.Member;

/// <summary>
///     A set of extensions methods for enumerable types.
/// </summary>
public static class EachParser
{
    /// <summary>
    ///     <para>
    ///         Parses each element of a list and returns a result containing <see cref="IndexError" />s with
    ///         inner errors created by the <paramref name="parser" /> in case of failure.
    ///         In case of success of each element, the result will contain the parsed values.
    ///     </para>
    /// </summary>
    /// <param name="values">The values to parse/validate.</param>
    /// <param name="parser">The parser to use for each element to determine the validity or transform.</param>
    /// <param name="message">A message to add to the error in case of failure.</param>
    /// <typeparam name="TElement">The type of the original values.</typeparam>
    /// <typeparam name="TParsed">The type of the parsed values.</typeparam>
    /// <returns>A result containing the parsed values or their errors.</returns>
    public static Result<ImmutableList<TParsed>> ParseEach<TElement, TParsed>(
        this IReadOnlyList<TElement> values,
        Func<TElement, Result<TParsed>> parser,
        string message = "") =>
        ParseEach(
            values,
            parser,
            (
                _,
                i,
                e) => new IndexError(
                i,
                message,
                e));

    /// <summary>
    ///     Parses each element of an enumerable and returns a result containing <see cref="IndexError" />s with
    ///     inner errors created by the <paramref name="parser" /> in case of failure.
    ///     In case of success of each element, the result will contain the parsed values.
    /// </summary>
    /// <param name="values">The values to parse/validate.</param>
    /// <param name="parser">The parser to use for each element to determine the validity or transform.</param>
    /// <param name="createError">
    ///     Creates an any error type based on the element, its index and the errors returned by the
    ///     parser.
    /// </param>
    /// <typeparam name="TElement">The type of the original values.</typeparam>
    /// <typeparam name="TParsed">The type of the parsed values.</typeparam>
    /// <returns>A result containing the parsed values or their errors.</returns>
    public static Result<ImmutableList<TParsed>> ParseEach<TElement, TParsed>(
        this IEnumerable<TElement> values,
        Func<TElement, Result<TParsed>> parser,
        Func<TElement, int, ImmutableList<Error>, Error> createError)
    {
        var results = values.Select(
            (x, i) =>
            {
                var result = parser(x);
                return result.Some is not null
                    ? result
                    : Result.Fail<TParsed>(
                        createError(
                            x,
                            i,
                            result.Errors));
            });

        return Result.Transpose(results);
    }

    /// <inheritdoc cref="ParseEach{TElement, TParsed}(IReadOnlyList{TElement}, Func{TElement, Result{TParsed}}, string)" />
    public static async ValueTask<Result<ImmutableList<TParsed>>> ParseEachAsync<TElement, TParsed>(
        this IReadOnlyList<TElement> values,
        Func<TElement, ValueTask<Result<TParsed>>> parser,
        string message = "") =>
        await ParseEachAsync(
            values,
            parser,
            (
                _,
                i,
                e) => new IndexError(
                i,
                message,
                e));

    /// <inheritdoc
    ///     cref="ParseEach{TElement, TParsed}(IEnumerable{TElement}, Func{TElement, Result{TParsed}}, Func{TElement, int, ImmutableList{Error}, Error})" />
    public static async ValueTask<Result<ImmutableList<TParsed>>> ParseEachAsync<TElement, TParsed>(
        this IEnumerable<TElement> values,
        Func<TElement, ValueTask<Result<TParsed>>> parser,
        Func<TElement, int, ImmutableList<Error>, Error> createError)
    {
        var results = await Task.WhenAll(
            values.Select(
                async (x, i) =>
                {
                    var result = await parser(x);
                    return result.Some is not null
                        ? result
                        : Result.Fail<TParsed>(
                            createError(
                                x,
                                i,
                                result.Errors));
                }));

        return Result.Transpose(results);
    }
}