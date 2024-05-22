using FsCheck;
using FsCheck.Fluent;

namespace Results.Immutable.Tests.Generators;

internal static class ResultOfObject
{
    public static Gen<Result<object?>> Generator() =>
        ArbMap.Default.GeneratorFor<object?>()
            .SelectMany(
                static obj =>
                    Gen.OneOf(
                        Gen.Constant(Result.Ok(obj)),
                        Gen.Constant(Result.Fail<object?>(new Error("Errored out")))));

    public static Gen<(Result<object?>, Result<object?>)> TwoTupleGenerator(object? first, object? second)
    {
        var firstError = new Error("First errored out");
        var secondError = new Error("Second errored out");

        return Gen.OneOf(
            Gen.Constant((First: Result.Ok(first), Second: Result.Ok(second))),
            Gen.Constant((First: Result.Fail<object?>(firstError), Second: Result.Ok(second))),
            Gen.Constant((First: Result.Ok(first), Second: Result.Fail<object?>(secondError))),
            Gen.Constant(
                (First: Result.Fail<object?>(firstError),
                    Second: Result.Fail<object?>(secondError))));
    }
}