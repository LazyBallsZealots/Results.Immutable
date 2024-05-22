namespace Results.Immutable.Extensions.FluentAssertions.Tests;

public class ResultTest
{
    [Fact(DisplayName = "Contains value with ok")]
    public void ContainsValueWithOk() =>
        Result.Ok(5)
            .Should()
            .ContainValue()
            .Which.Should()
            .Be(5);

    [Fact(DisplayName = "Contains errors with ok")]
    public void ContainsErrorsWithOk()
    {
        var act = () => Result.Ok(5)
            .Should()
            .ContainErrors("no");

        act.Should()
            .Throw<Exception>()
            .WithMessage("Expected result to be failed because no, but found ok.");
    }

    [Fact(DisplayName = "Contains errors of type with ok")]
    public void ContainsErrorOfTypeWithOk()
    {
        var act = () => Result.Ok(5)
            .Should()
            .ContainErrorsOfType<MyError>("no");

        act.Should()
            .Throw<Exception>()
            .WithMessage($"Expected result to contain errors of type {typeof(MyError)} because no, but found none.");
    }

    [Fact(DisplayName = "Contains error of type with error")]
    public void ContainsErrorOfTypeWithError() =>
        Result.Fail<int>(new MyError("important"))
            .Should()
            .ContainErrorsOfType<MyError>()
            .Which
            .Should()
            .ContainSingle(static x => x.Message == "important");

    private record MyError(string Message) : Error(Message);
}