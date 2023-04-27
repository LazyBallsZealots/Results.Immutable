namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class FailTests
{
    [Fact(DisplayName = "Creates a failed result with one error from error message")]
    public void CreatesAFailedResultWithOneErrorFromErrorMessage()
    {
        var r = Result.Fail("space shuttle blew");
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Single()
            .Message.Should()
            .Be("space shuttle blew");
    }

    [Fact(DisplayName = "Creates a failed result with one error from error record")]
    public void CreatesAFailedResultWithOneErrorFromErrorRecord()
    {
        var error = new Error("space shuttle blew");

        var r = Result.Fail(error);
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Single()
            .Should()
            .Be(error);
    }

    [Fact(DisplayName = "Creates a failed result with a collection of errors")]
    public void CreatesAFailedResultWithACollectionOfErrors()
    {
        var errors = ImmutableList.Create(new Error("space shuttle blew"), new Error("rocket exploded"));

        var r = Result.Fail(errors);
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Should()
            .BeEquivalentTo(errors);
    }

    [Fact(DisplayName = "Creates a failed result with a collection of errors as params")]
    public void CreatesAFailedResultWithACollectionOfErrorsAsParams()
    {
        var r = Result.Fail(new Error("space shuttle blew"), new Error("rocket exploded"));
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Should()
            .BeEquivalentTo(ImmutableList.Create(new Error("space shuttle blew"), new Error("rocket exploded")));
    }

    [Fact(DisplayName = "Creates a failed result of a provided type with one error")]
    public void CreatesFailedResultOfProvidedTypeFromErrorMessage()
    {
        var r = Result.Fail<int>("space shuttle blew");
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Single()
            .Message.Should()
            .Be("space shuttle blew");
    }

    [Fact(DisplayName = "Creates a failed result of a provided type with one error from error record")]
    public void CreatesAFailedResultOfProvidedTypeWithOneErrorFromErrorRecord()
    {
        var error = new Error("space shuttle blew");

        var r = Result.Fail<int>(error);
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Single()
            .Should()
            .Be(error);
    }

    [Fact(DisplayName = "Creates a failed result of a provided type with a collection of errors")]
    public void CreatesAFailedResultOfProvidedTypeWithACollectionOfErrors()
    {
        var errors = ImmutableList.Create(new Error("space shuttle blew"), new Error("rocket exploded"));

        var r = Result.Fail<int>(errors);
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Should()
            .BeEquivalentTo(errors);
    }

    [Fact(DisplayName = "Creates a failed result of a provided type with a collection of errors as params")]
    public void CreatesAFailedResultOfProvidedTypeWithACollectionOfErrorsAsParams()
    {
        var r = Result.Fail<int>(new Error("space shuttle blew"), new Error("rocket exploded"));
        r.IsErrored.Should()
            .BeTrue();

        r.Errors.Should()
            .BeEquivalentTo(ImmutableList.Create(new Error("space shuttle blew"), new Error("rocket exploded")));
    }
}