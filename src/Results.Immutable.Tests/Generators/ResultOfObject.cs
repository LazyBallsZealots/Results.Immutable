using FsCheck;

namespace Results.Immutable.Tests.Generators;

internal static class ResultOfObject
{
    public static Gen<Result<object?>> Generator() =>
        Arb.Generate<object?>()
            .SelectMany(
                static obj =>
                    Gen.OneOf(
                        Gen.Fresh(() => Result.Ok(obj)),
                        Gen.Fresh(() => Result.Fail<object?>("Errored out"))));

    public static Gen<(Result<object?>, Result<object?>)> TwoTupleGenerator(object? first, object? second)
    {
        const string firstErrorMessage = "First errored out";
        const string secondErrorMessage = "Second errored out";

        return Gen.OneOf(
            Gen.Fresh(() => (First: Result.Ok(first), Second: Result.Ok(second))),
            Gen.Fresh(() => (First: Result.Fail<object?>(firstErrorMessage), Second: Result.Ok(second))),
            Gen.Fresh(() => (First: Result.Ok(first), Second: Result.Fail<object?>(secondErrorMessage))),
            Gen.Fresh(
                static () => (First: Result.Fail<object?>("First errored out"),
                    Second: Result.Fail<object?>("Second errored out"))));
    }
}