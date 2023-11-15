namespace Results.Immutable;

/// <summary>
///     Extensions for <see cref="Result{T}" /> and <see cref="Option{T}" />
/// </summary>
public static class ResultOptionConvertExtensions
{
    /// <summary>
    ///     Converts a <see cref="Result{T}" /> to an <see cref="Option{T}" /> by dropping the <see cref="Result{T}.Error" />.
    /// </summary>
    /// <param name="result">The <see cref="Result{T}" /> to convert.</param>
    /// <typeparam name="T">The type of the <see cref="Result{T}" />.</typeparam>
    public static Option<T> ToOption<T>(this Result<T> result) =>
        result.Some is var (value) ? Option.Some(value) : Option.None<T>();

    /// <summary>
    ///     Converts an <see cref="Option{T}" /> to a <see cref="Result{T}" /> by wrapping the <see cref="Option{T}.Some" />,
    ///     or adds the <paramref name="error" /> to <see cref="Result{T}.Errors" /> if <see cref="Option{T}.IsNone" />.
    /// </summary>
    /// <param name="option">The <see cref="Option{T}" /> to convert.</param>
    /// <param name="error">The <see cref="Error" /> to add if <see cref="Option{T}.IsNone" />.</param>
    /// <typeparam name="T">The type of the <see cref="Option{T}" />.</typeparam>
    public static Result<T> ToResult<T>(this Option<T> option, Error error) =>
        option.Some is var (value) ? Result.Ok(value) : Result.Fail<T>(error);

    /// <summary>
    ///     Converts an <see cref="Option{T}" /> to a <see cref="Result{T}" /> by wrapping the <see cref="Option{T}.Some" />,
    ///     or adds the result of <paramref name="errorFactory" /> to <see cref="Result{T}.Errors" /> if
    ///     <see cref="Option{T}.IsNone" />.
    /// </summary>
    /// <param name="option">The <see cref="Option{T}" /> to convert.</param>
    /// <param name="errorFactory">The factory that creates the <see cref="Error" /> to add if <see cref="Option{T}.IsNone" />.</param>
    /// <typeparam name="T">The type of the <see cref="Option{T}" />.</typeparam>
    public static Result<T> ToResult<T>(this Option<T> option, Func<Error> errorFactory) =>
        option.Some is var (value) ? Result.Ok(value) : Result.Fail<T>(errorFactory());
}