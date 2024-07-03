namespace Results.Immutable.Tests;

public class ErrorTests
{
    [Fact(DisplayName = "Adds additional errors to an error")]
    public void AddAdditionalErrorsToAnError()
    {
        var error = new PiggyBankError("full of pennies", ImmutableList<Error>.Empty);
        Error rootCause = new("too many pennies");
        var newError = error
            .WithRootCauses(ImmutableList.Create(rootCause));

        newError.Should()
            .NotBe(error);

        newError.Should()
            .BeOfType<PiggyBankError>();

        newError.Message.Should()
            .Be("full of pennies");

        newError.InnerErrors.Should()
            .ContainSingle()
            .Which.Should()
            .Be(rootCause);
    }

    [Fact(DisplayName = "Adds an additional error to an error")]
    public void AddAnAdditionalErrorToAnError()
    {
        var error = new PiggyBankError("full of pennies", ImmutableList<Error>.Empty);
        Error rootCause = new("too many pennies");
        var newError = error
            .WithRootCause(rootCause);

        newError.Should()
            .NotBe(error);

        newError.Should()
            .BeOfType<PiggyBankError>();

        newError.Message.Should()
            .Be("full of pennies");

        newError.InnerErrors.Should()
            .ContainSingle()
            .Which.Should()
            .Be(rootCause);
    }

    private record PiggyBankError(
        string Message,
        ImmutableList<Error> InnerErrors) : Error<PiggyBankError>(Message, InnerErrors);
}