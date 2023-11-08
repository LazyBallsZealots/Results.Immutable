using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using Results.Immutable.Tests.Generators;

namespace Results.Immutable.Tests.Extensions;

public sealed class ZipExtensionsTests
{
    [Property(DisplayName = "Aggregates errors from two results")]
    public Property AggregatesErrorsFromTwoResults() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Two()
                .ToArbitrary(),
            static tuple =>
            {
                var (firstResult, secondResult) = tuple;
                var zipped = firstResult.Zip(secondResult);
                var errors = firstResult.Errors.AddRange(secondResult.Errors);
                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns a tuple of the values of the two results")]
    public void ReturnsATupleOfTheValuesOfTheTwoResults()
    {
        Result.Ok(1)
            .Zip(Result.Ok(2))
            .Should()
            .BeEquivalentTo(Result.Ok((1, 2)));
    }

    [Property(DisplayName = "Aggregates errors from three results")]
    public Property AggregatesErrorsFromThreeResults() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Three()
                .ToArbitrary(),
            static tuple =>
            {
                var (firstResult, secondResult, thirdResult) = tuple;
                var zipped = firstResult.Zip(secondResult, thirdResult);
                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns a tuple of the values of the three results")]
    public void ReturnsATupleOfTheValuesOfTheThreeResults()
    {
        Result.Ok(1)
            .Zip(Result.Ok(2), Result.Ok(3))
            .Should()
            .BeEquivalentTo(Result.Ok((1, 2, 3)));
    }

    [Property(DisplayName = "Aggregates errors from four results")]
    public Property AggregatesErrorsFromFourResults() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Four()
                .ToArbitrary(),
            static tuple =>
            {
                var (firstResult, secondResult, thirdResult, fourthResult) = tuple;
                var zipped = firstResult.Zip(
                    secondResult,
                    thirdResult,
                    fourthResult);

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors)
                    .AddRange(fourthResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns a tuple of the values of the four results")]
    public void ReturnsATupleOfTheValuesOfTheFourResults()
    {
        Result.Ok(1)
            .Zip(
                Result.Ok(2),
                Result.Ok(3),
                Result.Ok(4))
            .Should()
            .BeEquivalentTo(Result.Ok((1, 2, 3, 4)));
    }

    [Property(DisplayName = "Aggregates errors from five results")]
    public Property AggregatesErrorsFromFiveResults() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Two()
                .Three()
                .ToArbitrary(),
            static tuple =>
            {
                var ((firstResult, secondResult), (thirdResult, fourthResult), (fifthResult, _)) = tuple;
                var zipped = firstResult.Zip(
                    secondResult,
                    thirdResult,
                    fourthResult,
                    fifthResult);

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors)
                    .AddRange(fourthResult.Errors)
                    .AddRange(fifthResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns a tuple of the values of the five results")]
    public void ReturnsATupleOfTheValuesOfTheFiveResults()
    {
        Result.Ok(1)
            .Zip(
                Result.Ok(2),
                Result.Ok(3),
                Result.Ok(4),
                Result.Ok(5))
            .Should()
            .BeEquivalentTo(Result.Ok((1, 2, 3, 4, 5)));
    }

    [Property(DisplayName = "Aggregates errors from two results instead of calling the zipper")]
    public Property AggregatesErrorsFromTwoResultsInsteadOfCallingTheZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Two()
                .ToArbitrary(),
            static tuple =>
            {
                var (firstResult, secondResult) = tuple;
                var zipped = firstResult.ZipWith(secondResult, (a, b) => (a, b));
                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the zipper for two results")]
    public void ReturnsATupleOfTheValuesOfTheTwoResultsInsteadOfCallingTheZipper()
    {
        Result.Ok(1)
            .ZipWith(Result.Ok(2), (a, b) => a + b)
            .Should()
            .BeEquivalentTo(Result.Ok(3));
    }

    [Property(DisplayName = "Aggregates errors from three results instead of calling the zipper")]
    public Property AggregatesErrorsFromThreeResultsInsteadOfCallingTheZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Three()
                .ToArbitrary(),
            static tuple =>
            {
                var (firstResult, secondResult, thirdResult) = tuple;
                var zipped = firstResult.ZipWith(
                    secondResult,
                    thirdResult,
                    (
                        a,
                        b,
                        c) => (a, b, c));

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the zipper for three results")]
    public void ReturnsATupleOfTheValuesOfTheThreeResultsInsteadOfCallingTheZipper()
    {
        Result.Ok(1)
            .ZipWith(
                Result.Ok(2),
                Result.Ok(3),
                (
                    a,
                    b,
                    c) => a + b + c)
            .Should()
            .BeEquivalentTo(Result.Ok(6));
    }

    [Property(DisplayName = "Aggregates errors from four results instead of calling the zipper")]
    public Property AggregatesErrorsFromFourResultsInsteadOfCallingTheZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Four()
                .ToArbitrary(),
            static tuple =>
            {
                var (firstResult, secondResult, thirdResult, fourthResult) = tuple;
                var zipped = firstResult.ZipWith(
                    secondResult,
                    thirdResult,
                    fourthResult,
                    (
                        a,
                        b,
                        c,
                        d) => (a, b, c, d));

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors)
                    .AddRange(fourthResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the zipper for four results")]
    public void ReturnsATupleOfTheValuesOfTheFourResultsInsteadOfCallingTheZipper()
    {
        Result.Ok(1)
            .ZipWith(
                Result.Ok(2),
                Result.Ok(3),
                Result.Ok(4),
                (
                    a,
                    b,
                    c,
                    d) => a + b + c + d)
            .Should()
            .BeEquivalentTo(Result.Ok(10));
    }

    [Property(DisplayName = "Aggregates errors from five results instead of calling the zipper")]
    public Property AggregatesErrorsFromFiveResultsInsteadOfCallingTheZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Two()
                .Three()
                .ToArbitrary(),
            static tuple =>
            {
                var ((firstResult, secondResult), (thirdResult, fourthResult), (fifthResult, _)) = tuple;
                var zipped = firstResult.ZipWith(
                    secondResult,
                    thirdResult,
                    fourthResult,
                    fifthResult,
                    (
                        a,
                        b,
                        c,
                        d,
                        e) => (a, b, c, d, e));

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors)
                    .AddRange(fourthResult.Errors)
                    .AddRange(fifthResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the zipper for five results")]
    public void ReturnsATupleOfTheValuesOfTheFiveResultsInsteadOfCallingTheZipper()
    {
        Result.Ok(1)
            .ZipWith(
                Result.Ok(2),
                Result.Ok(3),
                Result.Ok(4),
                Result.Ok(5),
                (
                    a,
                    b,
                    c,
                    d,
                    e) => a + b + c + d + e)
            .Should()
            .BeEquivalentTo(Result.Ok(15));
    }

    [Property(DisplayName = "Aggregates errors from two results instead of calling the async zipper")]
    public Property AggregatesErrorsFromTwoResultsInsteadOfCallingTheAsyncZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Two()
                .ToArbitrary(),
            static async tuple =>
            {
                var (firstResult, secondResult) = tuple;
                var zipped = await firstResult.ZipWithAsync(secondResult, (a, b) => ValueTask.FromResult(a + b));
                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the async zipper for two results")]
    public async Task ReturnsTheResultOfTheAsyncZipperForTwoResults()
    {
        (await Result.Ok(1)
                .ZipWithAsync(Result.Ok(2), (a, b) => ValueTask.FromResult(a + b)))
            .Should()
            .BeEquivalentTo(Result.Ok(3));
    }

    [Property(DisplayName = "Aggregates errors from three results instead of calling the async zipper")]
    public Property AggregatesErrorsFromThreeResultsInsteadOfCallingTheAsyncZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Three()
                .ToArbitrary(),
            static async tuple =>
            {
                var (firstResult, secondResult, thirdResult) = tuple;
                var zipped = await firstResult.ZipWithAsync(
                    secondResult,
                    thirdResult,
                    (
                        a,
                        b,
                        c) => ValueTask.FromResult(a + b + c));

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the async zipper for three results")]
    public async Task ReturnsTheResultOfTheAsyncZipperForThreeResults()
    {
        (await Result.Ok(1)
                .ZipWithAsync(
                    Result.Ok(2),
                    Result.Ok(3),
                    (
                        a,
                        b,
                        c) => ValueTask.FromResult(a + b + c)))
            .Should()
            .BeEquivalentTo(Result.Ok(6));
    }

    [Property(DisplayName = "Aggregates errors from four results instead of calling the async zipper")]
    public Property AggregatesErrorsFromFourResultsInsteadOfCallingTheAsyncZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Four()
                .ToArbitrary(),
            static async tuple =>
            {
                var (firstResult, secondResult, thirdResult, fourthResult) = tuple;
                var zipped = await firstResult.ZipWithAsync(
                    secondResult,
                    thirdResult,
                    fourthResult,
                    (
                        a,
                        b,
                        c,
                        d) => ValueTask.FromResult(a + b + c + d));

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors)
                    .AddRange(fourthResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the async zipper for four results")]
    public async Task ReturnsTheResultOfTheAsyncZipperForFourResults()
    {
        (await Result.Ok(1)
                .ZipWithAsync(
                    Result.Ok(2),
                    Result.Ok(3),
                    Result.Ok(4),
                    (
                        a,
                        b,
                        c,
                        d) => ValueTask.FromResult(a + b + c + d)))
            .Should()
            .BeEquivalentTo(Result.Ok(10));
    }

    [Property(DisplayName = "Aggregates errors from five results instead of calling the async zipper")]
    public Property AggregatesErrorsFromFiveResultsInsteadOfCallingTheAsyncZipper() =>
        Prop.ForAll(
            ResultOf<uint>.Generator()
                .Two()
                .Three()
                .ToArbitrary(),
            static async tuple =>
            {
                var ((firstResult, secondResult), (thirdResult, fourthResult), (fifthResult, _)) = tuple;
                var zipped = await firstResult.ZipWithAsync(
                    secondResult,
                    thirdResult,
                    fourthResult,
                    fifthResult,
                    (
                        a,
                        b,
                        c,
                        d,
                        e) => ValueTask.FromResult(a + b + c + d + e));

                var errors = firstResult.Errors
                    .AddRange(secondResult.Errors)
                    .AddRange(thirdResult.Errors)
                    .AddRange(fourthResult.Errors)
                    .AddRange(fifthResult.Errors);

                return zipped.Errors.SequenceEqual(errors);
            });

    [Fact(DisplayName = "Returns the result of the async zipper for five results")]
    public async Task ReturnsTheResultOfTheAsyncZipperForFiveResults()
    {
        (await Result.Ok(1)
                .ZipWithAsync(
                    Result.Ok(2),
                    Result.Ok(3),
                    Result.Ok(4),
                    Result.Ok(5),
                    (
                        a,
                        b,
                        c,
                        d,
                        e) => ValueTask.FromResult(a + b + c + d + e)))
            .Should()
            .BeEquivalentTo(Result.Ok(15));
    }
}