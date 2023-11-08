using static Results.Immutable.Collection.Collection;

namespace Results.Immutable;

/// <summary>
///     Contains extension methods for <see cref="Result{T}" />
///     for joining multiple well-typed <see cref="Result{T}" />s into one.
/// </summary>
public static class ZipExtensions
{
    /// <summary>
    ///     Joins two <see cref="Result{T}" />s into a single <see cref="Result{T}" /> with a tuple.
    /// </summary>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <typeparam name="T1">Type of the first result</typeparam>
    /// <typeparam name="T2">Type of the second result</typeparam>
    public static Result<(T1, T2)> Zip<T1, T2>(this Result<T1> first, Result<T2> second) =>
        (first.Some, second.Some) is var ((v1), (v2)) ? new((v1, v2)) : new(ConcatLists(first.Errors, second.Errors));

    /// <summary>
    ///     Joins three <see cref="Result{T}" />s into a single <see cref="Result{T}" /> with a tuple.
    /// </summary>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <typeparam name="T1">Type of the first result</typeparam>
    /// <typeparam name="T2">Type of the second result</typeparam>
    /// <typeparam name="T3">Type of the third result</typeparam>
    public static Result<(T1, T2, T3)> Zip<T1, T2, T3>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third) =>
        (first.Some, second.Some, third.Some) is var ((v1), (v2), (v3))
            ? new((v1, v2, v3))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors));

    /// <summary>
    ///     Joins four <see cref="Result{T}" />s into a single <see cref="Result{T}" /> with a tuple.
    /// </summary>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="fourth">Fourth result</param>
    /// <typeparam name="T1">Type of the first result</typeparam>
    /// <typeparam name="T2">Type of the second result</typeparam>
    /// <typeparam name="T3">Type of the third result</typeparam>
    /// <typeparam name="T4">Type of the fourth result</typeparam>
    public static Result<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth) =>
        (first.Some, second.Some, third.Some, fourth.Some) is var ((v1), (v2), (v3), (v4))
            ? new((v1, v2, v3, v4))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors,
                    fourth.Errors));

    /// <summary>
    ///     Joins five <see cref="Result{T}" />s into a single <see cref="Result{T}" /> with a tuple.
    /// </summary>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="fourth">Fourth result</param>
    /// <param name="fifth">Fifth result</param>
    /// <typeparam name="T1">Type of the first result</typeparam>
    /// <typeparam name="T2">Type of the second result</typeparam>
    /// <typeparam name="T3">Type of the third result</typeparam>
    /// <typeparam name="T4">Type of the fourth result</typeparam>
    /// <typeparam name="T5">Type of the fifth result</typeparam>
    public static Result<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth) =>
        (first.Some, second.Some, third.Some, fourth.Some, fifth.Some) is var ((v1), (v2), (v3), (v4), (v5))
            ? new((v1, v2, v3, v4, v5))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors,
                    fourth.Errors,
                    fifth.Errors));

    /// <summary>
    ///     Joins two <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static Result<TR> ZipWith<T1, T2, TR>(
        this Result<T1> first,
        Result<T2> second,
        Func<T1, T2, TR> zipper) =>
        (first.Some, second.Some) is var ((v1), (v2))
            ? new(zipper(v1, v2))
            : new(ConcatLists(first.Errors, second.Errors));

    /// <summary>
    ///     Joins three <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="T3">Third result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static Result<TR> ZipWith<T1, T2, T3, TR>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Func<T1, T2, T3, TR> zipper) =>
        (first.Some, second.Some, third.Some) is var ((v1), (v2), (v3))
            ? new(
                zipper(
                    v1,
                    v2,
                    v3))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors));

    /// <summary>
    ///     Joins four <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="T3">Third result type</typeparam>
    /// <typeparam name="T4">Fourth result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="fourth">Fourth result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static Result<TR> ZipWith<T1, T2, T3, T4, TR>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Func<T1, T2, T3, T4, TR> zipper) =>
        (first.Some, second.Some, third.Some, fourth.Some) is var ((v1), (v2), (v3), (v4))
            ? new(
                zipper(
                    v1,
                    v2,
                    v3,
                    v4))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors,
                    fourth.Errors));

    /// <summary>
    ///     Joins five <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="T3">Third result type</typeparam>
    /// <typeparam name="T4">Fourth result type</typeparam>
    /// <typeparam name="T5">Fifth result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="fourth">Fourth result</param>
    /// <param name="fifth">Fifth result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static Result<TR> ZipWith<T1, T2, T3, T4, T5, TR>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Func<T1, T2, T3, T4, T5, TR> zipper) =>
        (first.Some, second.Some, third.Some, fourth.Some, fifth.Some) is var ((v1), (v2), (v3), (v4), (v5))
            ? new(
                zipper(
                    v1,
                    v2,
                    v3,
                    v4,
                    v5))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors,
                    fourth.Errors,
                    fifth.Errors));

    /// <summary>
    ///     Joins two <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static async ValueTask<Result<TR>> ZipWithAsync<T1, T2, TR>(
        this Result<T1> first,
        Result<T2> second,
        Func<T1, T2, ValueTask<TR>> zipper) =>
        (first.Some, second.Some) is var ((v1), (v2))
            ? new(await zipper(v1, v2))
            : new(ConcatLists(first.Errors, second.Errors));

    /// <summary>
    ///     Joins three <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="T3">Third result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static async ValueTask<Result<TR>> ZipWithAsync<T1, T2, T3, TR>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Func<T1, T2, T3, ValueTask<TR>> zipper) =>
        (first.Some, second.Some, third.Some) is var ((v1), (v2), (v3))
            ? new(
                await zipper(
                    v1,
                    v2,
                    v3))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors));

    /// <summary>
    ///     Joins four <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="T3">Third result type</typeparam>
    /// <typeparam name="T4">Fourth result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="fourth">Fourth result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static async ValueTask<Result<TR>> ZipWithAsync<T1, T2, T3, T4, TR>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Func<T1, T2, T3, T4, ValueTask<TR>> zipper) =>
        (first.Some, second.Some, third.Some, fourth.Some) is var ((v1), (v2), (v3), (v4))
            ? new(
                await zipper(
                    v1,
                    v2,
                    v3,
                    v4))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors,
                    fourth.Errors));

    /// <summary>
    ///     Joins five <see cref="Result{T}" />s into one with the specified <paramref name="zipper" />.
    /// </summary>
    /// <typeparam name="T1">First result type</typeparam>
    /// <typeparam name="T2">Second result type</typeparam>
    /// <typeparam name="T3">Third result type</typeparam>
    /// <typeparam name="T4">Fourth result type</typeparam>
    /// <typeparam name="T5">Fifth result type</typeparam>
    /// <typeparam name="TR">Result type</typeparam>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="fourth">Fourth result</param>
    /// <param name="fifth">Fifth result</param>
    /// <param name="zipper">A function to zip the results into one</param>
    public static async ValueTask<Result<TR>> ZipWithAsync<T1, T2, T3, T4, T5, TR>(
        this Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Func<T1, T2, T3, T4, T5, ValueTask<TR>> zipper) =>
        (first.Some, second.Some, third.Some, fourth.Some, fifth.Some) is var ((v1), (v2), (v3), (v4), (v5))
            ? new(
                await zipper(
                    v1,
                    v2,
                    v3,
                    v4,
                    v5))
            : new(
                ConcatLists(
                    first.Errors,
                    second.Errors,
                    third.Errors,
                    fourth.Errors,
                    fifth.Errors));
}