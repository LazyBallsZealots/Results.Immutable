namespace Results.Immutable.Extensions.FluentAssertions.Tests;

public class ResultTest
{
    [Fact(DisplayName = "Be ok with ok")]
    public void BeOkWithOk() =>
        Result.Ok(5)
            .Should()
            .BeOk()
            .Which.Should()
            .Be(5);

    [Fact(DisplayName = "Be ok with error")]
    public void BeOkWithError()
    {
        var act = () => Result.Fail<int>("anything")
            .Should()
            .BeOk("no");

        act.Should()
            .Throw<Exception>()
            .WithMessage("Expected result to be ok because no, but found error.");
    }

    [Fact(DisplayName = "Be failed with ok")]
    public void BeFailedWithOk()
    {
        var act = () => Result.Ok(5)
            .Should()
            .BeFailed("no");

        act.Should()
            .Throw<Exception>()
            .WithMessage("Expected result to be failed because no, but found ok.");
    }

    [Fact(DisplayName = "Be failed with error")]
    public void BeFailedWithError() =>
        Result.Fail<int>("anything")
            .Should()
            .BeFailed();

    [Fact(DisplayName = "Contain error of type with ok")]
    public void ContainErrorOfTypeWithOk()
    {
        var act = () => Result.Ok(5)
            .Should()
            .ContainErrorsOfType<MyError>("no");

        act.Should()
            .Throw<Exception>()
            .WithMessage($"Expected result to contain errors of type {typeof(MyError)} because no, but found none.");
    }

    [Fact(DisplayName = "Contain error of type with another type of error")]
    public void ContainErrorOfTypeWithAnotherTypeOfError()
    {
        var act = () => Result.Fail<int>("anything")
            .Should()
            .ContainErrorsOfType<MyError>("no");

        act.Should()
            .Throw<Exception>()
            .WithMessage($"Expected result to contain errors of type {typeof(MyError)} because no, but found none.");
    }

    [Fact(DisplayName = "Contain error of type with error")]
    public void ContainErrorOfTypeWithError() =>
        Result.Fail<int>(new MyError("important"))
            .Should()
            .ContainErrorsOfType<MyError>()
            .Which
            .Should()
            .ContainSingle(static x => x.Message == "important");

    private record MyError(string Message) : Error(Message);
}