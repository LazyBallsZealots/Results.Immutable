namespace Results.Immutable.Tests.ResultTests;

public sealed class ResultTests
{
    [Fact(DisplayName = "Constructs an errored result when the public constructor is used")]
    public void ConstructsAnErroredResultWhenThePublicConstructorIsUsed()
    {
        var result = new Result<Unit>();

        result.Should()
            .BeEquivalentTo(Result.Fail<Unit>(new Error("Constructed result")));
    }

    [Fact(DisplayName = "Checks if the result has an error of a specific type")]
    public void ChecksIfTheResultHasAnErrorOfType()
    {
        var result = Result.Fail(new ErrorA("Hello"));

        result.HasError<ErrorA>()
            .Should()
            .BeTrue();

        result.HasError<ErrorB>()
            .Should()
            .BeFalse();
    }

    [Fact(DisplayName = "Checks if the result has an error filtered by a predicate")]
    public void ChecksIfTheResultHasAnErrorFilteredByAPredicate()
    {
        var result = Result.Fail(new ErrorA("Hello"));

        result.HasError(static (Error e) => e is ErrorA)
            .Should()
            .BeTrue();

        result.HasError(static (Error e) => e is ErrorB)
            .Should()
            .BeFalse();
    }

    [Fact(DisplayName = "Matches an OK result and runs an action")]
    public void MatchesAnOkResultAndRunAnAction()
    {
        var result = Result.Ok();

        var ran = false;
        result.Match(
            _ => { ran = true; },
            _ => { ran = false; });

        ran.Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Matches an OK result, runs an action and returns the value")]
    public void MatchesAnOkErrorAndRunAnActionAndReturnsTheValue()
    {
        var result = Result.Ok("Hello");

        var value = result.Match(
            static greeting => $"{greeting} Steve",
            static _ => "Not OK");

        value.Should()
            .Be("Hello Steve");
    }

    [Fact(DisplayName = "Replaces the errors and forget the type error but keep down-casting possible")]
    public void ReplacesTheErrorsAndForgetTheTypeErrorButKeepDownCastingPossible()
    {
        var result = Result.Fail(new ErrorA("Hello"));
        var boxed = result.WithErrors(ImmutableList.Create(new Error("Good bye")));
        ((Result<Unit>)boxed).Should()
            .BeEquivalentTo(Result.Fail(new Error("Good bye")));
    }

    private sealed record ErrorA : Error
    {
        public ErrorA(string message)
            : base(message)
        {
        }
    }

    private abstract record ErrorB : Error
    {
        protected ErrorB(string message)
            : base(message)
        {
        }
    }
}