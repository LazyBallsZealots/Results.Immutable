﻿using FsCheck;
using FsCheck.Xunit;
using Results.Immutable.Tests.Extensions;
using static FluentAssertions.FluentActions;
using static Results.Immutable.Tests.Generators.IntegerResultGenerator;

namespace Results.Immutable.Tests;

public sealed class ResultProjectionTests
{
    private static Result<Unit> SuccessfulResult => Result.Ok();

    [Fact(DisplayName = "No-op bind on a successful result of unit should return a result of unit")]
    public void NoOpBindOnSuccessfulResultShouldReturnAUnit() =>
        SuccessfulResult.Select(static (_) => Unit.Value)
            .Should()
            .Be(SuccessfulResult);


    [Fact(DisplayName = "Bind on a failed result should return a new, equivalent result without executing the bind")]
    public void BindOnFailedResultShouldReturnANewEquivalentFailedResult()
    {
        const string errorMessage = "An error";
        var fail = Result.Fail(errorMessage);

        Invoking(() => fail.Select(ThrowException))
            .Should()
            .NotThrow()
            .Which
            .Should()
            .Match<Result<Unit>>(
                static r => r.IsErrored &&
                    r.Errors.Single()
                        .Message ==
                    errorMessage);

        static Unit ThrowException(Unit _) =>
            throw new InvalidOperationException("Synchronous bind on a failed result was executed");
    }

    [Property(DisplayName = "Bind on a successful result with value converts it")]
    public Property BindOnSuccessfulResultWithValueConvertsIt(int firstValue, int finalValue) =>
        (Result.Ok(firstValue)
                    .SelectMany(_ => Result.Ok(finalValue))
                is { IsOk: true, } r &&
            r.ValueMatches(i => i == finalValue))
        .ToProperty();


    [Fact(DisplayName = "No-op binding on multiple results should return result of unit")]
    public void NoOpBindingOnMultipleResultsShouldReturnResultOfUnit() =>
        SuccessfulResult.SelectMany(
                static _ => Result.Ok(),
                static (_, _) => Result.Ok())
            .Should()
            .Be(SuccessfulResult);

    [Property(
        DisplayName = "Binds are associative",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindsAreAssociative() =>
        Prop.ForAll(
            GetIntegerResultGenerator()
                .Three()
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third) = tuple;

                return first.SelectMany(_ => second).SelectMany(_ => third)
                    == first.SelectMany(_ => second.SelectMany(_ => third));
            });

    // [Property(
    //     DisplayName = "Binding on two successful results should return result with a proper value",
    //     MaxTest = 1000,
    //     StartSize = int.MaxValue / 2,
    //     EndSize = int.MaxValue)]
    // public Property BindingOnTwoSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
    //     int firstValue,
    //     int secondValue)
    // {
    //     var sum = firstValue + secondValue;
    //
    //     return (Result.Ok(firstValue)
    //                 .SelectMany(
    //                     _ => Result.Ok(secondValue),
    //                     static (first, second) => Result.Ok(first + second))
    //                      is { IsOk: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }
    //
    // [Property(
    //     DisplayName = "Binding on three successful results should return result with a proper value",
    //     MaxTest = 1000,
    //     StartSize = int.MaxValue / 2,
    //     EndSize = int.MaxValue)]
    // public Property BindingOnThreeSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
    //     int firstValue,
    //     int secondValue,
    //     int thirdValue)
    // {
    //     var sum = firstValue + secondValue + thirdValue;
    //
    //     return (Result.Ok(firstValue)
    //                 .SelectMany(
    //                     _ => Result.Ok(secondValue),
    //                     (_, _) => Result.Ok(thirdValue),
    //                     static (
    //                         first,
    //                         second,
    //                         third) => Result.Ok(first + second + third))
    //                          is { IsOk: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }
    //
    // [Property(
    //     DisplayName = "Binding on four successful results should return a result with a proper value",
    //     MaxTest = 1000,
    //     StartSize = int.MaxValue / 2,
    //     EndSize = int.MaxValue)]
    // public Property BindingOnFourSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
    //     int first,
    //     int second,
    //     int third,
    //     int fourth)
    // {
    //     var sum = first + second + third + fourth;
    //
    //     return (Result.Ok(first)
    //                 .SelectMany(
    //                     _ => Result.Ok(second),
    //                     (_, _) => Result.Ok(third),
    //                     (_, _, _) => Result.Ok(fourth),
    //                     static (a, b, c, d) => Result.Ok(a + b + c + d))
    //                          is { IsOk: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }

    //         return (Result.Ok(firstValue)
    //                     .SelectMany(
    //                         _ => Result.Ok(secondValue),
    //                         static (first, second) => first.SelectMany(
    //                                 _ => second,
    //                                 static (i1, i2) => Option.Some(i1 + i2))
    //                             .Match(
    //                                 static i => Result.Ok(i),
    //                                 MatchNone)) is { IsSuccessful: true, }
    // success &&
    //                 success.ValueMatches(i => i == sum))
    //             .ToProperty();
    //     }

    //     return (Result.Ok(first)
    //                 .SelectMany(
    //                     _ => Result.Ok(second),
    //                     _ => Result.Ok(third),
    //                     _ => Result.Ok(fourth),
    //                     _ => Result.Ok(fifth),
    //                     static (
    //                         a,
    //                         b,
    //                         c,
    //                         d,
    //                         e) => a.SelectMany(
    //                             _ => b,
    //                             _ => c,
    //                             _ => d,
    //                             _ => e,
    //                             static (
    //                                 a,
    //                                 b,
    //                                 c,
    //                                 d,
    //                                 e) => Option.Some(a + b + c + d + e))
    //                         .Match(
    //                             static i => Result.Ok(i),
    //                             MatchNone)) is { IsOk: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }

    //     return (Result.Ok(firstValue)
    //                 .SelectMany(
    //                     _ => Result.Ok(secondValue),
    //                     _ => Result.Ok(thirdValue),
    //                     static (
    //                         first,
    //                         second,
    //                         third) => first.SelectMany(
    //                             _ => second,
    //                             _ => third,
    //                             static (
    //                                 first,
    //                                 second,
    //                                 third) => Option.Some(first + second + third))
    //                         .Match(
    //                             static i => Result.Ok(i),
    //                             MatchNone)) is { IsSuccessful: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }

    //     return (Result.Ok(first)
    //                 .SelectMany(
    //                     _ => Result.Ok(second),
    //                     _ => Result.Ok(third),
    //                     _ => Result.Ok(fourth),
    //                     _ => Result.Ok(fifth),
    //                     _ => Result.Ok(sixth),
    //                     static (
    //                         a,
    //                         b,
    //                         c,
    //                         d,
    //                         e,
    //                         f) => a.SelectMany(
    //                             _ => b,
    //                             _ => c,
    //                             _ => d,
    //                             _ => e,
    //                             _ => f,
    //                             static (
    //                                 a,
    //                                 b,
    //                                 c,
    //                                 d,
    //                                 e,
    //                                 f) => Option.Some(a + b + c + d + e + f))
    //                         .Match(
    //                             static i => Result.Ok(i),
    //                             MatchNone)) is { IsOk: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }

    //     return (Result.Ok(first)
    //                 .SelectMany(
    //                     _ => Result.Ok(second),
    //                     _ => Result.Ok(third),
    //                     _ => Result.Ok(fourth),
    //                     static (
    //                         a,
    //                         b,
    //                         c,
    //                         d) => a.SelectMany(
    //                             _ => b,
    //                             _ => c,
    //                             _ => d,
    //                             static (
    //                                 a,
    //                                 b,
    //                                 c,
    //                                 d) => Option.Some(a + b + c + d))
    //                         .Match(
    //                             static i => Result.Ok(i),
    //                             MatchNone)) is { IsSuccessful: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }

    //                     return r1.IsErrored || r2.IsErrored;
    //                 })
    //             .ToArbitrary(),
    //         static tuple =>
    //         {
    //             var (first, second) = tuple;

    //     return (Result.Ok(first)
    //                 .SelectMany(
    //                     _ => Result.Ok(second),
    //                     _ => Result.Ok(third),
    //                     _ => Result.Ok(fourth),
    //                     _ => Result.Ok(fifth),
    //                     static (
    //                         a,
    //                         b,
    //                         c,
    //                         d,
    //                         e) => a.SelectMany(
    //                             _ => b,
    //                             _ => c,
    //                             _ => d,
    //                             _ => e,
    //                             static (
    //                                 a,
    //                                 b,
    //                                 c,
    //                                 d,
    //                                 e) => Option.Some(a + b + c + d + e))
    //                         .Match(
    //                             static i => Result.Ok(i),
    //                             MatchNone)) is { IsSuccessful: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }

    // [Property(
    //     DisplayName = "Binding on three results should return a failure if at least one of these results is a failure",
    //     MaxTest = 1000,
    //     StartSize = int.MaxValue / 2,
    //     EndSize = int.MaxValue)]
    // public Property BindingOnThreeResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
    //     Prop.ForAll(
    //         GetIntegerResultGenerator()
    //             .Three()
    //             .Where(
    //                 static t =>
    //                 {
    //                     var (r1, r2, r3) = t;

    //     return (Result.Ok(first)
    //                 .SelectMany(
    //                     _ => Result.Ok(second),
    //                     _ => Result.Ok(third),
    //                     _ => Result.Ok(fourth),
    //                     _ => Result.Ok(fifth),
    //                     _ => Result.Ok(sixth),
    //                     static (
    //                         a,
    //                         b,
    //                         c,
    //                         d,
    //                         e,
    //                         f) => a.SelectMany(
    //                             _ => b,
    //                             _ => c,
    //                             _ => d,
    //                             _ => e,
    //                             _ => f,
    //                             static (
    //                                 a,
    //                                 b,
    //                                 c,
    //                                 d,
    //                                 e,
    //                                 f) => Option.Some(a + b + c + d + e + f))
    //                         .Match(
    //                             static i => Result.Ok(i),
    //                             MatchNone)) is { IsSuccessful: true, } success &&
    //             success.ValueMatches(i => i == sum))
    //         .ToProperty();
    // }

    //             return first.SelectMany(
    //                     _ => second,
    //                     _ => third,
    //                     static (
    //                         _,
    //                         _,
    //                         _) => Result.Ok())
    //                 .IsErrored;
    //         });

    // [Property(
    //     DisplayName = "Binding on four results should return a failure if at least one of these results is a failure",
    //     MaxTest = 1000,
    //     StartSize = int.MaxValue / 2,
    //     EndSize = int.MaxValue)]
    // public Property BindingOnFourResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
    //     Prop.ForAll(
    //         GetIntegerResultGenerator()
    //             .Four()
    //             .Where(
    //                 static t =>
    //                 {
    //                     var (r1, r2, r3, r4) = t;

    //                     return new[]
    //                     {
    //                         r1,
    //                         r2,
    //                         r3,
    //                         r4,
    //                     }.Any(static r => r.IsErrored);
    //                 })
    //             .ToArbitrary(),
    //         static tuple =>
    //         {
    //             var (first, second, third, fourth) = tuple;

    //             return first.SelectMany(
    //                     _ => second,
    //                     _ => third,
    //                     _ => fourth,
    //                     static (
    //                         _,
    //                         _,
    //                         _,
    //                         _) => Result.Ok())
    //                 .IsErrored;
    //         });

    // [Property(
    //     DisplayName = "Binding on five results should return a failure if at least one of these results is a failure",
    //     MaxTest = 1000,
    //     StartSize = int.MaxValue / 2,
    //     EndSize = int.MaxValue)]
    // public Property BindingOnFiveResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
    //     Prop.ForAll(
    //         GetIntegerResultGenerator()
    //             .Four()
    //             .SelectMany(
    //                 static _ => GetIntegerResultGenerator(),
    //                 static (tuple, result) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, result))
    //             .Where(
    //                 static t =>
    //                 {
    //                     var (r1, r2, r3, r4, r5) = t;

    //                     return new[]
    //                     {
    //                         r1,
    //                         r2,
    //                         r3,
    //                         r4,
    //                         r5,
    //                     }.Any(static r => r.IsErrored);
    //                 })
    //             .ToArbitrary(),
    //         static tuple =>
    //         {
    //             var (first, second, third, fourth, fifth) = tuple;

    //             return first.SelectMany(
    //                     _ => second,
    //                     _ => third,
    //                     _ => fourth,
    //                     _ => fifth,
    //                     static (
    //                         _,
    //                         _,
    //                         _,
    //                         _,
    //                         _) => Result.Ok())
    //                 .IsErrored;
    //         });

    // [Property(
    //     DisplayName = "Binding on six results should return a failure if at least one of these results is a failure",
    //     MaxTest = 1000,
    //     StartSize = int.MaxValue / 2,
    //     EndSize = int.MaxValue)]
    // public Property BindingOnSixResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
    //     Prop.ForAll(
    //         GetIntegerResultGenerator()
    //             .Four()
    //             .SelectMany(
    //                 static _ => GetIntegerResultGenerator()
    //                     .Two(),
    //                 static (tuple1, tuple2) =>
    //                 {
    //                     var ((first, second, third, fourth), (fifth, sixth)) = (tuple1, tuple2);

    //                     return (first, second, third, fourth, fifth, sixth);
    //                 })
    //             .Where(
    //                 static t =>
    //                 {
    //                     var (r1, r2, r3, r4, r5, r6) = t;

    //                     return new[]
    //                     {
    //                         r1,
    //                         r2,
    //                         r3,
    //                         r4,
    //                         r5,
    //                         r6,
    //                     }.Any(static r => r.IsErrored);
    //                 })
    //             .ToArbitrary(),
    //         static tuple =>
    //         {
    //             var (first, second, third, fourth, fifth, sixth) = tuple;

    //             return first.SelectMany(
    //                     _ => second,
    //                     _ => third,
    //                     _ => fourth,
    //                     _ => fifth,
    //                     _ => sixth,
    //                     static (
    //                         _,
    //                         _,
    //                         _,
    //                         _,
    //                         _,
    //                         _) => Result.Ok())
    //                 .IsErrored;
    //         });

    private static Result<int> MatchNone() => Result.Fail<int>("One of the values is missing!");
}