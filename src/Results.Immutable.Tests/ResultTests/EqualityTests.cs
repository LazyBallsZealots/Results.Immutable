namespace Results.Immutable.Tests.ResultTests;

public sealed class EqualityTests
{
    [Fact(DisplayName = "Should be equal to itself")]
    public void ShouldBeEqualToItself()
    {
        var result = Result.Ok();

        result.Should()
            .Be(result);
    }

    [Fact(DisplayName = "Should be equal to a result with the same value")]
    public void ShouldBeEqualToAResultWithTheSameValue()
    {
        var intResult = Result.Ok(42);
        intResult.Should()
            .Be(Result.Ok(42));
    }

    [Fact(DisplayName = "Should not be equal to a result with a different value")]
    public void ShouldNotBeEqualToAResultWithADifferentValue()
    {
        var result = Result.Ok(15);
        result.Should()
            .NotBe(Result.Ok(42));
    }

    [Fact(DisplayName = "Should not be equal to a result with different errors")]
    public void ShouldNotBeEqualToAResultWithDifferentErrors()
    {
        var result = Result.Fail<int>("life is sad");
        result.Should()
            .NotBe(Result.Fail<int>("death is sad"));
    }

    [Fact(DisplayName = "Should be equal (typed")]
    public void ShouldBeEqualTyped()
    {
        var result = Result.Ok(42);
        result.Equals(Result.Ok(42))
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should not be equal (typed")]
    public void ShouldNotBeEqualTyped()
    {
        var result = Result.Ok(42);
        result.Equals(Result.Ok(15))
            .Should()
            .BeFalse();
    }

    [Fact(DisplayName = "Returns a boolean indicating equality (==)")]
    public void ReturnsABooleanIndicatingEquality()
    {
        (Result.Ok(42) == Result.Ok(42)).Should()
            .BeTrue();

        (Result.Ok(26) != Result.Ok(42)).Should()
            .BeTrue();
    }
    
    [Fact(DisplayName = "Returns a boolean indicating difference (!=)")]
    public void ReturnsABooleanIndicatingDifference()
    {
        (Result.Ok(29) != Result.Ok(29)).Should()
            .BeFalse();

        (Result.Ok(26) != Result.Ok(42)).Should()
            .BeTrue();
    }
}