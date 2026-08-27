using AwesomeAssertions.Execution;
using Results.Immutable.Extensions.AwesomeAssertions.Assertions;

namespace Results.Immutable.Extensions.AwesomeAssertions;

public static class OptionExtensions
{
    /// <summary>
    ///     Returns a <see cref="OptionAssertions{T}" /> for the given <see cref="Option{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the value of the <see cref="Option{T}" /> to assert.</typeparam>
    public static OptionAssertions<T> Should<T>(this Option<T> option) => new(option, AssertionChain.GetOrCreate());
}