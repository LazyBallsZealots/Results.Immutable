using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using Results.Immutable.Tests.Generators;
using static FluentAssertions.FluentActions;

namespace Results.Immutable.Tests.ResultTests;

public sealed class ResultProjectionTests
{
    private static Result<Unit> SuccessfulResult => Result.Ok();

    [Fact(DisplayName = "No-op projection of a successful result of unit should return a result of unit")]
    public void NoOpBindOnSuccessfulResultShouldReturnAUnit() =>
        SuccessfulResult.Select(static _ => Unit.Value)
            .Should()
            .Be(SuccessfulResult);

    [Fact(
        DisplayName =
            "Projecting a failed result should return a new, equivalent result without executing the selector")]
    public void BindOnFailedResultShouldReturnANewEquivalentFailedResult()
    {
        const string errorMessage = "An error";
        var fail = Result.Fail(errorMessage);

        Invoking(() => fail.Select(ThrowException))
            .Should()
            .NotThrow()
            .Which
            .Should()
            .Match<Result<Unit>>(
                static r => r.IsErrored &&
                    r.Errors.Single()
                        .Message ==
                    errorMessage);

        static Unit ThrowException(Unit _) =>
            throw new InvalidOperationException("Projection on a failed result was executed");
    }

    [Property(DisplayName = "SelectMany on successful result with value converts it")]
    public Property BindOnSuccessfulResultWithValueConvertsIt(object? firstValue, object? finalValue) =>
        (Result.Ok(firstValue)
                    .SelectMany(_ => Result.Ok(finalValue))
                is { IsOk: true, Some.Value: var value, } &&
            value?.Equals(finalValue) is null or true)
        .ToProperty();

    [Fact(DisplayName = "No-op SelectMany should return result of unit")]
    public void NoOpBindingOnMultipleResultsShouldReturnResultOfUnit() =>
        SuccessfulResult.SelectMany(
                static _ => Result.Ok(),
                static (_, _) => Unit.Value)
            .Should()
            .Be(SuccessfulResult);

    [Property(DisplayName = "Projecting two successful results should return a success")]
    public Property BindingOnTwoSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasSucceeded && tuple.SecondResult.HasSucceeded)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return first.SelectMany(_ => second, selector) is { Some.Value: var selectedValue, } &&
                    selectedValue?.Equals(finalValue) is true or null;
            });

    [Property(
        DisplayName =
            "Projecting two results, of which first is a failure, returns a failure indicating the first result has failed")]
    public Property ProjectingAFailureAndASuccessShouldReturnFailure() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasFailed)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, selector, _) = tuple;

                return first.SelectMany(_ => second, selector) is var failedResult &&
                    failedResult.HasError(static (Error e) => e.Message == "First errored out");
            });

    [Property(
        DisplayName =
            "Projecting two results, of which second is a failure, returns a failure indicating the second result has failed")]
    public Property ProjectionASuccessAndAFailureShouldReturnFailure() =>
        Prop.ForAll(
            SelectMany.Generator()
                .Where(static tuple => tuple.FirstResult.HasSucceeded && tuple.SecondResult.HasFailed)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, selector, _) = tuple;

                return first.SelectMany(_ => second, selector) is var failedResult &&
                    failedResult.HasError(static (Error e) => e.Message == "Second errored out");
            });
}