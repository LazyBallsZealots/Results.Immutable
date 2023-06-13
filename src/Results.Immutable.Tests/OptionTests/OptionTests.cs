using FsCheck;
using FsCheck.Xunit;

namespace Results.Immutable.Tests.OptionTests;

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

                return option is { ValueOrDefault: var actualValue, } &&
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

    [Fact(DisplayName = "Has an object wrapped in Some")]
    public void HasAnObjectWrappedInSome()
    {
        var option = Option.Some("hello");
        option.Some?.Value.Should()
            .Be("hello");
    }

    [Fact(DisplayName = "Has a null object wrapped in Some")]
    public void HasANullObjectWrappedInSome()
    {
        var option = Option.Some<string?>(null);
        option.Some?.Value.Should()
            .BeNull();
    }

    [Fact(DisplayName = "Has no object wrapped in Some")]
    public void HasNoObjectWrappedInSome()
    {
        var option = Option.None<string>();
        option.Some.Should()
            .BeNull();
    }

    [Fact(DisplayName = "Has a struct wrapped in Some")]
    public void HasAStructWrappedInSome()
    {
        var option = Option.Some(1);
        option.Some?.Value.Should()
            .Be(1);
    }

    [Fact(DisplayName = "Has a null struct wrapped in Some")]
    public void HasANullStructWrappedInSome()
    {
        var option = Option.Some<int?>(null);
        option.Some?.Value.Should()
            .BeNull();
    }

    [Fact(DisplayName = "Has no struct wrapped in Some")]
    public void HasNoStructWrappedInSome()
    {
        var option = Option.None<int>();
        option.Some.Should()
            .BeNull();
    }

    [Fact(DisplayName = "Gets object if it is there")]
    public void GetsObjectIfItIsThere()
    {
        var option = Option.Some("hello");
        option.ValueOrDefault.Should()
            .Be("hello");
    }

    [Fact(DisplayName = "Gets default object if there is no value")]
    public void GetsDefaultObjectIfThereIsNoValue()
    {
        var option = Option.None<string>();
        option.ValueOrDefault.Should()
            .BeNull();
    }

    [Fact(DisplayName = "Gets struct if it is there")]
    public void GetsStructIfItIsThere()
    {
        var option = Option.Some(1);
        option.ValueOrDefault.Should()
            .Be(1);
    }

    [Fact(DisplayName = "Gets default struct if there is no value")]
    public void GetsDefaultStructIfThereIsNoValue()
    {
        var option = Option.None<int>();
        option.ValueOrDefault.Should()
            .Be(0);
    }

    public class GetValueOrTests
    {
        [Fact(DisplayName = "Gets its value if available")]
        public void GetsTheValueIfSetAndNotOrPart()
        {
            var option = Option.Some(4);
            option.GetValueOr(5)
                .Should()
                .Be(4);
        }

        [Fact(DisplayName = "Gets the fallback if none")]
        public void GetsTheFallbackIfNone()
        {
            var option = Option.None<int>();
            option.GetValueOr(5)
                .Should()
                .Be(5);
        }

        [Fact(DisplayName = "Gets its null value if available")]
        public void GetsTheNullValueIfSetAndNotOrPart()
        {
            var option = Option.Some<string?>(null);
            option.GetValueOr("wrong")
                .Should()
                .BeNull();
        }
    }

    public class GetValueOrElseTests
    {
        [Fact(DisplayName = "Gets its value if available")]
        public void GetsTheValueIfSetAndNotOrPart()
        {
            var option = Option.Some(4);
            option.GetValueOrElse(() => 5)
                .Should()
                .Be(4);
        }

        [Fact(DisplayName = "Gets the fallback if none")]
        public void GetsTheFallbackIfNone()
        {
            var option = Option.None<int>();
            option.GetValueOrElse(() => 5)
                .Should()
                .Be(5);
        }

        [Fact(DisplayName = "Gets its null value if available")]
        public void GetsTheNullValueIfSetAndNotOrPart()
        {
            var option = Option.Some<string?>(null);
            option.GetValueOrElse(() => "wrong")
                .Should()
                .BeNull();
        }
    }
}