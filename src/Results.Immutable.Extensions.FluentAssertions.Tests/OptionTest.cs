namespace Results.Immutable.Extensions.FluentAssertions.Tests;

public class OptionTest
{
    [Fact(DisplayName = "Be some with some")]
    public void BeSomeWithSome() =>
        Option.Some(5)
            .Should()
            .BeSome()
            .Which.Should()
            .Be(5);

    [Fact(DisplayName = "Be some with none")]
    public void BeSomeWithNone()
    {
        var act = () => Option.None<int>()
            .Should()
            .BeSome("yes");

        act.Should()
            .Throw<Exception>()
            .WithMessage("Expected option to be some because yes, but found none.");
    }

    [Fact(DisplayName = "Be none with some")]
    public void BeNoneWithSome()
    {
        var act = () => Option.Some(5)
            .Should()
            .BeNone("yes");

        act.Should()
            .Throw<Exception>()
            .WithMessage("Expected option to be none because yes, but found some.");
    }

    [Fact(DisplayName = "Be none with none")]
    public void BeNoneWithNone() =>
        Option.None<int>()
            .Should()
            .BeNone();
}