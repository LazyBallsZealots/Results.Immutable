using Results.Immutable.Member;

namespace Results.Immutable.Tests.MemberTests;

public class EachParserTests
{
    [Fact(
        DisplayName =
            "Parses each element of a list and returns a result containing IndexErrors with inner errors created by the parser in case of failure.")]
    public void
        ParsesEachElementOfAListAndReturnsAResultContainingIndexErrorsWithInnerErrorsCreatedByTheParserInCaseOfFailure()
    {
        var result = new[]
        {
            1,
            3,
            4,
            5,
        }.ParseEach(
            age => Result.OkIf(
                age > 3,
                age,
                new Error("Too young")),
            "The user should not be a toddler");

        result.Errors.Should()
            .BeEquivalentTo(
                ImmutableList.Create(
                    new IndexError(
                        0,
                        "The user should not be a toddler",
                        ImmutableList.Create(new Error("Too young"))),
                    new IndexError(
                        1,
                        "The user should not be a toddler",
                        ImmutableList.Create(new Error("Too young")))));
    }

    [Fact(DisplayName = "Parses each element of a list successfully.")]
    public void ParsesEachElementOfAListSuccessfully()
    {
        var result = new[]
        {
            7,
            8,
        }.ParseEach(
            age => Result.OkIf(
                age > 3,
                age,
                new Error("Too young")),
            "The user should not be a toddler");

        result.Should()
            .ContainValue()
            .Which.Should()
            .BeEquivalentTo(
                new[]
                {
                    7,
                    8,
                });
    }

    [Fact(
        DisplayName =
            "Parses each element of a list and returns a result containing IndexErrors with inner errors created by the parser in case of failure asynchronously.")]
    public async Task
        ParsesEachElementOfAListAndReturnsAResultContainingIndexErrorsWithInnerErrorsCreatedByTheParserInCaseOfFailureAsync()
    {
        var result = await new[]
        {
            1,
            3,
            4,
            5,
        }.ParseEachAsync(
            age => ValueTask.FromResult(
                Result.OkIf(
                    age > 3,
                    age,
                    new Error("Too young"))),
            "The user should not be a toddler");

        result.Errors.Should()
            .BeEquivalentTo(
                ImmutableList.Create(
                    new IndexError(
                        0,
                        "The user should not be a toddler",
                        ImmutableList.Create(new Error("Too young"))),
                    new IndexError(
                        1,
                        "The user should not be a toddler",
                        ImmutableList.Create(new Error("Too young")))));
    }

    [Fact(DisplayName = "Parses each element of a list successfully asynchronously.")]
    public async Task ParsesEachElementOfAListSuccessfullyAsync()
    {
        var result = await new[]
        {
            7,
            8,
        }.ParseEachAsync(
            age => ValueTask.FromResult(
                Result.OkIf(
                    age > 3,
                    age,
                    new Error("Too young"))),
            "The user should not be a toddler");

        result.Should()
            .ContainValue()
            .Which.Should()
            .BeEquivalentTo(
                new[]
                {
                    7,
                    8,
                });
    }
}