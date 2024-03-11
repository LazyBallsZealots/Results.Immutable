namespace Results.Immutable.Extensions.FluentAssertions;

public static class ResultExtensions
{
    /// <summary>
    ///     Returns a <see cref="ResultAssertions{T}" /> for the given <see cref="Result{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the value of the <see cref="Result{T}" /> to assert.</typeparam>
    public static ResultAssertions<T> Should<T>(this Result<T> result) => new(result);
}