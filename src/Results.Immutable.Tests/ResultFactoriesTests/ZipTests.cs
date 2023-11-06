using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using Results.Immutable.Tests.Generators;

namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class ZipTests
{
    [Property(DisplayName = "Zipping 2 results should return appropriate value")]
    public Property ZippingTwoResultsShouldReturnAppropriateValue() =>
        Prop.ForAll(
            ResultOfObject.Generator()
                .Two()
                .ToArbitrary(),
            static tuple =>
                tuple switch
                {
                    ({ IsOk: true, } firstResult, { IsOk: true, } secondResult) =>
                        Result.Zip(firstResult, secondResult) is { Some.Value: var objectsTuple, } &&
                        firstResult is { Some.Value: var firstObject, } &&
                        secondResult is { Some.Value: var secondObject, } &&
                        objectsTuple == (firstObject, secondObject),
                    var (firstResult, secondResult) => Result.Zip(firstResult, secondResult)
                        .HasFailed,
                });

    [Property(DisplayName = "Zipping 3 results should return appropriate value")]
    public Property ZippingThreeResultsShouldReturnAppropriateValue() =>
        Prop.ForAll(
            ResultOfObject.Generator()
                .Three()
                .ToArbitrary(),
            static tuple =>
                tuple switch
                {
                    ({ IsOk: true, } firstResult, { IsOk: true, } secondResult, { IsOk: true, } thirdResult) =>
                        Result.Zip(
                            firstResult,
                            secondResult,
                            thirdResult) is { Some.Value: var objectsTuple, } &&
                        firstResult is { Some.Value: var firstObject, } &&
                        secondResult is { Some.Value: var secondObject, } &&
                        thirdResult is { Some.Value: var thirdObject, } &&
                        objectsTuple == (firstObject, secondObject, thirdObject),
                    var (firstResult, secondResult, thirdResult) => Result.Zip(
                            firstResult,
                            secondResult,
                            thirdResult)
                        .HasFailed,
                });

    [Property(DisplayName = "Zipping 4 results should return appropriate value", Replay = "1518894291,297195677")]
    public Property ZippingFourResultsShouldReturnAppropriateValue() =>
        Prop.ForAll(
            ResultOfObject.Generator()
                .Four()
                .ToArbitrary(),
            static tuple =>
                tuple switch
                {
                    ({ IsOk: true, } firstResult, { IsOk: true, } secondResult, { IsOk: true, } thirdResult, {
                            IsOk: true,
                        } fourthResult) =>
                        Result.Zip(
                            firstResult,
                            secondResult,
                            thirdResult,
                            fourthResult) is { Some.Value: var objectsTuple, } &&
                        firstResult is { Some.Value: var firstObject, } &&
                        secondResult is { Some.Value: var secondObject, } &&
                        thirdResult is { Some.Value: var thirdObject, } &&
                        fourthResult is { Some.Value: var fourthObject, } &&
                        objectsTuple == (firstObject, secondObject, thirdObject, fourthObject),
                    var (firstResult, secondResult, thirdResult, fourthResult) => Result.Zip(
                            firstResult,
                            secondResult,
                            thirdResult,
                            fourthResult)
                        .HasFailed,
                });

    [Property(DisplayName = "Zipping 5 results should return appropriate value")]
    public Property ZippingFiveResultsShouldReturnAppropriateValue() =>
        Prop.ForAll(
            ResultOfObject.Generator()
                .Four()
                .SelectMany(
                    static _ => ResultOfObject.Generator(),
                    static (tuple, result) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, result))
                .ToArbitrary(),
            static tuple =>
                tuple switch
                {
                    ({ IsOk: true, } firstResult,
                        { IsOk: true, } secondResult,
                        { IsOk: true, } thirdResult,
                        { IsOk: true, } fourthResult,
                        { IsOk: true, } fifthResult) =>
                        Result.Zip(
                            firstResult,
                            secondResult,
                            thirdResult,
                            fourthResult,
                            fifthResult) is { Some.Value: var objectsTuple, } &&
                        firstResult is { Some.Value: var firstObject, } &&
                        secondResult is { Some.Value: var secondObject, } &&
                        thirdResult is { Some.Value: var thirdObject, } &&
                        fourthResult is { Some.Value: var fourthObject, } &&
                        fifthResult is { Some.Value: var fifthObject, } &&
                        objectsTuple == (firstObject, secondObject, thirdObject, fourthObject, fifthObject),
                    var (firstResult, secondResult, thirdResult, fourthResult, fifthResult) => Result
                        .Zip(
                            firstResult,
                            secondResult,
                            thirdResult,
                            fourthResult,
                            fifthResult)
                        .HasFailed,
                });
}