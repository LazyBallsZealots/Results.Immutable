namespace Results.Immutable.Tests.OptionTests;

public sealed class OptionTransformationTests
{
    [Fact(DisplayName = "Selects a value via a transformation")]
    public void SelectsValueViaTransformation()
    {
        var option = Option.Some(1);

        var result = option.Select(static x => x + 1);

        result.Should()
            .Be(Option.Some(2));
    }

    [Fact(DisplayName = "Selects nothing via a transformation")]
    public void SelectsNothingViaTransformation()
    {
        var option = Option.None<int>();

        var fn = new Fn<int, int>(static x => x + 1);
        var result = option.Select(fn.Callable);

        result.Should()
            .Be(Option.None<int>());

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Selects many optional values via a transformation")]
    public void SelectsManyOptionalValuesViaTransformation()
    {
        var option = Option.Some(1);

        var result = option.SelectMany(static x => Option.Some(x + 1));

        result.Should()
            .Be(Option.Some(2));
    }

    [Fact(DisplayName = "Selects many via a transformation when given nothing")]
    public void SelectsManyViaTransformationWhenGivenNothing()
    {
        var option = Option.None<int>();

        var fn = new Fn<int, Option<int>>(static x => Option.Some(x + 1));
        var result = option.SelectMany(fn.Callable);

        result.Should()
            .Be(Option.None<int>());

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Selects many optional values via a transformation when resulting in nothing")]
    public void SelectsManyOptionalValuesViaTransformationWhenResultingInNothing()
    {
        var option = Option.Some(1);

        var result = option.SelectMany(static x => Option.None<int>());

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(DisplayName = "Selects many optional values via a transformation and a combinator")]
    public void SelectsManyOptionalValuesViaTransformationAndACombinator()
    {
        var option = Option.Some(1);

        var result = option.SelectMany(
            static x => Option.Some(x + 1),
            static (x, y) => x + y);

        result.Should()
            .Be(Option.Some(3));
    }

    [Fact(DisplayName = "Selects many optional values via a transformation and a combinator when given nothing")]
    public void SelectsManyOptionalValuesViaTransformationAndACombinatorWhenGivenNothing()
    {
        var option = Option.None<int>();

        var fnTransformation = new Fn<int, Option<int>>(static x => Option.Some(x + 1));
        var fnCombinator = new Fn<int, int, int>(static (x, y) => x + y);

        var result = option.SelectMany(
            fnTransformation.Callable,
            fnCombinator.Callable);

        result.Should()
            .Be(Option.None<int>());

        fnTransformation.CallCount.Should()
            .Be(0);

        fnCombinator.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Selects many optional values via a transformation and a combinator when resulting in nothing")]
    public void SelectsManyOptionalValuesViaTransformationAndACombinatorWhenResultingInNothing()
    {
        var option = Option.Some(1);

        var fn = new Fn<int, int, int>(static (x, y) => x + y);
        var result = option.SelectMany(
            static x => Option.None<int>(),
            fn.Callable);

        result.Should()
            .Be(Option.None<int>());

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Asynchronously projects an existing option")]
    public async Task AsynchronouslyProjectsAnOption()
    {
        var asyncOption = new ValueTask<Option<int>>(Option.Some(1));
        var result = await asyncOption.SelectAsync(static x => new ValueTask<int>(x + 1));

        result.Should()
            .Be(Option.Some(2));
    }

    [Fact(DisplayName = "Asynchronously projects Option.None")]
    public async Task AsynchronouslyProjectsOptionNone()
    {
        var asyncOption = new ValueTask<Option<int>>(Option.None<int>());
        var result = await asyncOption.SelectAsync(ThrowAsync<int, int>);

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(DisplayName = "Projects and flattens async option")]
    public async Task ProjectsAndFlattensAsyncOption()
    {
        var asyncOption = new ValueTask<Option<int>>(Option.Some(1));
        var result = await asyncOption.SelectMany(static i => Option.Some(i + 2));

        result.Should()
            .Be(Option.Some(3));
    }

    [Fact(DisplayName = "Projects and flattens async Option.None")]
    public async Task ProjectsAndFlattensAsyncOptionNone()
    {
        var asyncNone = new ValueTask<Option<int>>(Option.None<int>());
        var result = await asyncNone.SelectMany<int, int>(
            _ => throw new InvalidOperationException("Projected Option.None!"));

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(DisplayName = "Asynchronously projects and flattens async option with intermediate selector")]
    public async Task ProjectsAndFlattensAsyncOptionWithIntermediateSelector()
    {
        var asyncOption = new ValueTask<Option<int>>(Option.Some(1));
        var result = await asyncOption.SelectManyAsync(
            i => new ValueTask<Option<int>>(Option.Some(i + 2)),
            static (initial, intermediate) => initial + intermediate);

        result.Should()
            .Be(Option.Some(4));
    }

    [Fact(DisplayName = "Asynchronously projects and flattens async Option.None with intermediate selector")]
    public async Task ProjectsAndFlattensAsyncNoneWithIntermediateSelector()
    {
        var asyncNone = new ValueTask<Option<int>>(Option.None<int>());
        var result = await asyncNone.SelectManyAsync<int, int, int>(
            ThrowAsync<int, Option<int>>,
            static (_, _) => throw new InvalidOperationException("Invoked selector for Option.None!"));

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(DisplayName = "Filters in a value via a predicate")]
    public void FiltersInAValueViaAPredicate()
    {
        var option = Option.Some(1);

        var result = option.Where(static x => x > 0);

        result.Should()
            .Be(option);
    }

    [Fact(DisplayName = "Filters out a value via a predicate")]
    public void FiltersOutAValueViaAPredicate()
    {
        var option = Option.Some(1);

        var result = option.Where(static x => x < 0);

        result.Should()
            .Be(Option.None<int>());
    }

    [Fact(DisplayName = "Filters nothing via a predicate when given nothing")]
    public void FiltersNothingViaAPredicateWhenGivenNothing()
    {
        var option = Option.None<int>();

        var fn = new Fn<int, bool>(static x => x > 0);
        var result = option.Where(fn.Callable);

        result.Should()
            .Be(Option.None<int>());

        fn.CallCount.Should()
            .Be(0);
    }

    private static ValueTask<TOut> ThrowAsync<TIn, TOut>(TIn _) =>
        throw new InvalidOperationException("Asynchronously projected Option.None!");
}