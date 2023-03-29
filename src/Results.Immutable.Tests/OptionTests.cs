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

    [Fact(DisplayName = "Has a value wrapped in Some")]
    public void SomeHasAValue()
    {
        var option = Option.Some("hello");
        option.Some?.Value.Should().Be("hello");
    }
    
    [Fact(DisplayName = "Has a valid null as a value wrapped in Some")]
    public void SomeHasANullValue()
    {
        var option = Option.Some<DateTime?>(null);
        option.Some.Should().NotBeNull();
        option.Some?.Value.Should().BeNull();
    }
    
    [Fact(DisplayName = "Has no value (Some is null)")]
    public void SomeHasNoValue()
    {
        var option = Option.None<int>();
        option.Some.Should().BeNull();
    }
    
    [Fact(DisplayName = "Has no value (struct)")]
    public void HasNoNullableStructValue()
    {
        var option = Option.None<int>();
        option.GetValue().Should().BeNull();
    }
    
    [Fact(DisplayName = "Has value (struct)")]
    public void HasNullableStructValue()
    {
        var option = Option.Some(1);
        option.GetValue().Should().Be(1);
    }
    
    [Fact(DisplayName = "Has no value (ref)")]
    public void HasNoNullableRefValue()
    {
        var option = Option.None<Dummy>();
        option.Get().Should().BeNull();
    }
    
    [Fact(DisplayName = "Has value (ref)")]
    public void HasNullableRefValue()
    {
        var dummy = new Dummy();
        var option = Option.Some(dummy);
        option.Get().Should().Be(dummy);
    }
    
    [Fact(DisplayName = "Gets default value for structs if no value")]
    public void GetsDefaultValueForStructsIfNoValue()
    {
        var option = Option.None<int>();
        option.ValueOrDefault.Should().Be(0);
    }
    
    [Fact(DisplayName = "Gets the struct value if it is set")]
    public void GetsTheStructValueIfItIsSet()
    {
        var option = Option.Some(2);
        option.ValueOrDefault.Should().Be(2);
    }
    
    [Fact(DisplayName = "Gets default value for refs if no value")]
    public void GetsDefaultValueForRefsIfNoValue()
    {
        var option = Option.None<Dummy>();
        option.ValueOrDefault.Should().BeNull();
    }
    
    [Fact(DisplayName = "Gets the ref value if it is set")]
    public void GetsTheRefValueIfItIsSet()
    {
        var dummy = new Dummy();
        var option = Option.Some(dummy);
        option.ValueOrDefault.Should().Be(dummy);
    }
    
    [Fact(DisplayName = "Gets the value if set and not \"or\" part")]
    public void GetsTheValueIfSetAndNotOrPart()
    {
        var option = Option.Some(4);
        option.GetOr(5).Should().Be(4);
    }
    
    [Fact(DisplayName = "Gets \"or\" part if value is not set")]
    public void GetsOrPartIfValueIsNotSet()
    {
        var option = Option.None<int>();
        option.GetOr(5).Should().Be(5);
    }
    
    [Fact(DisplayName = "Gets the value if set and not \"orElse\" part")]
    public void GetsTheValueIfSetAndNotOrElseFunction()
    {
        var option = Option.Some(4);
        option.GetOrElse(() => 7).Should().Be(4);
    }
    
    [Fact(DisplayName = "Gets \"orElse\" value if value is not set")]
    public void GetsOrElseFunctionIfValueIsNotSet()
    {
        var option = Option.None<int>();
        option.GetOrElse(() => 8).Should().Be(8);
    }
}