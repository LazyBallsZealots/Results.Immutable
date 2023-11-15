namespace Results.Immutable.Tests.ExtensionsTests;

public sealed class ResultOptionConvertExtensionsTests
{
    [Fact(DisplayName = "Converts a result with some value to an option")]
    public void ConvertsAResultWithSomeValueToAnOption() =>
        Result.Ok(5)
            .ToOption()
            .Should()
            .Be(Option.Some(5));

    [Fact(DisplayName = "Converts a result with error to an option")]
    public void ConvertsAResultWithErrorToAnOption() =>
        Result.Fail<string>("error")
            .ToOption()
            .Should()
            .Be(Option.None<string>());

    [Fact(DisplayName = "Converts an option with some value to a result")]
    public void ConvertsAnOptionWithSomeValueToAResult() =>
        Option.Some(5)
            .ToResult(Error.Create("error"))
            .Should()
            .Be(Result.Ok(5));

    [Fact(DisplayName = "Converts an option with none to a result")]
    public void ConvertsAnOptionWithNoneToAResult() =>
        Option.None<int>()
            .ToResult(Error.Create("error"))
            .Should()
            .Be(Result.Fail<int>("error"));

    [Fact(DisplayName = "Converts an option with some value to a result with error factory")]
    public void ConvertsAnOptionWithSomeValueToAResultWithErrorFactory() =>
        Option.Some(5)
            .ToResult(() => Error.Create("error"))
            .Should()
            .Be(Result.Ok(5));

    [Fact(DisplayName = "Converts an option with none to a result with error factory")]
    public void ConvertsAnOptionWithNoneToAResultWithErrorFactory() =>
        Option.None<int>()
            .ToResult(() => Error.Create("error"))
            .Should()
            .Be(Result.Fail<int>("error"));
}