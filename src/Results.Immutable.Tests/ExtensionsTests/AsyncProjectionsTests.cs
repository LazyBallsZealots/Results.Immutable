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

    [Fact(DisplayName = "Projecting a successful async result should return a new result with matching value")]
    public async Task ProjectionOfASuccessfulResultShouldReturnAResultWithMatchingValue()
    {
        const int newValue = 2137;
        var result = await SuccessfulAsyncResult.SelectAsync(static _ => new ValueTask<int>(newValue));

        result.Should()
            .Match<Result<int>>(static r => r.Some!.Value.Value == newValue);
    }

    [Fact(DisplayName = "Asynchronous no-op projection of a successful result of unit should return a result of unit")]
    public async Task AsynchronousNoOpProjectionOfASuccessfulResultShouldReturnAResultOfUnit() =>
        (await SuccessfulAsyncResult.SelectAsync(_ => new ValueTask<Unit>(Unit.Value)))
        .Should()
        .BeEquivalentTo(Result.Ok());

    [Fact(DisplayName = "No-op asynchronous projection of multiple async results should return a result of unit")]
    public async Task NoOpProjectionOfMultipleResultsShouldReturnResultOfUnit() =>
        (await SuccessfulAsyncResult.SelectManyAsync(static _ => new ValueTask<Result<Unit>>(Result.Ok())))
        .Should()
        .Be(Result.Ok());

    [Property(DisplayName = "Projecting two asynchronous results should return a matching result")]
    public Property ProjectingTwoAsyncResultsShouldReturnAMatchingResultBasedOnTheSecondResult() =>
        Prop.ForAll(
            SelectMany.Generator()
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, _, _) = tuple;
                var combination = await CombineAsync(first, second);

                return (first, second) switch
                {
                    ({ IsOk: true, }, { Some.Value: var finalValue, }) =>
                        combination is { Some.Value: var value, } &&
                        value?.Equals(finalValue) is true or null,
                    ({ HasFailed: true, }, _) => combination is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    _ => combination is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "Second errored out"),
                };
            });

    [Property(
        DisplayName =
            "Projecting asynchronous and synchronous results should return a matching result based on final selector")]
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
            "Projecting asynchronous and synchronous results should return a matching result based on final selector (query syntax)")]
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
                    from intermediateValue in second
                    select selector(initialValue, intermediateValue);
            });

    [Property(DisplayName = "Projecting two asynchronous results should return a value based on the final selector")]
    public Property ProjectingTwoAsyncResultsShouldReturnAMatchingResultWithValueBasedOnTheFinalSelector() =>
        Prop.ForAll(
            SelectMany.Generator()
                .ToArbitrary(),
            static async tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return (first, second) switch
                {
                    ({ IsOk: true, }, { IsOk: true, }) => await CombineAsync(
                            first,
                            second,
                            selector) is { Some.Value: var value, } &&
                        value?.Equals(finalValue) is null or true,
                    ({ HasFailed: true, }, _) => await CombineAsync(
                            first,
                            second,
                            selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    _ => await CombineAsync(
                            first,
                            second,
                            selector) is var failure &&
                        failure.HasError<Error>(static e => e.Message == "Second errored out"),
                };
            });

    private static async ValueTask<Result<object?>> Combine(
        Result<object?> first,
        Result<object?> second,
        Func<object?, object?, object?>? selector = null) =>
        selector is null
            ? await ToValueTask(first)
                .SelectMany(_ => second)
            : await ToValueTask(first)
                .SelectMany(
                    _ => second,
                    selector);

    private static async ValueTask<Result<object?>> CombineAsync(
        Result<object?> first,
        Result<object?> second,
        Func<object?, object?, object?>? selector = null) =>
        selector is null
            ? await ToValueTask(first)
                .SelectManyAsync(_ => ToValueTask(second))
            : await ToValueTask(first)
                .SelectManyAsync(_ => ToValueTask(second), selector);

    private static ValueTask<Result<object?>> ToValueTask(Result<object?> result) => new(result);
}