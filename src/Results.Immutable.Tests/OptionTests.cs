using FsCheck;
using FsCheck.Xunit;

namespace Results.Immutable.Tests;

public sealed class OptionTests
{
    [Property(DisplayName = "Some has appropriate value", MaxTest = 1000)]
    public Property SomeHasAdequateValue() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(static i => (Option: Option.Some(i), ExpectedValue: i))
                .ToArbitrary(),
            static tuple =>
            {
                var (option, expectedValue) = tuple;

                return option is { Value: var actualValue, } &&
                    actualValue == expectedValue;
            });

    [Property(DisplayName = "Match of some returns proper value", MaxTest = 1000)]
    public Property MatchOfSomeReturnsProperValue() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(Option.Some)
                .ToArbitrary(),
            static option =>
                option.Match(
                        static _ => true,
                        static () => false)
                    .ToProperty());

    [Property(DisplayName = "Match of none returns proper value", MaxTest = 1000)]
    public Property MatchOfNoneReturnsProperValue() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(static _ => Option.None<int>())
                .ToArbitrary(),
            static option =>
                option.Match(
                        static _ => false,
                        static () => true)
                    .ToProperty());

    [Property(DisplayName = "Match of some executes proper action", MaxTest = 1000)]
    public Property MatchOfSomeExecutesProperAction() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(Option.Some)
                .ToArbitrary(),
            static option =>
            {
                var result = false;

                option.Match(
                    _ => result = true,
                    static () => { });

                return result;
            });

    [Property(DisplayName = "Match of none executes proper action", MaxTest = 1000)]
    public Property MatchOfNoneExecutesProperAction() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(static _ => Option.None<int>())
                .ToArbitrary(),
            static option =>
            {
                var result = false;

                option.Match(
                    static _ => { },
                    () => result = true);

                return result;
            });
}