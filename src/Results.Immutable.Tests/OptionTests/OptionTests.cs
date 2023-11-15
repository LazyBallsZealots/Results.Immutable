using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace Results.Immutable.Tests.OptionTests;

public sealed class OptionTests
{
    [Property(DisplayName = "Some has appropriate value", MaxTest = 1000)]
    public Property SomeHasAdequateValue() =>
        Prop.ForAll(
            ArbMap.Default.GeneratorFor<int>()
                .Select(static i => (Option: Option.Some(i), ExpectedValue: i))
                .ToArbitrary(),
            static tuple =>
            {
                var (option, expectedValue) = tuple;

                return option.Some is var (actualValue) &&
                    actualValue == expectedValue;
            });

    [Property(DisplayName = "Match of some returns proper value", MaxTest = 1000)]
    public Property MatchOfSomeReturnsProperValue() =>
        Prop.ForAll(
            ArbMap.Default.GeneratorFor<int>()
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
            ArbMap.Default.GeneratorFor<int>()
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
            ArbMap.Default.GeneratorFor<int>()
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
            ArbMap.Default.GeneratorFor<int>()
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

    [Fact(DisplayName = "Converts a nullable value to an Option")]
    public void ConvertsANullableValueToAnOption() =>
        Option.SomeIfNotNull<int>(1)
            .Should()
            .Be(Option.Some(1));

    [Fact(DisplayName = "Converts a null value to an Option")]
    public void ConvertsANullValueToAnOption() =>
        Option.SomeIfNotNull<int>(null)
            .Should()
            .Be(Option.None<int>());

    [Fact(DisplayName = "Converts a nullable reference to an Option")]
    public void ConvertsANullableReferenceToAnOption() =>
        Option.SomeIfNotNull("hello")
            .Should()
            .Be(Option.Some("hello"));

    [Fact(DisplayName = "Converts a null reference to an Option")]
    public void ConvertsANullReferenceToAnOption() =>
        Option.SomeIfNotNull<string>(null)
            .Should()
            .Be(Option.None<string>());

    [Fact(DisplayName = "Has an object wrapped in Some")]
    public void HasAnObjectWrappedInSome() =>
        Option.Some("hello")
            .Some?.Value.Should()
            .Be("hello");

    [Fact(DisplayName = "Has a null object wrapped in Some")]
    public void HasANullObjectWrappedInSome() =>
        Option.Some<string?>(null)
            .Some?.Value.Should()
            .BeNull();

    [Fact(DisplayName = "Has no object wrapped in Some")]
    public void HasNoObjectWrappedInSome() =>
        Option.None<string>()
            .Some.Should()
            .BeNull();

    [Fact(DisplayName = "Has a struct wrapped in Some")]
    public void HasAStructWrappedInSome() =>
        Option.Some(1)
            .Some?.Value.Should()
            .Be(1);

    [Fact(DisplayName = "Has a null struct wrapped in Some")]
    public void HasANullStructWrappedInSome() =>
        Option.Some<int?>(null)
            .Some?.Value.Should()
            .BeNull();

    [Fact(DisplayName = "Has no struct wrapped in Some")]
    public void HasNoStructWrappedInSome() =>
        Option.None<int>()
            .Some.Should()
            .BeNull();

    public class GetValueOrTests
    {
        [Fact(DisplayName = "Gets its value if available")]
        public void GetsTheValueIfSetAndNotOrPart() =>
            Option.Some(4)
                .GetValueOr(5)
                .Should()
                .Be(4);

        [Fact(DisplayName = "Gets the fallback if none")]
        public void GetsTheFallbackIfNone() =>
            Option.None<int>()
                .GetValueOr(5)
                .Should()
                .Be(5);

        [Fact(DisplayName = "Gets its null value if available")]
        public void GetsTheNullValueIfSetAndNotOrPart() =>
            Option.Some<string?>(null)
                .GetValueOr("wrong")
                .Should()
                .BeNull();
    }

    public class GetValueOrElseTests
    {
        [Fact(DisplayName = "Gets its value if available")]
        public void GetsTheValueIfSetAndNotOrPart() =>
            Option.Some(4)
                .GetValueOrElse(() => 5)
                .Should()
                .Be(4);

        [Fact(DisplayName = "Gets the fallback if none")]
        public void GetsTheFallbackIfNone() =>
            Option.None<int>()
                .GetValueOrElse(() => 5)
                .Should()
                .Be(5);

        [Fact(DisplayName = "Gets its null value if available")]
        public void GetsTheNullValueIfSetAndNotOrPart() =>
            Option.Some<string?>(null)
                .GetValueOrElse(() => "wrong")
                .Should()
                .BeNull();
    }
}