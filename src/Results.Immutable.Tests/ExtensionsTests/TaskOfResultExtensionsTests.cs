using Results.Immutable.Extensions;
using static FluentAssertions.FluentActions;

namespace Results.Immutable.Tests.ExtensionsTests;

public sealed class TaskOfResultExtensionsTests
{
    private static Task<Result<Unit>> SuccessfulAsyncResult => Task.FromResult(Result.Ok());

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

        (await Awaiting(
                    () => Task.FromResult(fail)
                        .Select(ThrowException))
                .Should()
                .NotThrowAsync())
            .Which
            .Should()
            .Match<Result<Unit>>(
                static r => r.HasFailed &&
                    r.Errors.Single()
                        .Message ==
                    errorMessage);

        static Unit ThrowException(Unit _) =>
            throw new InvalidOperationException("Projection on a failed result was executed!");
    }

    [Fact(DisplayName = "SelectMany on a task of successful result with value converts it")]
    public async Task SelectManyOnASuccessfulResultWithValueConvertsTheValue()
    {
        const int firstValue = 69;
        const int finalValue = 420;

        var result = await RunFirstTask()
            .SelectMany(RunSecondTask);

        result.Should()
            .Be(Result.Ok(finalValue));

        static Task<Result<int>> RunFirstTask() => Task.FromResult(Result.Ok(firstValue));

        static Task<Result<int>> RunSecondTask(int _) => Task.FromResult(Result.Ok(finalValue));
    }

    [Fact(DisplayName = "No-op asynchronous SelectMany should return a result of unit")]
    public async Task NoOpProjectionOfMultipleResultsShouldReturnResultOfUnit() =>
        (await SuccessfulAsyncResult.SelectMany(
            static _ => Result.Ok(),
            static (_, _) => Unit.Value))
        .Should()
        .Be(Result.Ok());

    [Fact(DisplayName = "No-op asynchronous SelectMany should return a result of unit")]
    public async Task NoOpProjectionOfMultipleAsyncResultsShouldReturnResultOfUnit() =>
        (await SuccessfulAsyncResult.SelectMany(
            static _ => Task.FromResult(Result.Ok()),
            static (_, _) => Unit.Value))
        .Should()
        .Be(Result.Ok());
}