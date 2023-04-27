using System.Collections.Immutable;
using FsCheck;
using FsCheck.Xunit;
using Results.Immutable.Tests.Extensions;
using static Results.Immutable.Tests.Generators.IntegerResultGenerator;

namespace Results.Immutable.Tests;

public sealed class ResultFactoriesTests
{

    public class Ok
    {
        [Fact(DisplayName = "Creates a successful result of unit")]
        public void ShouldCreateSuccessfulResult()
        {
            var r = Result.Ok();
            r.IsOk.Should().BeTrue();
            r.Some?.Value.Should().Be(Unit.Value);
        }

        [Fact(DisplayName = "Creates a successful result of provided type")]
        public void ShouldCreateSuccessfulResultOfProvidedType()
        {
            var r = Result.Ok(42);
            r.IsOk.Should().BeTrue();
            r.Some?.Value.Should().Be(42);
        }
    }

    public class Fail
    {
        [Fact(DisplayName = "Creates a failed result with one error from error message")]
        public void CreatesAFailedResultWithOneErrorFromErrorMessage()
        {
            var r = Result.Fail("space shuttle blowed");
            r.IsErrored.Should().BeTrue();
            r.Errors.Single().Message.Should().Be("space shuttle blowed");
        }

        [Fact(DisplayName = "Creates a failed result with one error from error record")]
        public void CreatesAFailedResultWithOneErrorFromErrorRecord()

        {
            var error = new Error("space shuttle blowed");

            var r = Result.Fail(error);
            r.IsErrored.Should().BeTrue();
            r.Errors.Single().Equals(error);
        }

        [Fact(DisplayName = "Creates a failed result with a collection of errors")]
        public void CreatesAFailedResultWithACollectionOfErrors()
        {
            var errors = ImmutableList.Create(new Error("space shuttle blowed"), new Error("rocket exploded"));

            var r = Result.Fail(errors);
            r.IsErrored.Should().BeTrue();
            r.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact(DisplayName = "Creates a failed result with a collection of errors as params")]
        public void CreatesAFailedResultWithACollectionOfErrorsAsParams()
        {
            var r = Result.Fail(new Error("space shuttle blowed"), new Error("rocket exploded"));
            r.IsErrored.Should().BeTrue();
            r.Errors.Should().BeEquivalentTo(ImmutableList.Create(new Error("space shuttle blowed"), new Error("rocket exploded")));
        }

        [Fact(DisplayName = "Creates a failed result of a provided type with one error")]
        public void CreatesFailedResultOfProvidedTypeFromErrorMessage()
        {
            var r = Result.Fail<int>("space shuttle blowed");
            r.IsErrored.Should().BeTrue();
            r.Errors.Single().Message.Should().Be("space shuttle blowed");
        }

        [Fact(DisplayName = "Creates a failed result of a provided type with one error from error record")]
        public void CreatesAFailedResultOfProvidedTypeWithOneErrorFromErrorRecord()
        {
            var error = new Error("space shuttle blowed");

            var r = Result.Fail<int>(error);
            r.IsErrored.Should().BeTrue();
            r.Errors.Single().Equals(error);
        }

        [Fact(DisplayName = "Creates a failed result of a provided type with a collection of errors")]
        public void CreatesAFailedResultOfProvidedTypeWithACollectionOfErrors()
        {
            var errors = ImmutableList.Create(new Error("space shuttle blowed"), new Error("rocket exploded"));

            var r = Result.Fail<int>(errors);
            r.IsErrored.Should().BeTrue();
            r.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact(DisplayName = "Creates a failed result of a provided type with a collection of errors as params")]
        public void CreatesAFailedResultOfProvidedTypeWithACollectionOfErrorsAsParams()
        {
            var r = Result.Fail<int>(new Error("space shuttle blowed"), new Error("rocket exploded"));
            r.IsErrored.Should().BeTrue();
            r.Errors.Should().BeEquivalentTo(ImmutableList.Create(new Error("space shuttle blowed"), new Error("rocket exploded")));
        }
    }

    // private const string ErrorMessage = "An error message";

    // [Fact(DisplayName = "Should create successful result with no reasons")]
    // public void ShouldCreateSuccessfulResultWithoutReasons() =>
    //     Result.Ok()
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && !r.Reasons.Any() && ValueIsAUnit(r));

    // [Fact(DisplayName = "Should create successful result of provided type with no reasons and no value")]
    // public void ShouldCreateSuccessfulResultOfProvidedTypeWithoutReasons() =>
    //     Result.Ok<int>()
    //         .Should()
    //         .Match<Result<int>>(static r => r.IsOk && !r.Reasons.Any() && r.Option.IsNone);

    // [Fact(DisplayName = "Should create a failed result with one error from error message")]
    // public void ShouldCreateFailedResultFromErrorMessage() =>
    //     Result.Fail(ErrorMessage)
    //         .Should()
    //         .Match<Result<Unit>>(
    //             static r => r.IsErrored &&
    //                 r.Errors.ToArray()
    //                     .Single()
    //                     .Message ==
    //                 ErrorMessage);

    // [Fact(DisplayName = "Should create a failed result with one error from error record")]
    // public void ShouldCreateFailedResultFromAnErrorRecord()
    // {
    //     var error = new Error(ErrorMessage);

    //     Result.Fail(error)
    //         .Should()
    //         .Match<Result<Unit>>(
    //             r => r.IsErrored &&
    //                 r.Errors.Single()
    //                     .Equals(error));
    // }

    // [Fact(DisplayName = "Should create a failed result with a collection of errors")]
    // public void ShouldCreateAFailedResultWithACollectionOfErrors()
    // {
    //     var errors = Enumerable.Range(1, 5)
    //         .Select(static i => new Error($"Error number {i}"))
    //         .ToList();

    //     Result.Fail(errors)
    //         .Should()
    //         .Match<Result<Unit>>(r => r.IsErrored && r.Errors.SequenceEqual(errors));
    // }

    // [Fact(DisplayName = "Should create a failed generic result with a collection of errors")]
    // public void ShouldCreateAFailedGenericResultWithACollectionOfErrors()
    // {
    //     var errors = Enumerable.Range(1, 5)
    //         .Select(static i => new Error($"Error number {i}"))
    //         .ToList();

    //     Result.Fail<int>(errors)
    //         .Should()
    //         .Match<Result<int>>(r => r.IsErrored && r.Errors.SequenceEqual(errors));
    // }

    // [Fact(DisplayName = "HasError should return true for a failed result with generic error")]
    // public void ShouldReturnTrueForFailedResult()
    // {
    //     var error = new Error("A failure");

    //     Result.Fail(error)
    //         .HasError<Error>()
    //         .Should()
    //         .BeTrue();
    // }

    // [Fact(DisplayName = "HasError should return true for a defined, nested error")]
    // public void ShouldReturnTrueForANestedError()
    // {
    //     var complexError = new Error(
    //         "Some complex failure",
    //         ImmutableList.Create<Error>(new RootError("Root cause of a failure")),
    //         ImmutableDictionary<string, object>.Empty);

    //     Result.Fail(complexError)
    //         .HasError<RootError>()
    //         .Should()
    //         .BeTrue();
    // }

    // [Fact(DisplayName = "HasSuccess should return true for a defined, nested success")]
    // public void ShouldReturnTrueForANestedSuccess()
    // {
    //     var complexSuccess = new Success(
    //         "Some complex success",
    //         ImmutableList.Create<Success>(new RootSuccess("Root cause")),
    //         ImmutableDictionary<string, object>.Empty);

    //     Result.Ok(
    //             Unit.Value,
    //             new[]
    //             {
    //                 complexSuccess,
    //             })
    //         .HasSuccess<RootSuccess>()
    //         .Should()
    //         .BeTrue();
    // }

    // [Fact(DisplayName = "Should create successful result with a provided value without any reasons")]
    // public void ShouldCreateSuccessfulResultWithProvidedValue() =>
    //     Result.Ok(0)
    //         .Should()
    //         .Match<Result<int>>(static r => r.IsOk && !r.Reasons.Any() && r.ValueMatches(static i => i == 0));

    // [Property(
    //     DisplayName = "Merger of a list of successful results should result in a success",
    //     MaxTest = 1000)]
    // public Property MergingAListOfSuccessfulResultsShouldReturnSuccessfulResult() =>
    //     Prop.ForAll(
    //         Gen.ListOf(GetSuccessfulIntegerResultWithValueGenerator())
    //             .ToArbitrary(),
    //         static list =>
    //             list.Merge() is { IsOk: true, Option.ValueOrDefault: { } enumerable, } &&
    //             enumerable.SequenceEqual(list.Select(static r => r.Option.ValueOrDefault)));

    // [Property(
    //     DisplayName = "Merger of a list of results is a failure if any of them has failed",
    //     MaxTest = 1000)]
    // public Property MergingAListOrResultsIsAFailureIfAnyOfThemHasFailed() =>
    //     Prop.ForAll(
    //         Gen.ListOf(GetIntegerResultGenerator())
    //             .Where(static list => list.Any(static r => r.IsErrored))
    //             .ToArbitrary(),
    //         static list => list.Merge()
    //             .IsErrored);

    // [Fact(DisplayName = "Merger of an empty list of results is successful")]
    // public void MergingOfAnEmptyListOfResultsIsSuccessful() =>
    //     Enumerable.Empty<Result<Unit>>()
    //         .Merge()
    //         .Should()
    //         .Match<Result<IEnumerable<Unit>>>(
    //             static r => r.IsOk && r.ValueMatches(static e => e != null && !e.Any()));

    // [Property(
    //     DisplayName = "Merger of successful Result params is successful",
    //     MaxTest = 1000)]
    // public Property MergingOfSuccessfulResultsIsSuccessful() =>
    //     Prop.ForAll(
    //         Gen.Two(GetSuccessfulIntegerResultWithValueGenerator())
    //             .ToArbitrary(),
    //         static tuple =>
    //         {
    //             var (first, second) = tuple;

    //             return Result.Merge(first, second)
    //                 .IsSuccessful.ToProperty();
    //         });

    // [Property(
    //     DisplayName = "Merger of Result params is a failure if any of them is a failure",
    //     MaxTest = 1000)]
    // public Property MergingOfResultsIsAFailureIfAnyOfThemIsAFailure() =>
    //     Prop.ForAll(
    //         Gen.Two(GetIntegerResultGenerator())
    //             .Where(
    //                 static tuple =>
    //                 {
    //                     var (first, second) = tuple;

    //                     return first.IsErrored || second.IsErrored;
    //                 })
    //             .ToArbitrary(),
    //         static tuple =>
    //         {
    //             var (first, second) = tuple;

    //             return Result.Merge(first, second)
    //                 .IsAFailure.ToProperty();
    //         });

    // [Fact(DisplayName = "OkIf returns successful result if the condition is true")]
    // public void OkIfReturnsSuccessfulResultIfTheConditionIsTrue() =>
    //     Result.OkIf(true, string.Empty)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && ValueIsAUnit(r));

    // [Fact(DisplayName = "OkIf returns a failed result with a matching error if the condition is false")]
    // public void OkIfReturnsSuccessfulResultIfTheConditionIsFalse()
    // {
    //     const string errorMessage = "An error";

    //     Result.OkIf(false, errorMessage)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsErrored && r.HasError<Error>(static e => e.Message == errorMessage));
    // }

    // [Fact(DisplayName = "OkIf returns a failed result with a matching, typed error if the condition is false")]
    // public void OkIfReturnsAFailedResultWithAMatchingTypedErrorIfTheConditionIsFalse()
    // {
    //     const string errorMessage = "An error";

    //     Result.OkIf(
    //             false,
    //             new RootError(errorMessage))
    //         .Should()
    //         .Match<Result<Unit>>(
    //             static r => r.IsErrored && r.HasError<RootError>(static e => e.Message == errorMessage));
    // }

    // [Fact(DisplayName = "OkIf returns successful result without calling error factory if the condition is true")]
    // public void OkIfReturnsSuccessfulResultWithoutCallingErrorMessageFactoryIfTheConditionIsTrue()
    // {
    //     Result.OkIf(
    //             true,
    //             GetErrorMessage)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && ValueIsAUnit(r));

    //     static string GetErrorMessage() =>
    //         throw new InvalidOperationException("Lazy OkIf overload instantiated an error for successful result!");
    // }

    // [Fact(
    //     DisplayName = "OkIf returns a failed result with a matching, lazily evaluated error if the condition is false")]
    // public void OkIfReturnsAFailureWithAMatchingLazilyEvaluatedErrorIfTheConditionIsFalse()
    // {
    //     const string errorMessage = "An error";

    //     Result.OkIf(
    //             false,
    //             static () => errorMessage)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsErrored && r.HasError<Error>(static e => e.Message == errorMessage));
    // }

    // [Fact(
    //     DisplayName =
    //         "OkIf returns a failed result with a matching, lazily evaluated and typed error if the condition is false")]
    // public void OkIfReturnsAFailedResultWithAMatchingLazilyEvaluatedAndTypedErrorIfTheConditionIsFalse()
    // {
    //     const string errorMessage = "An error";

    //     Result.OkIf(
    //             false,
    //             static () => new RootError(errorMessage))
    //         .Should()
    //         .Match<Result<Unit>>(
    //             static r => r.IsErrored && r.HasError<RootError>(static e => e.Message == errorMessage));
    // }

    // [Fact(DisplayName = "FailIf returns successful result if the condition is false")]
    // public void FailIfReturnsSuccessfulResultIfTheConditionIsFalse() =>
    //     Result.FailIf(false, string.Empty)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && ValueIsAUnit(r));

    // [Fact(DisplayName = "FailIf returns a failed result with a matching error if the condition is true")]
    // public void FailIfReturnsSuccessfulResultIfTheConditionIsTrue()
    // {
    //     const string errorMessage = "An error";

    //     Result.FailIf(true, errorMessage)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsErrored && r.HasError<Error>(static e => e.Message == errorMessage));
    // }

    // [Fact(DisplayName = "FailIf returns a failed result with a matching, typed error if the condition is true")]
    // public void FailIfReturnsAFailedResultWithAMatchingTypedErrorIfTheConditionIsTrue()
    // {
    //     const string errorMessage = "An error";

    //     Result.FailIf(
    //             true,
    //             new RootError(errorMessage))
    //         .Should()
    //         .Match<Result<Unit>>(
    //             static r => r.IsErrored && r.HasError<RootError>(static e => e.Message == errorMessage));
    // }

    // [Fact(DisplayName = "FailIf returns successful result without calling error factory if the condition is false")]
    // public void FailIfReturnsSuccessfulResultWithoutCallingErrorMessageFactoryIfTheConditionIsFalse()
    // {
    //     Result.FailIf(
    //             false,
    //             GetErrorMessage)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && ValueIsAUnit(r));

    //     static string GetErrorMessage() =>
    //         throw new InvalidOperationException("Lazy OkIf overload instantiated an error for successful result!");
    // }

    // [Fact(
    //     DisplayName =
    //         "FailIf returns a failed result with a matching, lazily evaluated error if the condition is true")]
    // public void FailIfReturnsAFailureWithAMatchingLazilyEvaluatedErrorIfTheConditionIsTrue()
    // {
    //     const string errorMessage = "An error";

    //     Result.FailIf(
    //             true,
    //             static () => errorMessage)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsErrored && r.HasError<Error>(static e => e.Message == errorMessage));
    // }

    // [Fact(
    //     DisplayName =
    //         "FailIf returns a failed result with a matching, lazily evaluated and typed error if the condition is true")]
    // public void FailIfReturnsAFailedResultWithAMatchingLazilyEvaluatedAndTypedErrorIfTheConditionIsTrue()
    // {
    //     const string errorMessage = "An error";

    //     Result.FailIf(
    //             true,
    //             static () => new RootError(errorMessage))
    //         .Should()
    //         .Match<Result<Unit>>(
    //             static r => r.IsErrored && r.HasError<RootError>(static e => e.Message == errorMessage));
    // }

    // [Fact(DisplayName = "Try returns successful result if the action succeeds")]
    // public void TryReturnsSuccessfulResultIfTheActionSucceeds()
    // {
    //     Result.Try(GetUnit)
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && ValueIsAUnit(r));

    //     static Unit GetUnit() => Unit.Value;
    // }

    // [Fact(
    //     DisplayName =
    //         "Try returns a failed result with an ExceptionalError with proper exception referenced by CausedBy property if exception is thrown")]
    // public void TryReturnsAFailedResultWithAnExceptionalErrorWithProperExceptionIfAnExceptionIsThrown()
    // {
    //     var exceptionToThrow = new InvalidOperationException("Oops!");

    //     Result.Try(ThrowException)
    //         .Should()
    //         .Match<Result<Unit>>(
    //             r => r.IsErrored && r.HasError<ExceptionalError>(ee => ee.CausedBy.Equals(exceptionToThrow)));

    //     Unit ThrowException() => throw exceptionToThrow;
    // }

    // [Fact(DisplayName = "Try returns successful result if the Task succeeds")]
    // public async Task TryReturnsSuccessfulResultIfTheTaskSucceeds()
    // {
    //     (await Result.Try(GetUnit))
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && ValueIsAUnit(r));

    //     static Task<Unit> GetUnit() => Task.FromResult(Unit.Value);
    // }

    // [Fact(
    //     DisplayName =
    //         "Try returns a failed result with an ExceptionalError with" +
    //         "proper exception referenced by CausedBy property if exception is thrown " +
    //         "by a Task")]
    // public async Task TryReturnsAFailedResultWithAnExceptionalErrorWithProperExceptionIfAnExceptionIsThrownByATask()
    // {
    //     var exceptionToThrow = new InvalidOperationException("Oops!");

    //     (await Result.Try(ThrowException))
    //         .Should()
    //         .Match<Result<Unit>>(
    //             r => r.IsErrored && r.HasError<ExceptionalError>(ee => ee.CausedBy.Equals(exceptionToThrow)));

    //     Task<Unit> ThrowException() => throw exceptionToThrow;
    // }

    // [Fact(DisplayName = "Try returns successful result if the ValueTask succeeds")]
    // public async Task TryReturnsSuccessfulResultIfTheValueTaskSucceeds()
    // {
    //     (await Result.Try(GetUnit))
    //         .Should()
    //         .Match<Result<Unit>>(static r => r.IsOk && ValueIsAUnit(r));

    //     static ValueTask<Unit> GetUnit() => new(Unit.Value);
    // }

    // [Fact(
    //     DisplayName =
    //         "Try returns a failed result with an ExceptionalError with" +
    //         "proper exception referenced by CausedBy property if exception is thrown " +
    //         "by a ValueTask")]
    // public async Task
    //     TryReturnsAFailedResultWithAnExceptionalErrorWithProperExceptionIfAnExceptionIsThrownByAValueTask()
    // {
    //     var exceptionToThrow = new InvalidOperationException("Oops!");

    //     (await Result.Try(ThrowException))
    //         .Should()
    //         .Match<Result<Unit>>(
    //             r => r.IsErrored && r.HasError<ExceptionalError>(ee => ee.CausedBy.Equals(exceptionToThrow)));

    //     ValueTask<Unit> ThrowException() => throw exceptionToThrow;
    // }

    // private static bool ValueIsAUnit(Result<Unit> result) => result.ValueMatches(static u => u == Unit.Value);

    // private sealed record RootError(
    //         string Message,
    //         ImmutableList<Error> Errors,
    //         ImmutableDictionary<string, object> Metadata)
    //     : Error(
    //         Message,
    //         Errors,
    //         Metadata)
    // {
    //     public RootError(string message)
    //         : this(
    //             message,
    //             ImmutableList<Error>.Empty,
    //             ImmutableDictionary<string, object>.Empty)
    //     {
    //     }
    // }

    // private sealed record RootSuccess(
    //         string Message,
    //         ImmutableList<Success> Successes,
    //         ImmutableDictionary<string, object> Metadata)
    //     : Success(
    //         Message,
    //         Successes,
    //         Metadata)
    // {
    //     public RootSuccess(string message)
    //         : this(
    //             message,
    //             ImmutableList<Success>.Empty,
    //             ImmutableDictionary<string, object>.Empty)
    //     {
    //     }
    // }
}