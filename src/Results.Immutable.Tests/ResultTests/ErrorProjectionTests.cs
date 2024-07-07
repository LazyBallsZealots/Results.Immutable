namespace Results.Immutable.Tests.ResultTests;

public sealed class ErrorProjectionTests
{
    private static readonly Result<Unit> SuccessfulResult = Result.Ok();

    private static readonly Result<Unit> FailedResult = Result.Fail(
        ImmutableList.Create(
            new Error(
                "First top-level",
                new SubError("Root cause")),
            new Error("Second top-level error")));

    [Fact(DisplayName = "SelectErrors on a successful result is a no-op")]
    public void SelectErrorsOnASuccessfulResultIsANoOp()
    {
        var errorsProjection = new Fn<Error, SubError>(error => new(error.Message, error.InnerErrors));

        SuccessfulResult.SelectErrors(errorsProjection.Callable)
            .Should()
            .Be(SuccessfulResult);

        errorsProjection.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "MergeErrorsWith on a successful result is a no-op")]
    public void MergeErrorsWithOnASuccessfulResultIsANoOp()
    {
        var errorsCombinator =
            new Fn<ImmutableList<Error>, SubError>(static errors => new("Welp, this should not work", errors));

        SuccessfulResult.MergeErrorsWith(errorsCombinator.Callable)
            .Should()
            .Be(SuccessfulResult);

        errorsCombinator.CallCount
            .Should()
            .Be(0);
    }

    [Fact(DisplayName = "SelectErrors on a failed result projects all top-level errors")]
    public void SelectErrorsOnAFailedResultProjectsAllRootErrors()
    {
        var errorsSelector =
            new Fn<Error, Error>(e => new($"Original message: {e.Message}", ImmutableList<Error>.Empty));

        var resultWithProjectedErrors = FailedResult.SelectErrors(errorsSelector.Callable);

        resultWithProjectedErrors.Should()
            .NotBe(FailedResult);

        errorsSelector.CallCount.Should()
            .Be((uint)FailedResult.Errors.Count);

        resultWithProjectedErrors.HasError<SubError>()
            .Should()
            .BeFalse();
    }

    [Fact(DisplayName = "MergeErrorsWith on a failed result combines all top-level errors")]
    public void MergeErrorsWithOnAFailedResultCombinesAllTopLevelErrors()
    {
        var errorsCombinator = new Fn<ImmutableList<Error>, Error>(
            es => new SubError(
                "By your powers combined, I fail this operation!",
                es));

        var resultWithZippedErrors = FailedResult.MergeErrorsWith(errorsCombinator.Callable);

        resultWithZippedErrors
            .Should()
            .NotBe(FailedResult);

        errorsCombinator.CallCount.Should()
            .Be(1);

        resultWithZippedErrors.Errors.Should()
            .SatisfyRespectively(
                single => single.InnerErrors.Should()
                    .BeEquivalentTo(FailedResult.Errors));
    }

    private sealed record SubError(string Message, ImmutableList<Error> InnerErrors) : Error(Message, InnerErrors)
    {
        public SubError(string message)
            : this(message, ImmutableList<Error>.Empty)
        {
        }
    }
}