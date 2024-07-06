using FsCheck;
using FsCheck.Fluent;

namespace Results.Immutable.Tests.Generators;

internal static class Errors
{
    public static Gen<TError> Generator<TError>()
        where TError : Error<TError>
    {
    }   
}
