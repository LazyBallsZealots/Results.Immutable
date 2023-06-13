using FsCheck;
using FsCheck.Xunit;
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

    [Property(
        DisplayName = "Projecting two results should return a success or a failure, depending on initial results")]
    public Property BindingOnTwoSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue() =>
        Prop.ForAll(
            Generator.OfResult()
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return (first, second) switch
                {
                    ({ IsOk: true, }, { IsOk: true, }) => first.SelectMany(_ => second, selector) is
                        {
                            Some.Value: var selectedValue,
                        } &&
                        selectedValue?.Equals(finalValue) is true or null,
                    ({ IsErrored: true, }, _) => first.SelectMany(_ => second, selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    (_, _) => first.SelectMany(_ => second, selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "Second errored out"),
                };
            });

    private static class Generator
    {
        public static Gen<(Result<object?> FirstResult, Result<object?> SecondResult,
                Func<object?, object?, object?> Selector, object? FinalValue)>
            OfResult() =>
            Arb.Generate<Tuple<object?, object?, Func<object?, object?, object?>>>()
                .SelectMany(
                    static tuple =>
                    {
                        var (first, second, _) = tuple;
                        const string firstErrorMessage = "First errored out";
                        const string secondErrorMessage = "Second errored out";

                        return Gen.OneOf(
                            Gen.Fresh(() => (First: Result.Ok(first), Second: Result.Ok(second))),
                            Gen.Fresh(
                                () => (First: Result.Fail<object?>(firstErrorMessage), Second: Result.Ok(second))),
                            Gen.Fresh(
                                () => (First: Result.Ok(first), Second: Result.Fail<object?>(secondErrorMessage))),
                            Gen.Fresh(
                                static () => (First: Result.Fail<object?>("First errored out"),
                                    Second: Result.Fail<object?>("Second errored out"))));
                    },
                    static (funcTuple, resultsTuple) =>
                    {
                        var ((first, second, selector), (result1, result2)) = (funcTuple, resultsTuple);

                        return (FirstResult: result1, SecondResult: result2, Selector: selector,
                            FinalValue: selector(first, second));
                    });
    }
}