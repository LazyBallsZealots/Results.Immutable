using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using Results.Immutable.Tests.Generators;

namespace Results.Immutable.Tests.ResultTests;

public sealed class ResultProjectionTests
{
    private static Result<Unit> SuccessfulResult => Result.Ok();

    [Fact(DisplayName = "No-op projection of a successful result of unit should return a result of unit")]
    public void NoOpBindOnSuccessfulResultShouldReturnAUnit() =>
        SuccessfulResult.Select(static _ => Unit.Value)
            .Should()
            .Be(SuccessfulResult);

    [Fact(DisplayName = "No-op async projection of a successful result of unit should return a result of unit")]
    public async Task NoOpAsyncProjectionOnSuccessfulResultShouldReturnAUnit() =>
        (await SuccessfulResult.SelectAsync(async _ => await new ValueTask<Unit>(Unit.Value)))
        .Should()
        .Be(SuccessfulResult);

    [Property(DisplayName = "SelectMany on successful result with value converts it")]
    public Property BindOnSuccessfulResultWithValueConvertsIt(object? firstValue, object? finalValue) =>
        (Result.Ok(firstValue)
                    .SelectMany(_ => Result.Ok(finalValue))
                is { IsOk: true, Some.Value: var value, } &&
            value?.Equals(finalValue) is null or true)
        .ToProperty();

    [Property(DisplayName = "Asynchronous SelectMany on successful result with value converts it")]
    public Property AsynchronousSelectManyOnSuccesfulResultWithValueConvertsIt() =>
        Prop.ForAll(
            ArbMap.Default.GeneratorFor<object?>()
                .Two()
                .ToArbitrary(),
            async tuple =>
            {
                var (initialValue, finalValue) = tuple;

                return await Result.Ok(initialValue)
                            .SelectManyAsync(_ => new ValueTask<Result<object?>>(Result.Ok(finalValue)))
                        is { Some.Value: var value, } &&
                    value?.Equals(finalValue) is null or true;
            });

    [Fact(DisplayName = "No-op SelectMany should return result of unit")]
    public void NoOpBindingOnMultipleResultsShouldReturnResultOfUnit() =>
        SuccessfulResult.SelectMany(
                static _ => Result.Ok(),
                static (_, _) => Unit.Value)
            .Should()
            .Be(SuccessfulResult);

    [Fact(DisplayName = "No-op async SelectMany should return result of unit")]
    public async Task NoOpSelectManyShouldReturnResultOfUnit() =>
        (await SuccessfulResult.SelectManyAsync(
            static _ => new ValueTask<Result<Unit>>(Result.Ok()),
            static (_, _) => Unit.Value))
        .Should()
        .Be(SuccessfulResult);

    [Property(
        DisplayName = "Projecting two results should return a success or a failure, depending on initial results")]
    public Property BindingOnTwoSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue() =>
        Prop.ForAll(
            SelectMany.Generator()
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return (first, second) switch
                {
                    ({ IsOk: true, }, { IsOk: true, }) => first.SelectMany(_ => second, selector) is
                        {
                            Some.Value: var selectedValue,
                        } &&
                        selectedValue?.Equals(finalValue) is true or null,
                    ({ IsErrored: true, }, _) => first.SelectMany(_ => second, selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    (_, _) => first.SelectMany(_ => second, selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "Second errored out"),
                };
            });

    [Property(DisplayName = "Asynchronous projection of two results should return a matching result")]
    public Property AsyncSelectManyShouldReturnAMatchingResult() =>
        Prop.ForAll(
            SelectMany.Generator()
                .ToArbitrary(),
            async tuple =>
            {
                var (first, second, selector, finalValue) = tuple;

                return (first, second) switch
                {
                    ({ IsOk: true, }, { IsOk: true, }) => await first.SelectManyAsync(SecondAsValueTask, selector) is
                        {
                            Some.Value: var value,
                        } &&
                        value?.Equals(finalValue) is null or true,
                    ({ IsErrored: true, }, _) =>
                        await first.SelectManyAsync(SecondAsValueTask, selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "First errored out"),
                    _ => await first.SelectManyAsync(SecondAsValueTask, selector) is var failedResult &&
                        failedResult.HasError(static (Error e) => e.Message == "Second errored out"),
                };

                ValueTask<Result<object?>> SecondAsValueTask(object? _) => new(second);
            });
}