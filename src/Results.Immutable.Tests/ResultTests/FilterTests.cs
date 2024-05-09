namespace Results.Immutable.Tests.ResultTests;

public sealed class FilterTests
{
    [Fact(DisplayName = "Filters in a value that passes a predicate")]
    public void FilterPassesPredicate() =>
        Result.Ok(5)
            .Where(static x => x > 0, new Error("irrelevant"))
            .Should()
            .ContainValue()
            .Which
            .Should()
            .Be(5);

    [Fact(DisplayName = "Filters out a value that fails a predicate")]
    public void FilterFailsPredicate()
    {
        Error error = new("epic");
        Result.Ok(0)
            .Where(static x => x > 0, error)
            .Errors
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Be(error);
    }

    [Fact(DisplayName = "Gives back the already failed error")]
    public void GivesBackAlreadyFailedError()
    {
        var fn = new Fn<Unit, bool>(_ => true);
        var result = Result.Fail(new Error("unique"));
        result.Where(fn.Callable, new Error("irrelevant"))
            .Should()
            .Be(result);

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Filters in a value that passes a predicate with a factory")]
    public void FilterPassesPredicateWithFactory()
    {
        var fn = new Fn<Error>(() => new("irrelevant"));
        Result.Ok(5)
            .Where(static x => x > 0, fn)
            .Should()
            .ContainValue()
            .Which
            .Should()
            .Be(5);

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Filters out a value that fails a predicate with a factory")]
    public void FilterFailsPredicateWithFactory()
    {
        Error error = new("abysmal");
        Result.Ok(0)
            .Where(static x => x > 0, () => error)
            .Errors
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Be(error);
    }

    [Fact(DisplayName = "Gives back the already failed error with factory")]
    public void GivesBackAlreadyFailedErrorWithFactory()
    {
        var fnPredicate = new Fn<Unit, bool>(_ => true);
        var fnError = new Fn<Error>(() => new("irrelevant"));
        var result = Result.Fail(new Error("unique"));
        result.Where(fnPredicate.Callable, fnError)
            .Should()
            .Be(result);

        fnPredicate.CallCount.Should()
            .Be(0);

        fnError.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Filters in asynchronously a value via a predicate")]
    public async Task FiltersInAsynchronouslyAValueViaAPredicate()
    {
        var result = await Result.Ok(5)
            .WhereAsync(static x => ValueTask.FromResult(x > 0), new Error("irrelevant"));

        result
            .Should()
            .ContainValue()
            .Which
            .Should()
            .Be(5);
    }

    [Fact(DisplayName = "Filters out asynchronously a value via a predicate")]
    public async Task FiltersOutAsynchronouslyAValueViaAPredicate()
    {
        Error error = new("epic");
        var result = await Result.Ok(0)
            .WhereAsync(static x => ValueTask.FromResult(x > 0), error);

        result
            .Errors
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Be(error);
    }

    [Fact(DisplayName = "Gives back asynchronously the already failed error ")]
    public async Task GivesBackAsynchronouslyAlreadyFailedError()
    {
        var fn = new Fn<Unit, ValueTask<bool>>(_ => ValueTask.FromResult(true));
        var result = Result.Fail(new Error("unique"));
        var filtered = await result.WhereAsync(fn.Callable, new Error("irrelevant"));
        filtered.Should()
            .Be(result);

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Filters in asynchronously a value that passes a predicate with a factory")]
    public async Task FiltersInAsynchronouslyAValueThatPassesAPredicateWithFactory()
    {
        var fn = new Fn<Error>(() => new("irrelevant"));
        var result = await Result.Ok(5)
            .WhereAsync(static x => ValueTask.FromResult(x > 0), fn);

        result
            .Should()
            .ContainValue()
            .Which
            .Should()
            .Be(5);

        fn.CallCount.Should()
            .Be(0);
    }

    [Fact(DisplayName = "Filters out asynchronously a value that fails a predicate with a factory")]
    public async Task FiltersOutAsynchronouslyAValueThatFailsAPredicateWithFactory()
    {
        Error error = new("abysmal");
        var result = await Result.Ok(0)
            .WhereAsync(static x => ValueTask.FromResult(x > 0), () => error);

        result
            .Errors
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Be(error);
    }

    [Fact(DisplayName = "Gives back asynchronously the already failed error with factory")]
    public async Task GivesBackAsynchronouslyAlreadyFailedErrorWithFactory()
    {
        var fnPredicate = new Fn<Unit, ValueTask<bool>>(_ => ValueTask.FromResult(true));
        var fnError = new Fn<Error>(() => new("irrelevant"));
        var result = Result.Fail(new Error("unique"));
        var filtered = await result.WhereAsync(fnPredicate.Callable, fnError);
        filtered.Should()
            .Be(result);

        fnPredicate.CallCount.Should()
            .Be(0);

        fnError.CallCount.Should()
            .Be(0);
    }
}