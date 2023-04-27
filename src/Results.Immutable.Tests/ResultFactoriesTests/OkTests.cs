namespace Results.Immutable.Tests.ResultFactoriesTests;

public sealed class OkTests
{
    [Fact(DisplayName = "Creates a successful result of unit")]
    public void ShouldCreateSuccessfulResult()
    {
        var r = Result.Ok();
        r.IsOk.Should().BeTrue();
        r.Some?.Value.Should().Be(Unit.Value);
    }

    [Fact(DisplayName = "Creates a successful result of provided type")]
    public void ShouldCreateSuccessfulResultOfProvidedType()
    {
        var r = Result.Ok(42);
        r.IsOk.Should().BeTrue();
        r.Some?.Value.Should().Be(42);
    }
}