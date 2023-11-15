using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using Results.Immutable.Extensions;
using Results.Immutable.Tests.Generators;

namespace Results.Immutable.Tests.ExtensionsTests;

public sealed class AsyncProjectionsTests
{
    private static ValueTask<Result<Unit>> SuccessfulAsyncResult => ValueTask.FromResult(Result.Ok());

    [Fact(DisplayName = "No-op projection of a task of successful result of unit should return a result of unit")]
    public async Task NoOpProjectionOfASuccessfulTaskOfUnitShouldReturnAResultOfUnit() =>
        (await SuccessfulAsyncResult.Select(static _ => Unit.Value))
        .Should()
        .BeEquivalentTo(Result.Ok());

    [Fact(DisplayName = "Projecting a failed result should return a new, equivalent result without ")]
    public async Task ProjectionOnAFailedResultShouldReturnANewEquivalentFailedResult()
    {
        const string errorMessage = "An error";
        var fail = Result.Fail(errorMessage);

        var result = await ValueTask.FromResult(fail)
            .Select(ThrowException);

        result.Should()
            .Match<Result<Unit>>(
                static r => r.HasFailed &&
                    r.Errors.Single()
                        .Message ==
                    errorMessage);

        static Unit ThrowException(Unit _) =>
            throw new InvalidOperationException("Projection on a failed result was executed!");
    }

    [Fact(DisplayName = "No-op asynchronous projection of multiple async results should return a result of unit")]
    public async Task NoOpProjectionOfMultipleResultsShouldReturnResultOfUnit() =>
        (await SuccessfulAsyncResult.SelectMany(static _ => ValueTask.FromResult(Result.Ok())))
        .Should()
        .Be(Result.Ok());

    [Property(
        DisplayName = "Projecting two asynchronous results should return a matching result based on the second result")]
    public Property ProjectingTwoAsyncResultsShouldReturnAMatchingResultBasedOnTheSecondResult() =>
        Prop.ForAll(
            SelectMany.Generator()
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, _, _) = tuple;

                return (first, second) switch
                {
                    ({ IsOk: true, }, { Some.Value: var finalValue, }) =>
                        await Combine(first, second) is { Some.Value: var value, } &&
                        value?.Equals(finalValue) is true or null,
                    ({ HasFailed: true, }, _) => await Combine(first, second) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    _ => await Combine(first, second) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "Second errored out"),
                };
            });

    [Property(
        DisplayName =
            "Projecting two successful asynchronous results should return a successful result based on final selector")]
    public Property ProjectingTwoSuccessfulAsyncResultsShouldReturnASuccess() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasSucceeded && tuple.SecondResult.HasSucceeded)
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return await Combine(
                        first,
                        second,
                        selector) is { Some.Value: var value, } &&
                    value?.Equals(finalValue) is true or null;
            });

    [Property(
        DisplayName =
            "Projecting a failed asynchronous result and another asynchronous result should return a failure")]
    public Property ProjectingAFailedAsynchronousResultAndASecondAsyncResultShouldReturnAFailure() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasFailed)
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, _) = tuple;

                return await Combine(
                        first,
                        second,
                        selector) is var failedResult &&
                    failedResult.HasError<Error>(static e => e.Message == "First errored out");
            });

    [Property(
        DisplayName =
            "Projecting a successful asynchronous result and a failed asynchronous result should return a failure")]
    public Property ProjectingASuccessfulAsynchronousResultAndAFailedAsyncResultShouldReturnAFailure() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasSucceeded && tuple.SecondResult.HasFailed)
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, _) = tuple;

                return await Combine(
                        first,
                        second,
                        selector) is var failedResult &&
                    failedResult.HasError<Error>(static e => e.Message == "Second errored out");
            });

    [Property(
        DisplayName =
            "Projecting two successful asynchronous results should return a successful result based on final selector (query syntax)")]
    public Property ProjectingTwoSuccessfulAsyncResultsWithQuerySyntaxShouldReturnASuccess() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasSucceeded && tuple.SecondResult.HasSucceeded)
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return await CombineWithQuery(
                        first,
                        second,
                        selector) is { Some.Value: var value, } &&
                    value?.Equals(finalValue) is true or null;
            });

    [Property(DisplayName = "Projecting asynchronous failed result and any other async result should return a failure")]
    public Property ProjectingAnAsyncFailedResultAndAnyOtherAsyncResultShouldReturnAFailure() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasFailed)
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, _) = tuple;

                return await CombineWithQuery(
                        first,
                        second,
                        selector) is var failedResult &&
                    failedResult.HasError<Error>(static e => e.Message == "First errored out");
            });

    [Property(
        DisplayName = "Projecting asynchronous successful result and an asynchronous failure should return a failure")]
    public Property ProjectingAsynchronousSuccessAndAsyncFailureShouldReturnAFailure() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasSucceeded && tuple.SecondResult.HasFailed)
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, _) = tuple;

                return await CombineWithQuery(
                        first,
                        second,
                        selector) is var failedResult &&
                    failedResult.HasError<Error>(static e => e.Message == "Second errored out");
            });

    private static async ValueTask<Result<object?>> Combine(
        Result<object?> first,
        Result<object?> second,
        Func<object?, object?, object?>? selector = null) =>
        selector is null
            ? await ToValueTask(first)
                .SelectMany(_ => ToValueTask(second))
            : await ToValueTask(first)
                .SelectMany(
                    _ => ToValueTask(second),
                    selector);

    private static ValueTask<Result<object?>> CombineWithQuery(
        Result<object?> first,
        Result<object?> second,
        Func<object?, object?, object?> selector) =>
        from initialValue in ToValueTask(first)
        from intermediateValue in ToValueTask(second)
        select selector(initialValue, intermediateValue);

    private static ValueTask<Result<object?>> ToValueTask(Result<object?> result) => new(result);
}