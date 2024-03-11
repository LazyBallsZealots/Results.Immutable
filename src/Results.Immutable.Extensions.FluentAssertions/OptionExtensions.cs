using Results.Immutable.Extensions.FluentAssertions.Assertions;

namespace Results.Immutable.Extensions.FluentAssertions;

public static class OptionExtensions
{
    /// <summary>
    ///     Returns a <see cref="OptionAssertions{T}" /> for the given <see cref="Option{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the value of the <see cref="Option{T}" /> to assert.</typeparam>
    public static OptionAssertions<T> Should<T>(this Option<T> option) => new(option);
}