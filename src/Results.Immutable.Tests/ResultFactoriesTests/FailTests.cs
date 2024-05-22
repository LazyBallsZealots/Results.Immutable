namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class FailTests
{
    [Fact(DisplayName = "Creates a failed result with one error from error record")]
    public void CreatesAFailedResultWithOneErrorFromErrorRecord()
    {
        var error = new Error("space shuttle blew");

        Result.Fail(error)
            .Should()
            .Match<Result<Unit>>(
                r => r.IsErrored &&
                    r.Errors.Single()
                        .Equals(error));
    }

    [Fact(DisplayName = "Creates a failed result with a collection of errors")]
    public void CreatesAFailedResultWithACollectionOfErrors()
    {
        var errors = ImmutableList.Create(new Error("space shuttle blew"), new Error("rocket exploded"));

        Result.Fail(errors)
            .Should()
            .Match<Result<Unit>>(r => r.IsErrored && r.Errors.SequenceEqual(errors));
    }

    [Fact(DisplayName = "Creates a failed result with a collection of errors as params")]
    public void CreatesAFailedResultWithACollectionOfErrorsAsParams()
    {
        var errors = new[]
        {
            new Error("space shuttle blew"),
            new Error("rocket exploded"),
        };

        Result.Fail(errors)
            .Should()
            .Match<Result<Unit>>(r => r.IsErrored && r.Errors.SequenceEqual(errors));
    }

    [Fact(DisplayName = "Creates a failed result of a provided type with one error from error record")]
    public void CreatesAFailedResultOfProvidedTypeWithOneErrorFromErrorRecord()
    {
        var error = new Error("space shuttle blew");

        Result.Fail<int>(error)
            .Should()
            .Match<Result<int>>(
                r => r.IsErrored &&
                    r.Errors.Single()
                        .Equals(error));
    }

    [Fact(DisplayName = "Creates a failed result of a provided type with a collection of errors")]
    public void CreatesAFailedResultOfProvidedTypeWithACollectionOfErrors()
    {
        var errors = ImmutableList.Create(new Error("space shuttle blew"), new Error("rocket exploded"));

        Result.Fail<int>(errors)
            .Should()
            .Match<Result<int>>(r => r.IsErrored && r.Errors.SequenceEqual(errors));
    }

    [Fact(DisplayName = "Creates a failed result of a provided type with a collection of errors as params")]
    public void CreatesAFailedResultOfProvidedTypeWithACollectionOfErrorsAsParams()
    {
        var errors = new[]
        {
            new Error("space shuttle blew"),
            new Error("rocket exploded"),
        };

        Result.Fail<int>(errors)
            .Should()
            .Match<Result<int>>(r => r.IsErrored && r.Errors.SequenceEqual(errors));
    }

    [Fact(DisplayName = "FailIf returns a failed result with a matching, typed error if the condition is true")]
    public void FailIfReturnsAFailedResultWithAMatchingTypedErrorIfTheConditionIsTrue()
    {
        const string errorMessage = "An error";

        Result.FailIf(
                true,
                new RootError(errorMessage))
            .Should()
            .Match<Result<Unit>>(
                static r => r.HasFailed && r.HasError<RootError>(static e => e.Message == errorMessage));
    }

    [Fact(
        DisplayName =
            "FailIf returns a failed result with a matching, lazily evaluated and typed error if the condition is true")]
    public void FailIfReturnsAFailedResultWithAMatchingLazilyEvaluatedAndTypedErrorIfTheConditionIsTrue()
    {
        const string errorMessage = "An error";

        Result.FailIf(
                true,
                static () => new RootError(errorMessage))
            .Should()
            .Match<Result<Unit>>(
                static r => r.HasFailed && r.HasError<RootError>(static e => e.Message == errorMessage));
    }

    [Fact(DisplayName = "FailIf returns a failed generic result if the condition is true")]
    public void FailIfReturnsAFailedGenericResultIfTheConditionIsTrue()
    {
        var error = new Error("whoopsie!");

        Result.FailIf(
                true,
                1,
                error)
            .Should()
            .Match<Result<int>>(static r => r.HasFailed)
            .And
            .ContainErrors()
            .Which
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    error,
                });
    }

    [Fact(DisplayName = "FailIf returns a failed generic result with lazily evaluated error if the condition is true")]
    public void FailIfReturnsAFailedGenericResultWithLazilyEvaluatedErrorIfTheConditionIsTrue()
    {
        var error = new Error("I'm lazy");

        var fn = new Fn<Error>(() => error);

        Result.FailIf(
                true,
                1,
                fn)
            .Should()
            .Match<Result<int>>(static r => r.HasFailed)
            .And
            .ContainErrors()
            .Which
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    error,
                });

        fn.CallCount.Should()
            .Be(1);
    }

    [Fact(DisplayName = "FailIf returns a successful generic result if the condition is false")]
    public void FailIfReturnsASuccessfulGenericResultIfTheConditionIsFalse()
    {
        const int value = 1;

        Result.FailIf(
                false,
                value,
                new Error("unreachable"))
            .Should()
            .ContainValue()
            .Which
            .Should()
            .Be(value);
    }

    [Fact(
        DisplayName =
            "FailIf returns a successful generic result with lazily evaluated error if the condition is false")]
    public void FailIfReturnsASuccessfulGenericResultWithLazilyEvaluatedErrorIfTheConditionIsFalse()
    {
        const int value = 1;
        var fn = new Fn<Error>(() => new("unreachable"));

        Result.FailIf(
                false,
                value,
                fn)
            .Should()
            .Match<Result<int>>(static r => r.HasSucceeded)
            .And
            .ContainValue()
            .Which
            .Should()
            .Be(value);
    }
}