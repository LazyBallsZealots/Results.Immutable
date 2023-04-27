namespace Results.Immutable.Tests.ResultTests.ProjectionsTests;

public sealed class SelectTests
{
    [Fact(DisplayName = "Maps a value into another")]
    public void NoOpBindOnSuccessfulResultShouldReturnAUnit() =>
        Result.Ok()
            .Select(static (_) => 5)
            .Should()
            .Be(Result.Ok(5));

    [Fact(DisplayName = "Returns an errored result if the original is errored")]
    public void ReturnsAnErroredResultIfTheOriginalIsErrored()
    {
        var ran = false;
        var result = Result.Fail("Unexpected");
        var selected = result
            .Select((_) => ran = true);

        selected
            .Should()
            .Be(Result.Fail<bool>("Unexpected"));

        ran.Should()
            .BeFalse();
    }
}