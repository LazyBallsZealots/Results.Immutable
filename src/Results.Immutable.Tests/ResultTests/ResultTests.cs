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

    // [Fact(DisplayName = "Should create a new result with provided reason")]
    // public void ShouldCreateANewResultWithProvidedReason()
    // {
    //     var reason = new Reason("A new and totally justifiable reason");

    //     SuccessfulResult.WithReason(reason)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.Match<Result<Unit>>(r => r.IsSuccessful && r.Reasons.Single() == reason);
    // }

    // [Fact(DisplayName = "Should create a new result with provided reasons")]
    // public void ShouldCreateANewResultWithProvidedReasons()
    // {
    //     const string message = "A new reason";

    //     var reasons = Enumerable.Range(1, 5)
    //         .Select(static i => new Reason($"{message} number {i}"))
    //         .ToList();

    //     SuccessfulResult.WithReasons(reasons)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Reasons.Should()
    //         .BeEquivalentTo(reasons.ToImmutableList());
    // }

    // [Fact(DisplayName = "Should create a new result with an error based on provided message")]
    // public void ShouldCreateANewResultWithAnErrorBasedOnProvidedMessage()
    // {
    //     const string message = "An error";

    //     SuccessfulResult.WithError(message)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.HasError(static (Error e) => e.Message == message)
    //         .Should()
    //         .BeTrue();
    // }

    // [Fact(DisplayName = "Should create a new result with errors based on provided messages")]
    // public void ShouldCreateANewResultWithErrorsBasedOnProvidedMessages()
    // {
    //     var errors = Enumerable.Range(1, 5)
    //         .Select(static i => $"Error number {i}")
    //         .ToList();

    //     SuccessfulResult.WithErrors(errors)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Errors.Select(static e => e.Message)
    //         .Should()
    //         .BeEquivalentTo(errors);
    // }

    // [Fact(DisplayName = "Should create a new result with provided error")]
    // public void ShouldCreateANewResultWithProvidedError()
    // {
    //     var initialResult = Result.Ok();

    //     var error = new Error("An error");

    //     initialResult.WithError(error)
    //         .Should()
    //         .NotBe(initialResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.HasError((Error e) => e.Equals(error))
    //         .Should()
    //         .BeTrue();
    // }

    // [Fact(DisplayName = "Should create a new failed result with provided errors")]
    // public void ShouldCreateANewResultWithProvidedErrors()
    // {
    //     var errors = Enumerable.Range(1, 5)
    //         .Select(static i => new Error($"Error number {i}"))
    //         .ToList();

    //     SuccessfulResult.WithErrors(errors)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Errors.Should()
    //         .BeEquivalentTo(errors.ToImmutableList());
    // }

    // [Fact(DisplayName = "Should create a new successful result with provided success")]
    // public void ShouldCreateASuccessfulResultWithProvidedSuccess()
    // {
    //     var success = new Success("A success");

    //     SuccessfulResult.WithSuccess(success)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.Match<Result<Unit>>(static r => r.IsSuccessful)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.HasSuccess((Success s) => s.Equals(success))
    //         .Should()
    //         .BeTrue();
    // }

    // [Fact(DisplayName = "Should create a new successful result with provided successes")]
    // public void ShouldCreateANewSuccessfulResultWithProvidedSuccesses()
    // {
    //     var successes = Enumerable.Range(1, 10)
    //         .Select(static i => new Success($"A success number {i}"))
    //         .ToList();

    //     SuccessfulResult.WithSuccesses(successes)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.Match<Result<Unit>>(static r => r.IsSuccessful)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Successes.Should()
    //         .BeEquivalentTo(successes.ToImmutableList());
    // }

    // [Fact(DisplayName = "Should create a new result containing a generic error with provided error message")]
    // public void ShouldCreateANewResultWithAGenericErrorWithProvidedMessage()
    // {
    //     const string message = "An error";

    //     SuccessfulResult.WithError(message)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Errors.Single()
    //         .Should()
    //         .Match<Error>(static e => e.Message == message);
    // }

    // [Theory(DisplayName = "Should create a new result containing provided error and its underlying causes")]
    // [MemberData(nameof(Errors))]
    // public void ShouldCreateANewResultContainingProvidedErrorAndItsCauses(Error error)
    // {
    //     SuccessfulResult.WithError(error)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Errors.SelectMany(Flatten)
    //         .Should()
    //         .Contain(Flatten(error));

    //     static IEnumerable<Error> Flatten(Error e) =>
    //         new[]
    //         {
    //             e,
    //         }.Concat(e.Errors.SelectMany(Flatten));
    // }

    // [Fact(DisplayName = "Getting value of a failed result should return None")]
    // public void FetchingValueFromAFailedResultShouldReturnNone() =>
    //     Result.Fail("An error")
    //         .Option
    //         .Should()
    //         .Be(Option.None<Unit>());

    // [Fact(DisplayName = "Should create a new result with generic errors containing provided messages")]
    // public void ShouldCreateANewResultWithGenericErrorsContainingProvidedMessages()
    // {
    //     var errorMessages = Enumerable.Repeat("Error message no", 10)
    //         .Select(static (messageTemplate, index) => $"{messageTemplate} {index + 1}")
    //         .ToList();
    //
    //     SuccessfulResult.WithErrors(errorMessages)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Errors
    //         .Select(static e => e.Message)
    //         .Should()
    //         .BeEquivalentTo(errorMessages);
    // }
}