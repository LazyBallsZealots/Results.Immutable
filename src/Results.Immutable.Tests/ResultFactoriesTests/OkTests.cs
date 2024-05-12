using FsCheck.Fluent;
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
            ArbMap.Default.GeneratorFor<object?>()
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

    [Fact(DisplayName = "OkIf returns a successful generic result if the condition is true")]
    public void OkIfReturnsGenericSuccessfulResultIfConditionIsTrue()
    {
        const int value = 1;

        Result.OkIf(
                true,
                value,
                new Error("unreachable"))
            .Should()
            .ContainValue()
            .Which
            .Should()
            .Be(value);
    }

    [Fact(
        DisplayName = "OkIf returns a successful generic result with lazily evaluated error if the condition is true")]
    public void OkIfReturnsASuccessfulResultWithLazilyEvaluatedErrorIfTheConditionIsTrue()
    {
        const int value = 1;

        var fn = new Fn<Error>(() => new("unreachable"));

        Result.OkIf(
                true,
                value,
                fn.Callable)
            .Should()
            .ContainValue()
            .Which
            .Should()
            .Be(value);

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "OkIf returns a failed generic result when the condition is false")]
    public void OkIfReturnsAFailedResultWithAMatchingErrorIfTheConditionIsFalse()
    {
        var error = new Error("whoops!");

        Result.OkIf(
                false,
                1,
                error)
            .Should()
            .ContainErrors()
            .Which
            .Should()
            .ContainSingle(single => single.Equals(error));
    }

    [Fact(DisplayName = "OkIf returns a failed generic result with lazily evaluated error when the condition is false")]
    public void OkIfReturnsFailedGenericResultWithAMatchingErrorWhenTheConditionIsFalse()
    {
        var error = new Error("lazy whoopsie!");

        var fn = new Fn<Error>(() => error);

        Result.OkIf(
                false,
                1,
                fn.Callable)
            .Should()
            .ContainErrors()
            .Which
            .Should()
            .ContainSingle(single => single.Equals(error));

        fn.CallCount.Should()
            .Be(1);
    }

    [Fact(DisplayName = "OkIfNotNull with error message returns successful result if the value is not null")]
    public void OkIfNotNullWithErrorMessageReturnsSuccessfulResultIfTheValueIsNotNull() =>
        Result.OkIfNotNull("kilo", "")
            .Should()
            .ContainValue()
            .Which.Should()
            .Be("kilo");

    [Fact(DisplayName = "OkIfNotNull with error message returns failed result if the value is null")]
    public void OkIfNotNullWithErrorMessageReturnsFailedResultIfTheValueIsNull() =>
        Result.OkIfNotNull(null as string, "pound")
            .Should()
            .ContainErrors()
            .Which.Should()
            .ContainSingle()
            .Which.Should()
            .Be(new Error("pound"));

    [Fact(DisplayName = "OkIfNotNull with error returns successful result if the value is not null")]
    public void OkIfNotNullWithErrorReturnsSuccessfulResultIfTheValueIsNotNull() =>
        Result.OkIfNotNull("kilo", new Error("pound"))
            .Should()
            .ContainValue()
            .Which.Should()
            .Be("kilo");

    [Fact(DisplayName = "OkIfNotNull with error returns failed result if the value is null")]
    public void OkIfNotNullWithErrorReturnsFailedResultIfTheValueIsNull() =>
        Result.OkIfNotNull(null as string, new Error("pound"))
            .Should()
            .ContainErrors()
            .Which.Should()
            .ContainSingle()
            .Which.Should()
            .Be(new Error("pound"));

    [Fact(DisplayName = "OkIfNotNull with error factory returns successful result if the value is not null")]
    public void OkIfNotNullWithErrorFactoryReturnsSuccessfulResultIfTheValueIsNotNull()
    {
        var fn = new Fn<Error>(() => new("pound"));
        Result.OkIfNotNull("kilo", fn)
            .Should()
            .ContainValue()
            .Which.Should()
            .Be("kilo");

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "OkIfNotNull with error factory returns failed result if the value is null")]
    public void OkIfNotNullWithErrorFactoryReturnsFailedResultIfTheValueIsNull()
    {
        Result.OkIfNotNull(null as string, () => new("pound"))
            .Should()
            .ContainErrors()
            .Which.Should()
            .ContainSingle()
            .Which.Should()
            .Be(new Error("pound"));
    }
}