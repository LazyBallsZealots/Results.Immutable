namespace Results.Immutable.Tests;

public class SomeTests
{
    [Fact(DisplayName = "Should wrap a value")]
    public void ShouldWrapAValue()
    {
        new Some<int>(1).Value.Should().Be(1);
    }

    [Fact(DisplayName = "Should wrap a nullable value")]
    public void ShouldWrapANullableValue()
    {
        new Some<int?>(1).Value.Should().Be(1);
        new Some<int?>(null).Value.Should().Be(null);
    }

    [Fact(DisplayName = "Should wrap an object value")]
    public void ShouldWrapAnObjectValue()
    {
        new Some<string?>("hello").Value.Should().Be("hello");
        new Some<string?>(null).Value.Should().Be(null);
    }
}