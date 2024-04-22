namespace Results.Immutable.Tests.OptionTests;

public class OptionProjectionTests
{
    [Fact(DisplayName = "Selects many async optional values via a transformation")]
    public async Task SelectsManyAsyncOptionalValuesViaTransformation()
    {
        var result = await Option.Some(1)
            .SelectManyAsync(static x => new ValueTask<Option<int>>(Option.Some(x + 1)));

        result.Should()
            .Be(Option.Some(2));
    }

    [Fact(DisplayName = "Selects many async optional values via a transformation when given nothing")]
    public async Task SelectsManyAsyncOptionalValuesViaTransformationWhenGivenNothing()
    {
        var result = await Option.None<int>()
            .SelectManyAsync(static x => new ValueTask<Option<int>>(Option.Some(x + 1)));

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(DisplayName = "Selects many async optional values via a transformation when resulting in nothing")]
    public async Task SelectsManyAsyncOptionalValuesViaTransformationWhenResultingInNothing()
    {
        var result = await Option.Some(1)
            .SelectManyAsync(static x => new ValueTask<Option<int>>(Option.None<int>()));

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(DisplayName = "Selects many async optional values via a transformation and a combinator")]
    public async Task SelectsManyAsyncOptionalValuesViaTransformationAndACombinator()
    {
        var result = await Option.Some(1)
            .SelectManyAsync(
                static x => new ValueTask<Option<int>>(Option.Some(x + 1)),
                static (x, y) => x + y);

        result.Should()
            .Be(Option.Some(3));
    }

    [Fact(DisplayName = "Selects many async optional values via a transformation and a combinator when given nothing")]
    public async Task SelectsManyAsyncOptionalValuesViaTransformationAndACombinatorWhenGivenNothing()
    {
        var result = await Option.None<int>()
            .SelectManyAsync(
                static x => new ValueTask<Option<int>>(Option.Some(x + 1)),
                static (x, y) => x + y);

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(
        DisplayName =
            "Selects many async optional values via a transformation and a combinator when resulting in nothing")]
    public async Task SelectsManyAsyncOptionalValuesViaTransformationAndACombinatorWhenResultingInNothing()
    {
        var result = await Option.Some(1)
            .SelectManyAsync(
                static x => new ValueTask<Option<int>>(Option.None<int>()),
                static (x, y) => x + y);

        result.Should()
            .Be(Option.None<int>());
    }
}