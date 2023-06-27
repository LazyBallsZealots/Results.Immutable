namespace Results.Immutable;

/// <summary>
///     Represents an optional value.
/// </summary>
public static class Option
{
    /// <summary>
    ///     Represents the <paramref name="value" />
    ///     as an option.
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="value">Value to associate with the option/</param>
    /// <returns>A new instance of <see cref="Immutable.Option{T}" /> with a value.</returns>
    public static Option<T> Some<T>(T value) => new(value);

    /// <summary>
    ///     Represents the lack of value.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <returns>
    ///     A new instance of <see cref="Immutable.Option{T}" /> without value.
    /// </returns>
    public static Option<T> None<T>() => new();

    /// <summary>
    /// Zip <see cref="Option{T}"/>s together.
    /// </summary>
    /// <param name="option1">First <see cref="Option{T}"/>.</param>
    /// <param name="option2">Second <see cref="Option{T}"/>.</param>
    /// <typeparam name="T1">Type of the first element.</typeparam>
    /// <typeparam name="T2">Type of the second element.</typeparam>
    /// <returns>A <see cref="Option{T}"/> with the zipped values.</returns>
    public static Option<(T1, T2)> Zip<T1, T2>(Option<T1> option1, Option<T2> option2) =>
        (option1.Some, option2.Some) is var ((v1), (v2))
            ? Some((v1, v2))
            : None<(T1, T2)>();

    /// <summary>
    /// Zip <see cref="Option{T}"/>s together.
    /// </summary>
    /// <param name="option1">First <see cref="Option{T}"/>.</param>
    /// <param name="option2">Second <see cref="Option{T}"/>.</param>
    /// <param name="option3">Third <see cref="Option{T}"/>.</param>
    /// <typeparam name="T1">Type of the first element.</typeparam>
    /// <typeparam name="T2">Type of the second element.</typeparam>
    /// <typeparam name="T3">Type of the third element.</typeparam>
    /// <returns>A <see cref="Option{T}"/> with the zipped values.</returns>
    public static Option<(T1, T2, T3)> Zip<T1, T2, T3>(
        Option<T1> option1,
        Option<T2> option2,
        Option<T3> option3) =>
        (option1.Some, option2.Some, option3.Some) is var ((v1), (v2), (v3))
            ? Some((v1, v2, v3))
            : None<(T1, T2, T3)>();

    /// <summary>
    /// Zip <see cref="Option{T}"/>s together.
    /// </summary>
    /// <param name="option1">First <see cref="Option{T}"/>.</param>
    /// <param name="option2">Second <see cref="Option{T}"/>.</param>
    /// <param name="option3">Third <see cref="Option{T}"/>.</param>
    /// <param name="option4">Fourth <see cref="Option{T}"/>.</param>
    /// <typeparam name="T1">Type of the first element.</typeparam>
    /// <typeparam name="T2">Type of the second element.</typeparam>
    /// <typeparam name="T3">Type of the third element.</typeparam>
    /// <typeparam name="T4">Type of the fourth element.</typeparam>
    /// <returns>A <see cref="Option{T}"/> with the zipped values.</returns>
    public static Option<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
        Option<T1> option1,
        Option<T2> option2,
        Option<T3> option3,
        Option<T4> option4) =>
        (option1.Some, option2.Some, option3.Some, option4.Some) is var ((v1), (v2), (v3), (v4))
            ? Some((v1, v2, v3, v4))
            : None<(T1, T2, T3, T4)>();

    /// <summary>
    /// Zip <see cref="Option{T}"/>s together.
    /// </summary>
    /// <param name="option1">First <see cref="Option{T}"/>.</param>
    /// <param name="option2">Second <see cref="Option{T}"/>.</param>
    /// <param name="option3">Third <see cref="Option{T}"/>.</param>
    /// <param name="option4">Fourth <see cref="Option{T}"/>.</param>
    /// <param name="option5">Fifth <see cref="Option{T}"/>.</param>
    /// <typeparam name="T1">Type of the first element.</typeparam>
    /// <typeparam name="T2">Type of the second element.</typeparam>
    /// <typeparam name="T3">Type of the third element.</typeparam>
    /// <typeparam name="T4">Type of the fourth element.</typeparam>
    /// <typeparam name="T5">Type of the fifth element.</typeparam>
    /// <returns>A <see cref="Option{T}"/> with the zipped values.</returns>
    public static Option<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
        Option<T1> option1,
        Option<T2> option2,
        Option<T3> option3,
        Option<T4> option4,
        Option<T5> option5) =>
        (option1.Some, option2.Some, option3.Some, option4.Some, option5.Some) is var ((v1), (v2), (v3), (v4), (v5))
            ? Some((v1, v2, v3, v4, v5))
            : None<(T1, T2, T3, T4, T5)>();

    /// <summary>
    /// Transpose a list of <see cref="Option{T}"/>s,
    /// aggregating the <see cref="Option{T}.Some"/>s into a <see cref="ImmutableList{T}"/>.
    /// If any of the <see cref="Option{T}" />s is None, the resulting <see cref="Option{T}"/> will be None. 
    /// </summary>
    /// <param name="options">List of <see cref="Option{T}"/>s</param>
    /// <typeparam name="T">Type of the elements.</typeparam>
    /// <returns></returns>
    public static Option<ImmutableList<T>> Transpose<T>(IEnumerable<Option<T>> options)
    {
        var builder = ImmutableList.CreateBuilder<T>();
        foreach (var option in options)
        {
            if (option.Some is var (value))
            {
                builder.Add(value);
            }
            else
            {
                return None<ImmutableList<T>>();
            }
        }

        return Some(builder.ToImmutable());
    }
}