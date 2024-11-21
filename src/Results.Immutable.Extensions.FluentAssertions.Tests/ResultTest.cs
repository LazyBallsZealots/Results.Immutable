using Results.Immutable.Member;

namespace Results.Immutable.Extensions.FluentAssertions.Tests;

public sealed class ResultTest
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
        var act = static () => Result.Ok(5)
            .Should()
            .ContainErrors("no");

        act.Should()
            .Throw<Exception>()
            .WithMessage("Expected result to be failed because no, but found ok.");
    }

    [Fact(DisplayName = "Contains errors of type with ok")]
    public void ContainsErrorOfTypeWithOk()
    {
        var act = static () => Result.Ok(5)
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

    [Fact(DisplayName = "Contains error of type with a hierarchy of errors")]
    public void ContainsErrorsOfTypeWithNestedError() =>
        Result.Fail<int>(new IndexError(0, "whoops!").WithRootCause(new MyError("The real reason")))
            .Should()
            .ContainErrorsOfType<MyError>()
            .Which.Should()
            .ContainSingle(static e => e.Message == "The real reason");

    [Fact(DisplayName = "Contains top-level errors of given type with ok")]
    public void ContainsTopLevelErrorsOfTypeWithOk()
    {
        var act = static () => Result.Ok()
            .Should()
            .ContainTopLevelErrorsOfType<Error>();

        act.Should()
            .Throw<Exception>();
    }

    [Fact(DisplayName = "Contains top-level errors of given type for failures not containing matching errors")]
    public void ContainsTopLevelErrorsOfTypeWithFailureNotContainingMatchingErrors()
    {
        var act = static () => Result.Fail(new Error("Not my error!"))
            .Should()
            .ContainTopLevelErrorsOfType<MyError>();

        act.Should()
            .Throw<Exception>();
    }

    [Fact(DisplayName = "Contains top-level errors of given type for failure containing matching errors")]
    public void ContainsTopLevelErrorsOfTypeWithFailureContainingMatchingErrors() =>
        Result.Fail(new MyError("Whoooops!"))
            .Should()
            .ContainTopLevelErrorsOfType<MyError>()
            .Which.Should()
            .ContainSingle(static e => e.Message == "Whoooops!");

    private sealed record MyError(string Message) : Error(Message);
}