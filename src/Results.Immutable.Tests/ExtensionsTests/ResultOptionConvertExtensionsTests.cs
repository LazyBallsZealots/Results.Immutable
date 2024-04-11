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

    [Fact(DisplayName = "Transposes an option to a result with value")]
    public void TransposesAnOptionToAResultWithValue() =>
        Option.Some(Result.Ok(5))
            .Transpose()
            .Should()
            .Be(Result.Ok(Option.Some(5)));

    [Fact(DisplayName = "Transposes a option to a result with errors")]
    public void TransposesAnOptionToAResultWithErrors() =>
        Option.Some(Result.Fail<int>("cow"))
            .Transpose()
            .Should()
            .Be(Result.Fail<Option<int>>("cow"));

    [Fact(DisplayName = "Transposes a option to a result with none")]
    public void TransposesAnOptionToAResultWithNone() =>
        Option.None<Result<int>>()
            .Transpose()
            .Should()
            .Be(Result.Ok(Option.None<int>()));

    [Fact(DisplayName = "Transposes a result to an option with value")]
    public void TransposesAResultToAnOptionWithValue() =>
        Result.Ok(Option.Some(5))
            .Transpose()
            .Should()
            .Be(Option.Some(Result.Ok(5)));

    [Fact(DisplayName = "Transposes a result to an option with errors")]
    public void TransposesAResultToAnOptionWithErrors() =>
        Result.Fail<Option<int>>("monkey")
            .Transpose()
            .Should()
            .Be(Option.Some(Result.Fail<int>("monkey")));

    [Fact(DisplayName = "Transposes a result to an option with none")]
    public void TransposesAResultToAnOptionWithNone() =>
        Result.Ok(Option.None<int>())
            .Transpose()
            .Should()
            .Be(Option.None<Result<int>>());
}