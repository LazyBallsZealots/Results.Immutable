using System.Runtime.CompilerServices;

namespace Results.Immutable;

public readonly partial record struct Option<T>
{

    /// <summary>
    /// Project the value of the <see cref="Option{T}"/> to a different type with 
    /// a selector function that returns a <see cref="Option{TOut}"/>.
    /// 
    /// Example:
    /// 
    /// <code>
    /// var option = Option.Some(1);
    /// var result = option.AndThen(x => Option.Some(x + 1));
    /// var resultNone = option.AndThen(x => Option.None&lt;int&gt;());
    /// </code>
    /// 
    /// </summary>
    /// <param name="selector">A function to transform the value of the <see cref="Option{T}"/>.</param>
    /// <typeparam name="TOut">The new type.</typeparam>
    /// <returns>An option of the new type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TOut> SelectMany<TOut>(Func<T, Option<TOut>> selector) =>
            Some is var (value)
            ? selector(value)
            : Option.None<TOut>();

    /// <inheritdoc cref="AndThen{TOut}(Func{T, Option{TOut}})"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TOut> AndThen<TOut>(Func<T, Option<TOut>> selector) =>
            SelectMany(selector);

    /// <summary>
    ///     Returns this <see cref="Option{T}" />, if this <see cref="Option{T}" /> has a value.
    ///     Otherwise, returns the returned value by <paramref name="fn" />.
    /// </summary>
    /// <param name="fn">A function that returns an <see cref="Option{T}" />.</param>
    /// <returns>An <see cref="Option{T}" />.</returns>
    public Option<T> OrElse(Func<Option<T>> fn) =>
            IsSome ? this : fn();

    /// <summary>
    ///     Returns the first <see cref="Option{T}

    /// <summary>
    ///     Combines the options and uses <paramref name="selector" />
    ///     to project the combined values to a new <see cref="Option{T}" />.
    /// </summary>
    /// <typeparam name="T1">
    ///     Generic type of the combined <see cref="Option{T}" />.
    /// </typeparam>
    /// <typeparam name="TOut">
    ///     Generic type of the resulting <see cref="Option{T}" />.
    /// </typeparam>
    /// <param name="combinator">
    ///     Delegate used to obtain an <see cref="Option{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="selector">
    ///     Selector for the final <see cref="Option{T}" />.
    /// </param>
    /// <returns>
    ///     An <see cref="Option{T}" />, which combines all
    ///     of the intermediate options.
    /// </returns>
    public Option<TOut> SelectMany<T1, TOut>(
        Func<T, Option<T1>> combinator,
        Func<T, T1, Option<TOut>> selector) =>
        Some is var (value) && combinator(value).Some is var (value1)
            ? selector(value, value1)
            : Option.None<TOut>();

    /// <typeparam name="T2">
    ///     Generic type of the combined <see cref="Option{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain an <see cref="Option{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain an <see cref="Option{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <param name="selector"></param>
    /// <inheritdoc cref="SelectMany{T1,TOut}" />
    public Option<TOut> SelectMany<T1, T2, TOut>(
        Func<T, Option<T1>> firstCombinator,
        Func<T, Option<T2>> secondCombinator,
        Func<T, T1, T2, Option<TOut>> selector) =>
        Some is var (value)
            && firstCombinator(value).Some is var (value1)
            && secondCombinator(value).Some is var (value2)
            ? selector(value, value1, value2)
            : Option.None<TOut>();

    /// <typeparam name="T3">
    ///     Generic type of the combined <see cref="Option{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T3" /> from existing option.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,T2,TOut}" />
    public Option<TOut> SelectMany<T1, T2, T3, TOut>(
        Func<Option<T>, Option<T1>> firstCombinator,
        Func<Option<T>, Option<T2>> secondCombinator,
        Func<Option<T>, Option<T3>> thirdCombinator,
        Func<T?, T1?, T2?, T3?, Option<TOut>> selector) =>
        Some is var (value)
            && firstCombinator(this).Some is var (value1)
            && secondCombinator(this).Some is var (value2)
            && thirdCombinator(this).Some is var (value3)
            ? selector(value, value1, value2, value3)
            : Option.None<TOut>();

    /// <typeparam name="T4">
    ///     Generic type of the combined <see cref="Option{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T3" /> from existing option.
    /// </param>
    /// <param name="fourthCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T4" /> from existing option.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,T2,T3,TOut}" />
    public Option<TOut> SelectMany<T1, T2, T3, T4, TOut>(
        Func<Option<T>, Option<T1>> firstCombinator,
        Func<Option<T>, Option<T2>> secondCombinator,
        Func<Option<T>, Option<T3>> thirdCombinator,
        Func<Option<T>, Option<T4>> fourthCombinator,
        Func<T?, T1?, T2?, T3?, T4?, Option<TOut>> selector) =>
        Some is var (value)
            && firstCombinator(this).Some is var (value1)
            && secondCombinator(this).Some is var (value2)
            && thirdCombinator(this).Some is var (value3)
            && fourthCombinator(this).Some is var (value4)
            ? selector(value, value1, value2, value3, value4)
            : Option.None<TOut>();

    /// <typeparam name="T5">
    ///     Generic type of the combined <see cref="Option{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T3" /> from existing option.
    /// </param>
    /// <param name="fourthCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T4" /> from existing option.
    /// </param>
    /// <param name="fifthCombinator">
    ///     Delegate used to obtain a <see cref="Option{T}" />
    ///     of <typeparamref name="T4" /> from existing option.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,T2,T3,T4,TOut}" />
    public Option<TOut> SelectMany<T1, T2, T3, T4, T5, TOut>(
        Func<Option<T>, Option<T1>> firstCombinator,
        Func<Option<T>, Option<T2>> secondCombinator,
        Func<Option<T>, Option<T3>> thirdCombinator,
        Func<Option<T>, Option<T4>> fourthCombinator,
        Func<Option<T>, Option<T5>> fifthCombinator,
        Func<T?, T1?, T2?, T3?, T4?, T5?, Option<TOut>> selector) =>
        Some is var (value)
            && firstCombinator(this).Some is var (value1)
            && secondCombinator(this).Some is var (value2)
            && thirdCombinator(this).Some is var (value3)
            && fourthCombinator(this).Some is var (value4)
            && fifthCombinator(this).Some is var (value5)
            ? selector(value, value1, value2, value3, value4, value5)
            : Option.None<TOut>();
}