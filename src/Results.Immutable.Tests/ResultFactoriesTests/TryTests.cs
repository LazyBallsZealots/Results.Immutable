using static Results.Immutable.Tests.ResultFactoriesTests.ResultMatching;

namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class TryTests
{
    [Fact(DisplayName = "Try returns successful result if the action succeeds")]
    public void TryReturnsSuccessfulResultIfTheActionSucceeds()
    {
        Result.Try(GetUnit)
            .Should()
            .Match<Result<Unit>>(static r => r.HasSucceeded && ValueIsAUnit(r));

        static Unit GetUnit() => Unit.Value;
    }

    [Fact(
        DisplayName =
            "Try returns a failed result with an ExceptionalError with proper exception referenced in CausedBy property if exception is thrown")]
    public void TryReturnsAFailedResultWithAnExceptionalErrorWithProperExceptionIfAnExceptionIsThrown()
    {
        var exceptionToThrow = new InvalidOperationException("Oops!");

        Result.Try(ThrowException)
            .Should()
            .Match<Result<Unit>>(
                r => r.HasFailed && r.HasError<ExceptionalError>(ee => ee.CausedBy.Equals(exceptionToThrow)));

        Unit ThrowException() => throw exceptionToThrow;
    }

    [Fact(DisplayName = "TryAsync returns successful result of Unit if the task succeeds")]
    public async Task TryAsyncReturnsASuccessfulResultOfUnitIfTheTaskSucceeds()
    {
        (await Result.TryAsync(RunTask))
            .Should()
            .Match<Result<Unit>>(static r => r.HasSucceeded && ValueIsAUnit(r));

        static ValueTask RunTask() => new();
    }

    [Fact(DisplayName = "TryAsync returns a failed result with an exceptional error if the task throws")]
    public async Task TryAsyncReturnsAFailedResultWithAnExceptionalErrorIfTheTaskThrows()
    {
        var exceptionToThrow = new InvalidOperationException("Asynchronous whoopsie!");

        (await Result.TryAsync(Throw))
            .Should()
            .Match<Result<Unit>>(
                r => r.HasFailed && r.HasError<ExceptionalError>(ee => ee.CausedBy.Equals(exceptionToThrow)));

        ValueTask Throw() => throw exceptionToThrow;
    }

    [Fact(DisplayName = "TryAsync returns successful generic result if the task succeeds")]
    public async Task TryAsyncReturnsASuccessfulResultIfTheTaskSucceeds()
    {
        (await Result.TryAsync(RunTask))
            .Should()
            .Match<Result<Unit>>(static r => r.HasSucceeded && ValueIsAUnit(r));

        static ValueTask<Unit> RunTask() => new(Unit.Value);
    }

    [Fact(DisplayName = "TryAsync returns a failed result with an exceptional error if a generic task throws")]
    public async Task TryAsyncReturnsAFailedResultWithAnExceptionalErrorIfTheGenericTaskThrows()
    {
        var exceptionToThrow = new InvalidOperationException("Generic async whoopsie!");

        (await Result.TryAsync(ThrowAsync))
            .Should()
            .Match<Result<Unit>>(r => r.HasError<ExceptionalError>(ee => ee.CausedBy.Equals(exceptionToThrow)));

        ValueTask<Unit> ThrowAsync() => throw exceptionToThrow;
    }
}