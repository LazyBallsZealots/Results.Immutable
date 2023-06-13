using FsCheck;

namespace Results.Immutable.Tests.Generators;

internal static class SelectMany
{
    public static Gen<(Result<object?> FirstResult,
            Result<object?> SecondResult,
            Func<object?, object?, object?> Selector,
            object? FinalValue)>
        Generator() =>
        Arb.Generate<object?>()
            .Two()
            .SelectMany(
                _ => Arb.Generate<Func<object?, object?, object?>>(),
                static (objects, func) => (objects.Item1, objects.Item2, func))
            .SelectMany(
                tuple => ResultOfObject.TwoTupleGenerator(tuple.Item1, tuple.Item2),
                static (funcTuple, resultsTuple) =>
                {
                    var ((first, second, selector), (result1, result2)) = (funcTuple, resultsTuple);

                    return (FirstResult: result1, SecondResult: result2, Selector: selector,
                        FinalValue: selector(first, second));
                });
}