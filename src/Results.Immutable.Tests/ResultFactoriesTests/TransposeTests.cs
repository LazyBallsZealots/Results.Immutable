using FsCheck;
using FsCheck.Xunit;

namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class TransposeTests
{
    [Property(DisplayName = "Transposition of a list of successful results should result in a success")]
    public Property TransposingAListOfSuccessfulResultsShouldReturnSuccessfulResult() =>
        Prop.ForAll(
            Gen.ListOf(
                    Arb.Generate<int>()
                        .Select(Result.Ok))
                .ToArbitrary(),
            static list =>
                list.Transpose() is { HasSucceeded: true, Some.Value: var enumerable, } &&
                enumerable.SequenceEqual(list.Select(static r => r.Some!.Value.Value)));

    [Fact(DisplayName = "Merger of a list of failed results aggregates all errors")]
    public void MergingAListOfFailedResultsAggregatesAllErrors() =>
        Result.Merge(
                Enumerable.Range(0, 100)
                    .Select(static i => $"Error number {i}")
                    .Select(Result.Fail)
                    .ToList())
            .Errors
            .Select(static (e, index) => (Error: e, Index: index))
            .Should()
            .AllSatisfy(
                static tuple => tuple.Error.Message.Should()
                    .EndWith(tuple.Index.ToString()));

    [Property(DisplayName = "Transposition of a list of results is a failure if any of them has failed")]
    public Property TransposingAListOrResultsIsAFailureIfAnyOfThemHasFailed() =>
        Prop.ForAll(
            Gen.ListOf(GetIntegerResultGenerator())
                .Where(static list => list.Any(static r => r.HasFailed))
                .ToArbitrary(),
            static list => list.Transpose()
                .HasFailed);

    [Fact(DisplayName = "Merger of an empty list of results is successful")]
    public void MergingOfAnEmptyListOfResultsIsSuccessful() =>
        Enumerable.Empty<Result<Unit>>()
            .Merge()
            .Should()
            .Match<Result<Unit>>(static r => r.HasSucceeded);

    [Property(
        DisplayName = "Transposition of successful Result params is successful",
        MaxTest = 1000)]
    public Property TransposingOfSuccessfulResultsIsSuccessful() =>
        Prop.ForAll(
            Gen.Two(
                    Arb.Generate<int>()
                        .Select(Result.Ok))
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;
                return Result.Transpose(first, second)
                    .HasSucceeded;
            });

    [Property(
        DisplayName = "Merger of Result params is a failure if any of them is a failure",
        MaxTest = 1000)]
    public Property TransposingOfResultsIsAFailureIfAnyOfThemIsAFailure() =>
        Prop.ForAll(
            Gen.Two(GetIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second) = tuple;
                        return first.HasFailed || second.HasFailed;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;
                return Result.Transpose(first, second)
                    .HasFailed;
            });

    private static Gen<Result<int>> GetIntegerResultGenerator() =>
        Gen.OneOf(
            Arb.Generate<int>()
                .Select(Result.Ok),
            Arb.Generate<string>()
                .Select(static em => Result.Fail<int>(em)));
}