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
    }

    [Fact(
        DisplayName =
            "Try returns a failed result with an ExceptionalError with proper exception referencediny CausedBy property if exception is thrown")]
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

        static Task RunTask() => Task.CompletedTask;
    }

    [Fact(DisplayName = "TryAsync returns a failed result with an exceptional error if the task throws")]
    public async Task TryAsyncReturnsAFailedResultWithAnExceptionalErrorIfTheTaskThrows()
    {
        var exceptionToThrow = new InvalidOperationException("Asynchronous whoopsie!");

        (await Result.TryAsync(Throw))
            .Should()
            .Match<Result<Unit>>(
                r => r.HasFailed && r.HasError<ExceptionalError>(ee => ee.CausedBy.Equals(exceptionToThrow)));

        Task Throw() => throw exceptionToThrow;
    }

    [Fact(DisplayName = "TryAsync returns successful generic result if the task succeeds")]
    public async Task TryAsyncReturnsASuccessfulResultIfTheTaskSucceeds()
    {
        (await Result.TryAsync(RunTask))
            .Should()
            .Match<Result<Unit>>(static r => r.HasSucceeded && ValueIsAUnit(r));

        static Task<Unit> RunTask() => Task.FromResult(Unit.Value);
    }

    [Fact(DisplayName = "TryAsync returns a failed result with an exceptional error if a generic task throws")]
    public async Task TryAsyncReturnsAFailedResultWithAnExceptionalErrorIfTheGenericTaskThrows()
    {
        var exception = new InvalidOperationException("Async whoops!");

        (await Result.TryAsync(Throw))
            .Should()
            .Match<Result<Unit>>(r => r.HasFailed && r.HasError<ExceptionalError>(ee => ee.CausedBy == exception));

        Task<Unit> Throw() => throw exception;
    }

    private static Unit GetUnit() => Unit.Value;
}