using Results.Immutable.Metadata;

namespace Results.Immutable;

public readonly partial record struct Result<T>
{
    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="selector" />
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="selector">Bind delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Result{T}" />, wrapping <typeparamref name="TNew" />
    ///     instance.
    /// </returns>
    /// <remarks>
    ///     This overload disregards the <see cref="Option" /> of this
    ///     <see cref="Result{T}" />.
    /// </remarks>
    public Result<TNew> Select<TNew>(Func<Result<TNew>> selector) =>
        IsSuccessful &&
        selector() is { option: var boundValue, reasons: var additionalReasons, IsAFailure: var isAFailure, }
            ? new(
                boundValue,
                MergeReasonsWith(additionalReasons),
                isAFailure)
            : new(reasons, true);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="bindingFunction" />
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="bindingFunction">Bind delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Result{T}" />, wrapping <typeparamref name="TNew" />
    ///     instance.
    /// </returns>
    public Result<TNew> Select<TNew>(Func<Option<T>, Result<TNew>> bindingFunction) =>
        IsSuccessful &&
        bindingFunction(Option) is
        {
            option: var boundValue, reasons: var additionalReasons, IsAFailure: var isAFailure,
        }
            ? new(
                boundValue,
                MergeReasonsWith(additionalReasons),
                isAFailure)
            : new(reasons, true);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncSelector" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncSelector">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    /// <remarks>
    ///     This overload disregards the <see cref="Option" /> of this
    ///     <see cref="Result{T}" />.
    /// </remarks>
    public async Task<Result<TNew>> SelectAsync<TNew>(Func<Task<Result<TNew>>> asyncSelector) =>
        IsSuccessful &&
        await asyncSelector() is { option: var boundValue, reasons: var additionalReasons, IsAFailure: var isAFailure, }
            ? new(
                boundValue,
                MergeReasonsWith(additionalReasons),
                isAFailure)
            : new(reasons, true);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncBindingFunction" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncBindingFunction">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    public async Task<Result<TNew>> SelectAsync<TNew>(Func<Option<T>, Task<Result<TNew>>> asyncBindingFunction) =>
        IsSuccessful &&
        await asyncBindingFunction(Option) is
        {
            option: var boundValue, reasons: var additionalReasons, IsAFailure: var isAFailure,
        }
            ? new(
                boundValue,
                MergeReasonsWith(additionalReasons),
                isAFailure)
            : new(reasons, true);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncSelector" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncSelector">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    /// <remarks>
    ///     This overload disregards the <see cref="Option" /> of this
    ///     <see cref="Result{T}" />.
    /// </remarks>
    public async ValueTask<Result<TNew>> SelectAsync<TNew>(Func<ValueTask<Result<TNew>>> asyncSelector) =>
        IsSuccessful &&
        await asyncSelector() is { option: var boundValue, reasons: var additionalReasons, IsAFailure: var isAFailure, }
            ? new(
                boundValue,
                MergeReasonsWith(additionalReasons),
                isAFailure)
            : new(reasons, true);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncBindingFunction" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncBindingFunction">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    public async ValueTask<Result<TNew>> SelectAsync<TNew>(
        Func<Option<T>, ValueTask<Result<TNew>>> asyncBindingFunction) =>
        IsSuccessful &&
        await asyncBindingFunction(Option) is
        {
            option: var boundValue, reasons: var additionalReasons, IsAFailure: var isAFailure,
        }
            ? new(
                boundValue,
                MergeReasonsWith(additionalReasons),
                isAFailure)
            : new(reasons, true);

    private ImmutableList<Reason>? MergeReasonsWith(ImmutableList<Reason>? reasonsToAdd) =>
        (reasons?.Any(), reasonsToAdd?.Any()) is (true, true) ? reasons.AddRange(reasonsToAdd) : null;
}