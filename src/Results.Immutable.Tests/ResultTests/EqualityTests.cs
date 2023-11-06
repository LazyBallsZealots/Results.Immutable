using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

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

    [Property(DisplayName = "Returns a boolean indicating (in)equality")]
    public Property ReturnsBooleansIndicatingEquality() =>
        Prop.ForAll(
            ArbMap.Default.GeneratorFor<Tuple<object?, object?>>()
                .ToArbitrary(),
            static tuple =>
            {
                var (value1, value2) = tuple;

                return value1?.Equals(value2) is true || (value1 is null && value2 is null)
                    ? Result.Ok(value1) == Result.Ok(value2)
                    : Result.Ok(value1) != Result.Ok(value2);
            });

    [Property(DisplayName = "Hashes failed results in the same way")]
    public Property ReturnsAValidHashCodeForFailedResults() =>
        Prop.ForAll<List<string>>(
            static errorMessages =>
            {
                var errors = errorMessages.Select(static s => new Error(s))
                    .ToList();

                var r1 = Result.Fail(errors);
                var r2 = Result.Fail(errors);

                return r1.GetHashCode() == r2.GetHashCode();
            });

    [Property(DisplayName = "Hashes successful results in the same way")]
    public Property ReturnsAValidHashCodeForSuccessfulResults() =>
        Prop.ForAll<object?>(
            static value =>
            {
                var r1 = Result.Ok(value);
                var r2 = Result.Ok(value);

                return r1.GetHashCode() == r2.GetHashCode();
            });
}