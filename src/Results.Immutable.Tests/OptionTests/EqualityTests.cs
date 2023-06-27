using FsCheck;
using FsCheck.Xunit;

namespace Results.Immutable.Tests.OptionTests;

public sealed class EqualityTests
{
    [Fact(DisplayName = "Equals to itself")]
    public void EqualsToItself()
    {
        var option = Option.Some(1);
        option.Should()
            .Be(option);
    }

    [Property(DisplayName = "Equals to another object with the same value")]
    public void EqualsToAnotherObjectWithTheSameValue() =>
        Prop.ForAll<object?>(
            static value =>
            {
                Option.Some(value)
                    .Should()
                    .Be(Option.Some(value));
            });

    [Property(DisplayName = "Not equals to another object with a different value")]
    public void NotEqualsToAnotherObjectWithADifferentValue() =>
        Prop.ForAll(
            Arb.Generate<(object?, object?)>()
                .Where(static value => value.Item1 != value.Item2)
                .ToArbitrary(),
            static value =>
            {
                Option.Some(value.Item1)
                    .Should()
                    .NotBe(Option.Some(value.Item2));
            });

    [Fact(DisplayName = "Doesn't equal to none")]
    public void DoesNotEqualToNone()
    {
        Option.Some(1)
            .Should()
            .NotBe(Option.None<int>());

        Option.None<int>()
            .Should()
            .NotBe(Option.Some(1));
    }

    [Fact(DisplayName = "None equals another None")]
    public void NoneEqualsAnotherNone()
    {
        Option.None<int>()
            .Should()
            .Be(Option.None<int>());
    }
}