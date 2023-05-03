using System.Collections.Immutable;

public class SucccessfulExtensionsTests
{
    public class Map
    {
        [Fact(DisplayName = "Maps a value in a result")]
        public void MapsAValueInAResult()
        {
            var r = Result.Ok(Successful.Create(1));
            r.Select(x => x + 1).Should().Be(Result.Ok(Successful.Create(2)));
        }

        [Fact(DisplayName = "Maps a value in an option")]
        public void MapsAValueInAnOption()
        {
            var r = Option.Some(Successful.Create(1));
            r.Select(x => x + 1).Should().Be(Option.Some(Successful.Create(2)));
        }
    }

    public class AndThen
    {
        [Fact(DisplayName = "Maps a value in a result")]
        public void MapsAValueInAResult()
        {
            var r = Result.Ok(Successful.Create(1));
            r.AndThen(x => Successful.CreateResult(x + 1)).Should().Be(Successful.CreateResult(2));
        }

        [Fact(DisplayName = "Concats the successes of the computations")]
        public void ConcatenatesTheSuccessesOfTheComputations()
        {
            var r = Successful.CreateResult(1, new Success("First Success"));
            r.AndThen(x => Successful.CreateResult(x + 1, new Success("Second Success")))
                .Should()
                .Be(Successful.CreateResult(2, ImmutableList.Create(new Success("First Success"), new Success("Second Success"))));
        }

        [Fact(DisplayName = "Returns errors of the first computation")]
        public void ReturnsErrorsOfTheFirstComputation()
        {
            var r = Result.Fail<Successful<int>>(new Error("First Error"));
            r.AndThen(x => Successful.CreateResult(x + 1)).Should().Be(Result.Fail<Successful<int>>(new Error("First Error")));
        }

        [Fact(DisplayName = "Returns errors of the second computation")]
        public void ReturnsErrorsOfTheSecondComputation()
        {
            var r = Successful.CreateResult(1);
            r.AndThen(x => Result.Fail<Successful<int>>(new Error("Second Error"))).Should().Be(Result.Fail<Successful<int>>(new Error("Second Error")));
        }
    }

    public class SelectManyOnOption
    {
        [Fact(DisplayName = "Selects many as an option using query syntax")]
        public void SelectsManyAsAnOptionUsingQuerySyntax()
        {
            var opt1 = Successful.CreateOption(2);
            var resultSyntax = from x in opt1
                               from y in Successful.CreateOption(x + 1)
                               select x + y;
            var resultMethods = opt1.SelectMany(x => Successful.CreateOption(x + 1), (x, y) => x + y);
            resultSyntax.Should().Be(Successful.CreateOption(5));
            resultMethods.Should().Be(Successful.CreateOption(5));
        }
    }

    public class AddSuccess
    {
        [Fact(DisplayName = "Adds a success to a result without successes")]
        public void AddsASuccessToAResultWithoutSuccesses()
        {
            var r = Result.Ok(1);
            r.AddSuccess("First Success").Should().Be(Successful.CreateResult(1, new Success("First Success")));
        }

        [Fact(DisplayName = "Adds a success to a result with successes")]
        public void AddsASuccessToAResultWithSuccesses()
        {
            var r = Successful.CreateResult(1, new Success("First Success"));
            r.AddSuccess("Second Success").Should().Be(Successful.CreateResult(1, ImmutableList.Create(new Success("First Success"), new Success("Second Success"))));
        }
    }
}