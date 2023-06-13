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
}