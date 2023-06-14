namespace Results.Immutable.Tests.ResultTests;

public sealed class ResultTests
{
    [Fact(DisplayName = "Constructs an errored result when the public constructor is used")]
    public void ConstructsAnErroredResultWhenThePublicConstructorIsUsed()
    {
        var result = new Result<Unit>();

        result.Should()
            .BeEquivalentTo(Result.Fail<Unit>("Constructed result"));
    }

    [Fact(DisplayName = "Adds an error to a successful result")]
    public void AddsAnErrorToASuccessfulResult()
    {
        var original = Result.Ok();

        var result = original.AddError("An error");

        result
            .Should()
            .BeEquivalentTo(Result.Fail("An error"));

        original.IsOk.Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Adds multiple errors to a successful result")]
    public void AddsMultipleErrorsToASuccessfulResult()
    {
        var original = Result.Ok();

        var result = original.AddErrors(
            new[]
            {
                "An error",
                "Another error",
            });

        result
            .Should()
            .BeEquivalentTo(Result.Fail(new("An error"), new("Another error")));

        original.IsOk.Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Adds an error to an errored result")]
    public void AddsAnErrorToAnErroredResult()
    {
        var result = Result.Fail()
            .AddError("An error");

        result
            .Should()
            .BeEquivalentTo(Result.Fail("An error"));
    }

    [Fact(DisplayName = "Adds multiple errors to an errored result")]
    public void AddsMultipleErrorsToAnErroredResult()
    {
        var result = Result.Fail()
            .AddErrors(
                new[]
                {
                    "An error",
                    "Another error",
                });

        result
            .Should()
            .BeEquivalentTo(Result.Fail(new("An error"), new("Another error")));
    }

    [Fact(DisplayName = "Gets the errors of a result")]
    public void GetsTheErrorsOfAResult()
    {
        var result = Result.Fail(new("An error"), new("Another error"));

        result.Errors.Should()
            .BeEquivalentTo(
                new[]
                {
                    new Error("An error"),
                    new("Another error"),
                });
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

    [Fact(DisplayName = "Matches an errored result and run an action")]
    public void MatchesAnErroredResultAndRunAnAction()
    {
        var result = Result.Fail("A strange error");

        IEnumerable<Error>? errors = null;
        result.Match(
            _ => { errors = null; },
            err => { errors = err; });

        errors.Should()
            .BeEquivalentTo(
                new[]
                {
                    new Error("A strange error"),
                });
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

    [Fact(DisplayName = "Matches an errored result, runs an action and returns the value")]
    public void MatchesAnErroredResultAndRunAnActionAndReturnsTheValue()
    {
        var result = Result.Fail("A strange error");

        var value = result.Match(
            static _ => Enumerable.Empty<Error>(),
            static errors => errors);

        value.Should()
            .BeEquivalentTo(
                new[]
                {
                    new Error("A strange error"),
                });
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