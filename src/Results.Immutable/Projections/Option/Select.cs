using System.Runtime.CompilerServices;
namespace Results.Immutable;

public readonly partial record struct Option<T>
{
    /// <summary>
    /// Project the value of the <see cref="Option{T}"/> to a different type.
    /// 
    /// If the original option is <see cref="Option.None{T}"/> then <see cref="Option.None{TNew}"/> is returned.
    /// 
    /// Example:
    /// 
    /// <code>
    /// var opt = Option.Some("Hello");
    /// var opt2 = opt.Select(x => x.Length);
    /// </code>
    /// 
    /// </summary>
    /// <param name="selector">Transform function.</param>
    /// <typeparam name="TNew">The new type.</typeparam>
    /// <returns>An option of the new type.</returns>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TNew> Select<TNew>(Func<T, TNew> selector) =>
        Some is var (x) ? Option.Some(selector(x)) : Option.None<TNew>();

    ///<inheritdoc cref="Select{TNew}(Func{T, TNew})"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TNew> Map<TNew>(Func<T, TNew> map) =>
            Select(map);

}