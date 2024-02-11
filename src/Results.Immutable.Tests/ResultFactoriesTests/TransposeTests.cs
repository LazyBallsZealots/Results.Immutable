using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class TransposeTests
{
    [Property(DisplayName = "Transposition of a list of successful results should result in a success")]
    public Property TransposingAListOfSuccessfulResultsShouldReturnSuccessfulResult() =>
        Prop.ForAll(
            ArbMap.Default.GeneratorFor<int>()
                .Select(Result.Ok)
                .ListOf()
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
            GetIntegerResultGenerator()
                .ListOf()
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

    [Fact(DisplayName = "Merger of a list of successful results is a success")]
    public void MergingAListOfSuccessfulResultsIsSuccessful() =>
        Enumerable.Repeat(Result.Ok(), 10)
            .Merge()
            .HasSucceeded
            .Should()
            .BeTrue();

    [Property(DisplayName = "Merger of a list of multiple successful results returns success")]
    public Property MergingAListOfMultipleSuccessesReturnsSuccess() =>
        Prop.ForAll(
            Gen.Constant(Result.Ok())
                .NonEmptyListOf()
                .ToArbitrary(),
            static list => list.Merge()
                .HasSucceeded);

    [Property(DisplayName = "Merger of a list which contains at least one failure is a failure")]
    public Property MergingAListOfMultipleResultsWithAtLeastOneFailureIsAFailure() =>
        Prop.ForAll(
            Gen.OneOf(
                    Gen.Constant(Result.Ok()),
                    Gen.Constant(Result.Fail("Errored out!")))
                .NonEmptyListOf()
                .Where(static list => list.Any(static r => r.HasFailed))
                .ToArbitrary(),
            static list => list.Merge()
                .HasFailed);

    [Fact(DisplayName = "Merger of successful Result params is a success")]
    public void MergingTwoSuccessfulResultsIsASuccess() =>
        Result.Merge(
                Result.Ok(),
                Result.Ok())
            .HasSucceeded
            .Should()
            .BeTrue();

    [Property(
        DisplayName = "Transposition of successful Result params is successful",
        MaxTest = 1000)]
    public Property TransposingOfSuccessfulResultsIsSuccessful() =>
        Prop.ForAll(
            ArbMap.Default.GeneratorFor<int>()
                .Select(Result.Ok)
                .Two()
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;
                return Result.Transpose(first, second)
                    .HasSucceeded;
            });

    [Property(
        DisplayName = "Transposition of Result params is a failure if any of them is a failure",
        MaxTest = 1000)]
    public Property TransposingOfResultsIsAFailureIfAnyOfThemIsAFailure() =>
        Prop.ForAll(
            GetIntegerResultGenerator()
                .Two()
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
            ArbMap.Default.GeneratorFor<int>()
                .Select(Result.Ok),
            ArbMap.Default.GeneratorFor<string>()
                .Select(static em => Result.Fail<int>(em)));
}