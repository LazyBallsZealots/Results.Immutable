using FsCheck;
using FsCheck.Fluent;

namespace Results.Immutable.Tests.Generators;

internal static class ResultOf<T>
{
    public static Gen<Result<T>> Generator() =>
        Gen.OneOf(
            ArbMap.Default.GeneratorFor<ImmutableList<string>>()
                .Select(errors => Result.Fail<T>(errors.Select(e => new Error(e)))),
            ArbMap.Default.GeneratorFor<T>()
                .Select(Result.Ok));
}