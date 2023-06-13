using FsCheck;
using FsCheck.Xunit;
using static Results.Immutable.Tests.ResultFactoriesTests.ResultMatching;

namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class OkTests
{
    [Fact(DisplayName = "Creates a successful result of unit")]
    public void ShouldCreateSuccessfulResult()
    {
        var unit = Unit.Value;
        Result.Ok()
            .Should()
            .Match<Result<Unit>>(r => r.IsOk && r.Some!.Value.Value == unit);
    }

    [Property(DisplayName = "Creates a successful result of provided type")]
    public void ShouldCreateSuccessfulResultOfProvidedType() =>
        Prop.ForAll(
            Arb.Generate<object?>()
                .ToArbitrary(),
            static obj =>
            {
                var result = Result.Ok(obj);
                return result is { IsOk: true, Some.Value: var value, } && value == obj;
            });

    [Fact(DisplayName = "OkIf returns successful result if the condition is true")]
    public void OkIfReturnsSuccessfulResultIfTheConditionIsTrue() =>
        Result.OkIf(true, string.Empty)
            .Should()
            .Match<Result<Unit>>(static r => r.HasSucceeded && ValueIsAUnit(r));

    [Fact(DisplayName = "OkIf returns a failed result with a matching error if the condition is false")]
    public void OkIfReturnsSuccessfulResultIfTheConditionIsFalse()
    {
        const string errorMessage = "An error";

        Result.OkIf(false, errorMessage)
            .Should()
            .Match<Result<Unit>>(static r => r.HasFailed && r.HasError<Error>(static e => e.Message == errorMessage));
    }

    [Fact(DisplayName = "OkIf returns a failed result with a matching, typed error if the condition is false")]
    public void OkIfReturnsAFailedResultWithAMatchingTypedErrorIfTheConditionIsFalse()
    {
        const string errorMessage = "An error";

        Result.OkIf(
                false,
                new RootError(errorMessage))
            .Should()
            .Match<Result<Unit>>(
                static r => r.HasFailed && r.HasError<RootError>(static e => e.Message == errorMessage));
    }

    [Fact(DisplayName = "OkIf returns successful result without calling error factory if the condition is true")]
    public void OkIfReturnsSuccessfulResultWithoutCallingErrorMessageFactoryIfTheConditionIsTrue()
    {
        Result.OkIf(
                true,
                GetErrorMessage)
            .Should()
            .Match<Result<Unit>>(static r => r.HasSucceeded && ValueIsAUnit(r));

        static string GetErrorMessage() =>
            throw new InvalidOperationException("Lazy OkIf overload instantiated an error for successful result!");
    }

    [Fact(
        DisplayName = "OkIf returns a failed result with a matching, lazily evaluated error if the condition is false")]
    public void OkIfReturnsAFailureWithAMatchingLazilyEvaluatedErrorIfTheConditionIsFalse()
    {
        const string errorMessage = "An error";

        Result.OkIf(
                false,
                static () => errorMessage)
            .Should()
            .Match<Result<Unit>>(static r => r.HasFailed && r.HasError<Error>(static e => e.Message == errorMessage));
    }

    [Fact(
        DisplayName =
            "OkIf returns a failed result with a matching, lazily evaluated and typed error if the condition is false")]
    public void OkIfReturnsAFailedResultWithAMatchingLazilyEvaluatedAndTypedErrorIfTheConditionIsFalse()
    {
        const string errorMessage = "An error";

        Result.OkIf(
                false,
                static () => new RootError(errorMessage))
            .Should()
            .Match<Result<Unit>>(
                static r => r.HasFailed && r.HasError<RootError>(static e => e.Message == errorMessage));
    }
}