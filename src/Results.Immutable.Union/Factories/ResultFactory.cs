namespace Results.Immutable;

public static class Result
{
    /// <summary>
    ///     Returns successful <see cref="Result{T}" />, containing provided <paramref name="value" />.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <returns>
    ///     A successful <see cref="Result{T}" />.
    /// </returns>
    public static Result<T> Ok<T>(T value) => Result<T>.IUnionMembers.Create(value);

    /// <summary>
    ///     Returns a failed <see cref="Result{T}" />, containing provided <paramref name="errors" />.
    /// </summary>
    /// <param name="errors"><see cref="Error" />s to be passed to the returned <see cref="Result{T}" />.</param>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <returns>
    ///     A failed <see cref="Result{T}" />
    /// </returns>
    public static Result<T> Fail<T>(IEnumerable<Error> errors) => Result<T>.IUnionMembers.Create([.. errors,]);
}