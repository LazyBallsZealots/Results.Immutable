﻿namespace Results.Immutable;

/// <summary>
///     Static factory methods for <see cref="Result{T}" />.
/// </summary>
public static class Result
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
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">Value to associate with successful result.</param>
    /// <returns>
    ///     A successful result, wrapping provided <paramref name="value" />.
    /// </returns>
    /// <remarks></remarks>
    public static Result<T> Ok<T>(T value) => new(value);

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> if the
    ///     <paramref name="condition" /> is <see langword="true" />,
    ///     otherwise - failure is returned.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    /// <returns>
    ///     A <see cref="Result{T}" />,
    ///     dependent on the <paramref name="condition" />.
    /// </returns>
    /// <seealso cref="OkIf(bool, Func{Error})" />
    public static Result<Unit> OkIf(bool condition, Error error) => condition ? Ok() : Fail<Unit>(error);

    /// <param name="errorFactory">
    ///     A <see cref="Func{T}" />, returning an <see cref="Error" />.
    /// </param>
    /// <inheritdoc cref="OkIf(bool, Error)" />
    /// <seealso cref="OkIf(bool, Error)" />
    public static Result<Unit> OkIf(bool condition, Func<Error> errorFactory) =>
        condition ? Ok() : Fail(errorFactory());

    /// <typeparam name="T">Generic type of the result.</typeparam>
    /// <param name="value">Value to associate with successful result.</param>
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    /// <inheritdoc cref="OkIf(bool,Error)" />
    /// <seealso cref="OkIf{T}(bool,T,Func{Error})" />
    public static Result<T> OkIf<T>(
        bool condition,
        T value,
        Error error) =>
        condition ? Ok(value) : Fail<T>(error);

    /// <typeparam name="T">Generic type of the result.</typeparam>
    /// <param name="value">Value to associate with successful result.</param>
    /// <param name="errorFactory">A <see cref="Func{T}" />, returning an <see cref="Error" />.</param>
    /// <inheritdoc cref="OkIf(bool,Func{Error})" />
    /// <seealso cref="OkIf{T}(bool,T,Error)" />
    public static Result<T> OkIf<T>(
        bool condition,
        T value,
        Func<Error> errorFactory) =>
        condition ? Ok(value) : Fail<T>(errorFactory());

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> if the <paramref name="value" /> is not
    ///     <see langword="null" />,
    ///     otherwise - failure is returned.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">Value to check.</param>
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    /// <returns>A <see cref="Result{T}" />, dependent on the <paramref name="value" />.</returns>
    /// <seealso cref="OkIfNotNull{T}(T,Func{Error})" />
    public static Result<T> OkIfNotNull<T>(T? value, Error error)
        where T : notnull =>
        value is null
            ? Fail<T>(error)
            : Ok(value);

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> if the <paramref name="value" /> is not
    ///     <see langword="null" />,
    ///     otherwise - failure is returned.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">Value to check.</param>
    /// <param name="errorFactory">
    ///     A <see cref="Func{T}" />, returning an <see cref="Error" /> instance.
    /// </param>
    /// <returns>A <see cref="Result{T}" />, dependent on the <paramref name="value" />.</returns>
    /// <seealso cref="OkIfNotNull{T}(T,Error)" />
    public static Result<T> OkIfNotNull<T>(T? value, Func<Error> errorFactory)
        where T : notnull =>
        value is null
            ? Fail<T>(errorFactory())
            : Ok(value);

    /// <summary>
    ///     Represents a failed operation.
    /// </summary>
    /// <param name="error">
    ///     An <see cref="Error" /> to be associated with the the <see cref="Result{T}" />.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result{T}" />.
    /// </returns>
    public static Result<Unit> Fail(Error error) => new(ImmutableList.Create(error));

    /// <param name="errors">
    ///     A collection of <see cref="Error" />s to be passed to the <see cref="Result{T}" />.
    /// </param>
    /// <inheritdoc cref="Fail(Error)" />
    public static Result<Unit> Fail(IEnumerable<Error> errors) => new(errors.ToImmutableList());

    /// <typeparam name="T">Generic type of the result.</typeparam>
    /// <inheritdoc cref="Fail(Error)" />
    public static Result<T> Fail<T>(Error error) => new(ImmutableList.Create(error));

    /// <typeparam name="T">Generic type of the result.</typeparam>
    /// <inheritdoc cref="Fail(IEnumerable{Error})" />
    public static Result<T> Fail<T>(IEnumerable<Error> errors) => new(errors.ToImmutableList());

    /// <summary>
    ///     Creates a failed <see cref="Result{T}" /> if the
    ///     <paramref name="condition" /> is <see langword="true" />,
    ///     otherwise - successful result is returned.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    /// <returns>
    ///     A <see cref="Result{T}" />,
    ///     dependent on the <paramref name="condition" />.
    /// </returns>
    /// <seealso cref="FailIf(bool, Func{Error})" />
    public static Result<Unit> FailIf(bool condition, Error error) => condition ? Fail(error) : Ok();

    /// <summary>
    ///     Creates a failed <see cref="Result{T}" /> if the
    ///     <paramref name="condition" /> is <see langword="true" />,
    ///     otherwise - successful result is returned.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorFactory">
    ///     A <see cref="Func{T}" />, returning an <see cref="Error" />
    ///     which should be associated with a failed <see cref="Result{T}" />.
    /// </param>
    /// <inheritdoc cref="FailIf(bool, Error)" />
    /// <seealso cref="FailIf(bool, Error)" />
    public static Result<Unit> FailIf(bool condition, Func<Error> errorFactory) =>
        condition ? Fail(errorFactory()) : Ok();

    /// <typeparam name="T">Generic type of the result.</typeparam>
    /// <param name="condition">Condition to check.</param>
    /// <param name="value">Value to associate with successful result.</param>
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    /// <inheritdoc cref="FailIf(bool,Error)" />
    public static Result<T> FailIf<T>(
        bool condition,
        T value,
        Error error) =>
        condition ? Fail<T>(error) : Ok(value);

    /// <typeparam name="T">Generic type of the result.</typeparam>
    /// <param name="condition">Condition to check.</param>
    /// <param name="value">Value to associate with successful result.</param>
    /// <param name="errorFactory"></param>
    /// <inheritdoc cref="FailIf(bool,Func{Error})" />
    public static Result<T> FailIf<T>(
        bool condition,
        T value,
        Func<Error> errorFactory) =>
        condition ? Fail<T>(errorFactory()) : Ok(value);

    /// <inheritdoc cref="Transpose{T}(IEnumerable{Result{T}})" />
    public static Result<ImmutableList<T>> Transpose<T>(params Result<T>[] results) =>
        Transpose((IReadOnlyCollection<Result<T>>)results);

    /// <summary>
    ///     Transposes a collection of <see cref="Result{T}" />s,
    ///     aggregating the values into an <see cref="ImmutableList{T}" />.
    ///     If at least one of the <paramref name="results" /> is failed, then the whole computation is failed,
    ///     and the <see cref="Result{T}.Errors" /> will be aggregated.
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
    public static Result<ImmutableList<T>> Transpose<T>(IEnumerable<Result<T>> results)
    {
        if (!results.Any())
        {
            return Ok(ImmutableList<T>.Empty);
        }

        var errorsBuilder = ImmutableList.CreateBuilder<Error>();
        var valuesBuilder = ImmutableList.CreateBuilder<T>();
        var isErrored = false;

        foreach (var result in results)
        {
            if (result.Some is var (x))
            {
                valuesBuilder.Add(x);
            }
            else
            {
                isErrored = true;
                errorsBuilder.AddRange(result.Errors);
            }
        }

        return isErrored
            ? new(errorsBuilder.ToImmutable())
            : new(valuesBuilder.ToImmutable());
    }

    /// <inheritdoc cref="Merge(IReadOnlyCollection{Result{Unit}})" />
    public static Result<Unit> Merge(params Result<Unit>[] results) => Merge(Array.AsReadOnly(results));

    /// <summary>
    ///     Merges all <paramref name="results" />
    ///     into one <see cref="Result{T}" />,
    ///     aggregating all errors and successes into one <see cref="Result{T}" />.
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
        var isErrored = false;

        foreach (var result in results.Where(static r => r.Some is null))
        {
            isErrored = true;
            errorsBuilder.AddRange(result.Errors);
        }

        return isErrored
            ? new(errorsBuilder.ToImmutable())
            : new(Unit.Value);
    }

    /// <summary>
    ///     Attempts to perform a <paramref name="func" />;
    ///     if no exception is thrown, a successful
    ///     <see cref="Result{T}" /> is returned.
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
    ///     Attempts to run <paramref name="func" />
    ///     asynchronously; if no exception is thrown,
    ///     <see cref="Result{T}" /> of <see cref="Unit" /> is returned.
    ///     Otherwise, a failed <see cref="Result{T}" /> is returned,
    ///     with an error instance built from <paramref name="catchHandler" />.
    /// </summary>
    /// <param name="func">
    ///     An asynchronous delegate to execute.
    /// </param>
    /// <param name="catchHandler">
    ///     A delegate which returns an instance of an <see cref="Error" />
    ///     if an exception is thrown.
    /// </param>
    /// <returns>
    ///     Successful <see cref="Result{T}" /> of <see cref="Unit" /> if no exception was thrown,
    ///     otherwise - a failure.
    /// </returns>
    /// <remarks>
    ///     The default <paramref name="catchHandler" />
    ///     returns an <see cref="ExceptionalError" />,
    ///     which contains the thrown exception in the
    ///     <see cref="ExceptionalError.CausedBy" /> property.
    /// </remarks>
    public static async ValueTask<Result<Unit>> TryAsync(
        Func<ValueTask> func,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

        try
        {
            await func();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail(catchHandler(e));
        }
    }

    /// <summary>
    ///     Attempts to run <paramref name="func" />
    ///     asynchronously; if no exception is thrown,
    ///     <see cref="Result{T}" /> of <typeparamref name="T" /> is returned.
    ///     Otherwise, a failed <see cref="Result{T}" /> is returned,
    ///     with an error instance built from <paramref name="catchHandler" />.
    /// </summary>
    /// <param name="func">
    ///     An asynchronous delegate to execute.
    /// </param>
    /// <param name="catchHandler">
    ///     A delegate which returns an instance of an <see cref="Error" />
    ///     if an exception is thrown.
    /// </param>
    /// <returns>
    ///     Successful <see cref="Result{T}" /> of <see cref="Unit" /> if no exception was thrown,
    ///     otherwise - a failure.
    /// </returns>
    /// <remarks>
    ///     The default <paramref name="catchHandler" />
    ///     returns an <see cref="ExceptionalError" />,
    ///     which contains the thrown exception in the
    ///     <see cref="ExceptionalError.CausedBy" /> property.
    /// </remarks>
    public static async ValueTask<Result<T>> TryAsync<T>(
        Func<ValueTask<T>> func,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

        try
        {
            return Ok(await func());
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }

    private static Error ExceptionHandler(Exception exception) => new ExceptionalError(exception);
}