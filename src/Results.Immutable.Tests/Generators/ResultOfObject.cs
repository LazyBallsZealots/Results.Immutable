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
                        Gen.Constant(Result.Fail<object?>("Errored out"))));

    public static Gen<(Result<object?>, Result<object?>)> TwoTupleGenerator(object? first, object? second)
    {
        const string firstErrorMessage = "First errored out";
        const string secondErrorMessage = "Second errored out";

        return Gen.OneOf(
            Gen.Constant((First: Result.Ok(first), Second: Result.Ok(second))),
            Gen.Constant((First: Result.Fail<object?>(firstErrorMessage), Second: Result.Ok(second))),
            Gen.Constant((First: Result.Ok(first), Second: Result.Fail<object?>(secondErrorMessage))),
            Gen.Constant(
                (First: Result.Fail<object?>("First errored out"),
                    Second: Result.Fail<object?>("Second errored out"))));
    }
}