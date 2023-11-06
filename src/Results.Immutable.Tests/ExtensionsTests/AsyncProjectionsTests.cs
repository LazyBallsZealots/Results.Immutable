using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using Results.Immutable.Extensions;
using Results.Immutable.Tests.Generators;

namespace Results.Immutable.Tests.ExtensionsTests;

public sealed class AsyncProjectionsTests
{
    private static ValueTask<Result<Unit>> SuccessfulAsyncResult => new(Result.Ok());

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

        var result = await new ValueTask<Result<Unit>>(fail)
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
        (await SuccessfulAsyncResult.SelectMany(static _ => new ValueTask<Result<Unit>>(Result.Ok())))
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
            "Projecting two asynchronous results should return a matching result based on final selector")]
    public Property ProjectingTwoAsyncResultsShouldReturnAMatchingResultBasedOnFinalSelector() =>
        Prop.ForAll(
            SelectMany.Generator()
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return (first, second) switch
                {
                    ({ IsOk: true, }, { IsOk: true, }) => await Combine(
                            first,
                            second,
                            selector) is { Some.Value: var value, } &&
                        value?.Equals(finalValue) is true or null,
                    ({ HasFailed: true, }, _) => await Combine(
                            first,
                            second,
                            selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    _ => await Combine(
                            first,
                            second,
                            selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "Second errored out"),
                };
            });

    [Property(
        DisplayName =
            "Projecting two asynchronous results should return a matching result based on final selector (query syntax)")]
    public Property ProjectingTwoAsyncResultsWithQuerySyntaxShouldReturnAMatchingResultBasedOnFinalSelector() =>
        Prop.ForAll(
            SelectMany.Generator()
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return (first, second) switch
                {
                    ({ IsOk: true, }, { IsOk: true, }) => await CombineWithQuery() is { Some.Value: var value, } &&
                        value?.Equals(finalValue) is true or null,
                    ({ HasFailed: true, }, _) => await CombineWithQuery() is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    _ => await CombineWithQuery() is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "Second errored out"),
                };

                ValueTask<Result<object?>> CombineWithQuery() =>
                    from initialValue in ToValueTask(first)
                    from intermediateValue in ToValueTask(second)
                    select selector(initialValue, intermediateValue);
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

    private static ValueTask<Result<object?>> ToValueTask(Result<object?> result) => new(result);
}