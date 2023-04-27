using System.Collections.Immutable;

namespace Results.Immutable.Tests;

public class ResultTests
{
    public static readonly TheoryData<Error> Errors = new()
    {
        new("Sample flat error"),
        new(
            "Complex error",
            ImmutableList.Create(new Error("Cause of the original error"))),
    };

    public class Equality
    {
        [Fact(DisplayName = "Should be equal to itself")]
        public void ShouldBeEqualToItself()
        {
            var result = Result.Ok();

            result.Should().Be(result);
        }

        [Fact(DisplayName = "Should be equal to a result with the same value")]
        public void ShouldBeEqualToAResultWithTheSameValue()
        {
            var intResult = Result.Ok(42);
            intResult.Should().Be(Result.Ok(42));
        }

        [Fact(DisplayName = "Should be equal to a result with the same value and same successes")]
        public void ShouldBeEqualToAResultWithTheSameValueAndSameSuccesses()
        {
            var result = Result.Ok()
                .AddSuccess(new Success("A success"));
            result.Should().Be(Result.Ok()
                .AddSuccess(new Success("A success")));
        }

        [Fact(DisplayName = "Should not be equal to a result with the same value but different successes")]
        public void ShouldNotBeEqualToAResultWithTheSameValueButDifferentSuccesses()
        {
            var result = Result.Ok()
                .AddSuccess(new Success("A success"));
            result.Should().NotBe(Result.Ok()
                .AddSuccess(new Success("Another success")));
        }

        [Fact(DisplayName = "Should not be equal to a result with a different value")]
        public void ShouldNotBeEqualToAResultWithADifferentValue()
        {
            var result = Result.Ok(15);
            result.Should().NotBe(Result.Ok(42));
        }

        [Fact(DisplayName = "Should not be equal to a result with a different value and same successes")]
        public void ShouldNotBeEqualToAResultWithADifferentValueAndSameSuccesses()
        {
            var result = Result.Ok(15)
                .AddSuccess(new Success("A success"));
            result.Should().NotBe(Result.Ok(10)
                .AddSuccess(new Success("A success")));
        }

        [Fact(DisplayName = "Should not be equal to a result with different errors")]
        public void ShouldNotBeEqualToAResultWithDifferentErrors()
        {
            var result = Result.Fail<int>("life is sad");
            result.Should().NotBe(Result.Fail<int>("death is sad"));
        }

        [Fact(DisplayName = "Should not be equal to a result with different errors and same successes")]
        public void ShouldNotBeEqualToAResultWithDifferentErrorsAndSameSuccesses()
        {
            var result = Result.Fail<int>("life is sad")
                .AddSuccess(new Success("A success"));
            result.Should().NotBe(Result.Fail<int>("death is sad")
                .AddSuccess(new Success("A success")));
        }

        [Fact(DisplayName = "Should not be equal to a result with the same errors but different successes")]
        public void ShouldNotBeEqualToAResultWithTheSameErrorsButDifferentSuccesses()
        {
            var result = Result.Fail<int>("life is sad")
                .AddSuccess(new Success("A success"));
            result.Should().NotBe(Result.Fail<int>("life is sad")
                .AddSuccess(new Success("Another success")));
        }
    }

    // private static readonly Result<Unit> SuccessfulResult = Result.Ok();

    // [Fact(DisplayName = "Creating new result via ctor returns successful result without a value")]
    // public void ShouldCreateSuccessfulResultWithoutAValue() =>
    //     new Result<Unit>()
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && r.Option.IsNone);

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

    //     SuccessfulResult.WithErrors(errorMessages)
    //         .Should()
    //         .NotBe(SuccessfulResult)
    //         .And.BeOfType<Result<Unit>>()
    //         .Which.Errors
    //         .Select(static e => e.Message)
    //         .Should()
    //         .BeEquivalentTo(errorMessages);
    // }

    // [Fact(DisplayName = "HasError should return false for successful result")]
    // public void HasErrorShouldReturnFalseForSuccessfulResult() =>
    //     SuccessfulResult.HasError<Error>()
    //         .Should()
    //         .BeFalse();

    // [Fact(DisplayName = "HasSuccess should return false for a new, failed result")]
    // public void HasSuccessShouldReturnFalseForANewFailedResult() =>
    //     Result.Fail("An error")
    //         .HasSuccess<Success>()
    //         .Should()
    //         .BeFalse();
}