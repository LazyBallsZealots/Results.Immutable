using FsCheck;
using FsCheck.Xunit;

namespace Results.Immutable.Tests;

public class OptionProjectionTests
{
    [Property(DisplayName = "Projecting two options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingTwoOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Two()
                .Where(static tuple => tuple.Item1.IsNone || tuple.Item2.IsNone)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;

                return first.SelectMany(_ => second, static (i, i1) => Option.Some(i + i1))
                    .IsNone;
            });

    [Property(DisplayName = "Projecting three options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingThreeOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Three()
                .Where(static tuple => tuple.Item1.IsNone || tuple.Item2.IsNone || tuple.Item3.IsNone)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        static (
                            i,
                            i1,
                            i2) => Option.Some(i + i1 + i2))
                    .IsNone;
            });

    [Property(DisplayName = "Projecting four options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingFourOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Four()
                .Where(
                    static tuple => tuple.Item1.IsNone ||
                        tuple.Item2.IsNone ||
                        tuple.Item3.IsNone ||
                        tuple.Item4.IsNone)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        static (
                            i,
                            i1,
                            i2,
                            i3) => Option.Some(i + i1 + i2 + i3))
                    .IsNone;
            });

    [Property(DisplayName = "Projecting five options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingFiveOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Four()
                .SelectMany(
                    static _ => GetOptionalIntegerGenerator(),
                    static (tuple, option) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, option))
                .Where(
                    static tuple => tuple.Item1.IsNone ||
                        tuple.Item2.IsNone ||
                        tuple.Item3.IsNone ||
                        tuple.Item4.IsNone ||
                        tuple.Item5.IsNone)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        _ => fifth,
                        static (
                            i,
                            _,
                            _,
                            _,
                            _) => Option.Some(i))
                    .IsNone;
            });

    [Property(DisplayName = "Projecting six options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingSixOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Four()
                .SelectMany(
                    static _ => GetOptionalIntegerGenerator()
                        .Two(),
                    static (tuple, tuple2) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple2.Item1,
                        tuple2.Item2))
                .Where(
                    static tuple => tuple.Item1.IsNone ||
                        tuple.Item2.IsNone ||
                        tuple.Item3.IsNone ||
                        tuple.Item4.IsNone ||
                        tuple.Item5.IsNone ||
                        tuple.Item6.IsNone)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth, sixth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        _ => fifth,
                        _ => sixth,
                        static (
                            i,
                            _,
                            _,
                            _,
                            _,
                            _) => Option.Some(i))
                    .IsNone;
            });

    private static Gen<Option<int>> GetOptionalIntegerGenerator() =>
        Arb.Generate<int>()
            .SelectMany(
                static i => Gen.OneOf(
                    Gen.Fresh(() => Option.Some(i)),
                    Gen.Fresh(static () => Option.None<int>())));
}