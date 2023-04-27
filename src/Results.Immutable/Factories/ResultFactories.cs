using Results.Immutable.Metadata;
using static Results.Immutable.Collection.Collection;

namespace Results.Immutable;

public readonly record struct Result
{
    /// <summary>
    ///     Represents a successful operation.
    /// </summary>
    /// <returns>
    ///     A successful result, wrapping a <see cref="Unit" />.
    /// </returns>
    /// <remarks>
    ///     This method should be used by all operations
    ///     which are supposed to return <see langword="void" />.
    /// </remarks>
    public static Result<Unit> Ok() => new(Unit.Value);

    /// <summary>
    ///     Represents a successful operation.
    /// </summary>
    /// <returns>
    ///     A successful result.
    /// </returns>
    public static Result<T> Ok<T>() => new();

    /// <summary>
    ///     Represents a successful operation.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">Value to associate with successful result.</param>
    /// <returns>
    ///     A successful result, wrapping provided <paramref name="value" />.
    /// </returns>
    /// <remarks></remarks>
    public static Result<T> Ok<T>(T value) => new(value);

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" />
    ///     of <see cref="Unit" /> if the <paramref name="condition" />
    ///     is <see langword="true" />, otherwise - failure is returned.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorMessage">An error message to associate with failed result.</param>
    /// <returns>
    ///     A <see cref="Result{T}" /> of <see cref="Unit" />,
    ///     dependent on the <paramref name="condition" />.
    /// </returns>
    public static Result<Unit> OkIf(bool condition, string errorMessage) => condition ? Ok() : Fail<Unit>(errorMessage);

    /// <inheritdoc cref="OkIf(bool, string)" />
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    public static Result<Unit> OkIf(bool condition, Error error) => condition ? Ok() : Fail<Unit>(error);

    /// <param name="errorMessageFactory">
    ///     A <see cref="Func{T}" />, returning a <see cref="string" />
    ///     to be used to build an <see cref="Error" /> instance.
    /// </param>
    /// <inheritdoc cref="OkIf(bool, string)" />
    public static Result<Unit> OkIf(bool condition, Func<string> errorMessageFactory) =>
        condition ? Ok() : Fail<Unit>(errorMessageFactory());

    /// <param name="errorMessageFactory">
    ///     A <see cref="Func{T}" />, returning an <see cref="Error" />.
    /// </param>
    /// <inheritdoc cref="OkIf(bool, Error)" />
    public static Result<Unit> OkIf(bool condition, Func<Error> errorFactory) =>
        condition ? Ok() : Fail(errorFactory());

    /// <summary>
    ///     Represents a failed operation.
    /// </summary>
    /// <param name="errorMessage">
    ///     A message to be associated with a generic
    ///     <see cref="Error" /> returned with the <see cref="Result{T}" />.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result{T}" /> of <see cref="Unit" />.
    /// </returns>
    public static Result<Unit> Fail(string errorMessage) => Fail(new Error(errorMessage));

    /// <inheritdoc cref="Fail(string)" />
    public static Result<Unit> Fail(Error error) => new(ImmutableList.Create(error));

    /// <inheritdoc cref="Fail(Error)" />
    public static Result<Unit> Fail(IEnumerable<Error> errors) => new(errors.ToImmutableList());

    /// <inheritdoc cref="Fail(Error)" />
    public static Result<Unit> Fail(params Error[] errors) => new(errors.ToImmutableList());

    /// <inheritdoc cref="Fail(string)" />
    public static Result<T> Fail<T>(string errorMessage) => Fail<T>(new Error(errorMessage));

    /// <inheritdoc cref="Fail(Error)" />
    public static Result<T> Fail<T>(Error error) => new(ImmutableList.Create(error));

    /// <inheritdoc cref="Fail(IEnumerable{Error})" />
    public static Result<T> Fail<T>(IEnumerable<Error> errors) => new(errors.ToImmutableList());

    /// <inheritdoc cref="Fail(IEnumerable{Error})" />
    public static Result<T> Fail<T>(params Error[] errors) => new(errors.ToImmutableList());

    /// <summary>
    ///     Creates a failed <see cref="Result{T}" />
    ///     of <see cref="Unit" /> if the <paramref name="condition" />
    ///     is <see langword="true" />, otherwise - successful result
    ///     is returned.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorMessage">An error message to associate with failed result.</param>
    /// <returns>
    ///     A <see cref="Result{T}" /> of <see cref="Unit" />,
    ///     dependent on the <paramref name="condition" />.
    /// </returns>
    /// <seealso cref="FailIf(bool, Func{string})" />
    public static Result<Unit> FailIf(bool condition, string errorMessage) => condition ? Fail(errorMessage) : Ok();

    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    /// <inheritdoc cref="FailIf(bool, string)" />
    /// <seealso cref="FailIf(bool, Func{Error})" />
    public static Result<Unit> FailIf(bool condition, Error error) => condition ? Fail(error) : Ok();

    /// <param name="errorMessageFactory">
    ///     A <see cref="Func{T}" />, returning a <see cref="string" />,
    ///     which contains an error message to associate with failed
    ///     <see cref="Result{T}" />.
    /// </param>
    /// <remarks>
    ///     This overload should be used if
    ///     lazy evaluation of the error is required.
    /// </remarks>
    /// <inheritdoc cref="FailIf(bool, string)" />
    /// <seealso cref="FailIf(bool, string)" />
    public static Result<Unit> FailIf(bool condition, Func<string> errorMessageFactory) =>
        condition ? Fail(errorMessageFactory()) : Ok();

    /// <param name="errorFactory">
    ///     A <see cref="Func{T}" />, returning an <see cref="Error" />
    ///     which should be associated with a failed <see cref="Result{T}" />.
    /// </param>
    /// <inheritdoc cref="FailIf(bool, Func{string})" />
    /// <seealso cref="FailIf(bool, Error)" />
    public static Result<Unit> FailIf(bool condition, Func<Error> errorFactory) =>
        condition ? Fail(errorFactory()) : Ok();

    /// <inheritdoc cref="Result.Transpose{T}(IReadOnlyCollection{Result{T}})" />
    public static Result<ImmutableList<T>> Transpose<T>(params Result<T>[] results) => Transpose(results);

    /// <summary>
    ///     Transposes a collection of <see cref="Result{T}" />s,
    ///     aggregating the values into an <see cref="ImmutableList{T}" />.
    /// 
    ///     If at least one of the <paramref name="results" /> is failed, then the whole computation is failed,
    ///     and the <see cref="Result{T}.Errors" /> will be aggregated.
    /// 
    ///     The order of the values in the <see cref="ImmutableList{T}" /> is the same as in the input collection.
    ///     The successes are aggregated in both OK and ERROR states.
    /// </summary>
    /// <param name="results">
    ///     A collection of <see cref="Result{T}" />s to transpose.
    /// </param>
    /// <typeparam name="T">Type of the values</typeparam>
    /// <returns>
    ///     A <see cref="Result{T}" /> containing an aggregation of all the values of the input collection.
    /// </returns>
    public static Result<ImmutableList<T>> Transpose<T>(IReadOnlyCollection<Result<T>> results)
    {
        if (results.Count == 0)
        {
            return Ok(ImmutableList<T>.Empty);
        }

        var errorsBuilder = ImmutableList.CreateBuilder<Error>();
        var valuesBuilder = ImmutableList.CreateBuilder<T>();
        var IsErrored = false;

        foreach (var result in results)
        {
            if (result.Some is var (x))
            {
                valuesBuilder.Add(x);
            }
            else
            {
                IsErrored = true;
                errorsBuilder.AddRange(result.Errors);
            }
        }
        return IsErrored
            ? new Result<ImmutableList<T>>(errorsBuilder.ToImmutable())
            : new Result<ImmutableList<T>>(valuesBuilder.ToImmutable());
    }

    /// <summary>
    ///     Merges all <paramref name="results" />
    ///     into one <see cref="Result{T}" />,
    ///     aggregating all errors and successes into one <see cref="Result{T}" />.
    /// 
    ///     If at least one of the <paramref name="results" /> is failed, then the whole computation is failed.
    /// </summary>
    /// <param name="results">
    ///     A collection of <see cref="Result{T}" />s to merge.
    /// </param>
    /// <returns>
    ///     A merged <see cref="Result{T}" />,
    ///     containing an aggregation of all
    ///     <see cref="Result{T}.Errors" />.
    /// </returns>
    public static Result<Unit> Merge(IReadOnlyCollection<Result<Unit>> results)
    {
        var errorsBuilder = ImmutableList.CreateBuilder<Error>();
        var IsErrored = false;

        foreach (var result in results)
        {
            if (result.Some is null)
            {
                IsErrored = true;
                errorsBuilder.AddRange(result.Errors);
            }
        }
        return IsErrored
            ? new Result<Unit>(errorsBuilder.ToImmutable())
            : new Result<Unit>(Unit.Value);

    }

    /// <summary>
    ///     Creates a <see cref="Result{T}" /> based on the arguments.
    /// </summary>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <typeparam name="T1">Type of the first result</typeparam>
    /// <typeparam name="T2">Type of the second result</typeparam>
    public static Result<(T1, T2)> Zip<T1, T2>(Result<T1> first, Result<T2> second)
    {
        if (first.Some is var (v1) && second.Some is var (v2))
        {
            return new Result<(T1, T2)>((v1, v2));
        }
        return new Result<(T1, T2)>(ConcatLists(first.Errors, second.Errors));
    }

    /// <summary>
    ///     Creates a <see cref="Result{T}" /> based on the arguments.
    /// </summary>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <typeparam name="T1">Type of the first result</typeparam>
    /// <typeparam name="T2">Type of the second result</typeparam>
    /// <typeparam name="T3">Type of the third result</typeparam>
    public static Result<(T1, T2, T3)> Zip<T1, T2, T3>(Result<T1> first, Result<T2> second, Result<T3> third)
    {
        if (first.Some is var (v1) && second.Some is var (v2) && third.Some is var (v3))
        {
            return new Result<(T1, T2, T3)>((v1, v2, v3));
        }
        return new Result<(T1, T2, T3)>(ConcatLists(first.Errors, second.Errors, third.Errors));
    }

    /// <summary>
    ///     Creates a <see cref="Result{T}" /> based on the arguments.
    /// </summary>
    /// <param name="first">First result</param>
    /// <param name="second">Second result</param>
    /// <param name="third">Third result</param>
    /// <param name="fourth">Fourth result</param>
    /// <typeparam name="T1">Type of the first result</typeparam>
    /// <typeparam name="T2">Type of the second result</typeparam>
    /// <typeparam name="T3">Type of the third result</typeparam>
    /// <typeparam name="T4">Type of the fourth result</typeparam>
    public static Result<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(Result<T1> first, Result<T2> second, Result<T3> third, Result<T4> fourth)
    {
        if (first.Some is var (v1) && second.Some is var (v2) && third.Some is var (v3) && fourth.Some is var (v4))
        {
            return new Result<(T1, T2, T3, T4)>((v1, v2, v3, v4));
        }
        return new Result<(T1, T2, T3, T4)>(ConcatLists(first.Errors, second.Errors, third.Errors, fourth.Errors));
    }

    /// <summary>
    ///     Creates a <see cref="Result{T}" /> based on the arguments.
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
    public static Result<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(Result<T1> first, Result<T2> second, Result<T3> third, Result<T4> fourth, Result<T5> fifth)
    {
        if (first.Some is var (v1) && second.Some is var (v2) && third.Some is var (v3) && fourth.Some is var (v4) && fifth.Some is var (v5))
        {
            return new Result<(T1, T2, T3, T4, T5)>((v1, v2, v3, v4, v5));
        }
        return new Result<(T1, T2, T3, T4, T5)>(ConcatLists(first.Errors, second.Errors, third.Errors, fourth.Errors, fifth.Errors));
    }

    /// <summary>
    ///     Attempts to perform a <paramref name="func" />;
    ///     if no exception is thrown, a successful
    ///     <see cref="Result{T}" /> is returned,
    ///     Otherwise - a failed <see cref="Result{T}" />
    ///     is returned, with an error instance built from
    ///     <paramref name="catchHandler" />.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of the value to return from <paramref name="func" />.
    /// </typeparam>
    /// <param name="func">A delegate, representing a function which can throw.</param>
    /// <param name="catchHandler">
    ///     A delegate which returns an instance of an <see cref="Error" />
    ///     if an exception is thrown.
    /// </param>
    /// <returns>
    ///     Successful <see cref="Result{T}" /> if no exception was thrown,
    ///     otherwise - a failure.
    /// </returns>
    /// <remarks>
    ///     The default <paramref name="catchHandler" />
    ///     returns an <see cref="ExceptionalError" />,
    ///     which contains the thrown exception in the
    ///     <see cref="ExceptionalError.CausedBy" /> property.
    /// </remarks>
    public static Result<T> Try<T>(
        Func<T> func,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

        try
        {
            return Ok(func());
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }

    /// <summary>
    ///     Attempts to perform a <paramref name="asyncFunc" />;
    ///     if no exception is thrown, a successful
    ///     <see cref="Result{T}" /> is returned,
    ///     Otherwise - a failed <see cref="Result{T}" />
    ///     is returned, with an error instance built from
    ///     <paramref name="catchHandler" />.
    /// </summary>
    /// <param name="asyncFunc">
    ///     An asynchronous delegate, represented by a
    ///     <see cref="Func{TResult}" /> of a <see cref="Task{TResult}" />
    ///     to execute.
    /// </param>
    /// <inheritdoc cref="Try{T}(Func{T}, Func{Exception, Error}?)" />
    /// <seealso cref="Try{T}(Func{ValueTask{T}}, Func{Exception, Error}?)" />
    public static async Task<Result<T>> Try<T>(
        Func<Task<T>> asyncFunc,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

        try
        {
            var result = await asyncFunc();
            return Ok(result);
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }

    /// <param name="asyncFunc">
    ///     An asynchronous delegate, represented by a
    ///     <see cref="Func{TResult}" /> of a <see cref="ValueTask{TResult}" />
    ///     to execute.
    /// </param>
    /// <inheritdoc cref="Try{T}(Func{Task{T}}, Func{Exception, Error}?)" />
    /// <seealso cref="Try{T}(Func{Task{T}}, Func{Exception, Error}?)" />
    public static async ValueTask<Result<T>> Try<T>(
        Func<ValueTask<T>> asyncFunc,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

        try
        {
            var result = await asyncFunc();
            return Ok(result);
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }

    private static Error ExceptionHandler(Exception exception) => new ExceptionalError(exception);
}