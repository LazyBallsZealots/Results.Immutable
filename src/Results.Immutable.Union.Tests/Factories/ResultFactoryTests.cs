using System.Collections.Immutable;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace Results.Immutable.Union.Tests.Factories;

public sealed class ResultFactoryTests
{
    [Property(
        MaxTest = 5000,
        DisplayName = "Value types are handled correctly")]
    public Property ValueTypesAreHandledCorrectly() =>
        // FsCheck cannot generate arbitrary value types. This is the best we can do :/
        Prop.ForAll<int?>(static value =>
        {
            var result = Result.Ok(value);
            var unwrap = result switch
            {
                null => value is null,
                int unwrapped => unwrapped == value,
                ImmutableList<Error> => false,
            };

            return (unwrap && result.IsOk && result.HasSucceeded && !result.HasFailed && !result.IsErrored)
                .ToProperty()
                .Classify(value == null, "null")
                .Classify(value != null, "not null");
        });

    [Property(
        MaxTest = 5000,
        DisplayName = "Reference types are handled correctly")]
    public Property ReferenceTypesAreHandledCorrectly() =>
        // Arbitrary<object?> does not generate nulls, for some spaghetti-monster forsaken reason…?
        Prop.ForAll(
            Gen.OneOf(
                    Gen.Constant<object?>(null),
                    ArbMap.Default.GeneratorFor<object?>())
                .ToArbitrary(),
            static referenceValue =>
            {
                var result = Result.Ok(referenceValue);
                var unwrap = result switch
                {
                    null => referenceValue is null,
                    ImmutableList<Error> => false,
                    _ => result.Equals(referenceValue),
                };

                return (result is { IsErrored: false, HasFailed: false, HasSucceeded: true, IsOk: true, } && unwrap)
                    .ToProperty()
                    .Classify(referenceValue is null, "null")
                    .Classify(referenceValue is not null, "not null");
            });

    [Property(
        DisplayName = "Failures contain passed errors",
        MaxTest = 5000)]
    public Property FailuresContainPassedErrors() =>
        Prop.ForAll(
            ArbMap.Default.GeneratorFor<string>()
                .Select(static s => new Error(s))
                .NonEmptyListOf()
                .ToArbitrary(),
            static errors => (Result.Fail<int>(errors) switch
                {
                    int => false,
                    IReadOnlyCollection<Error> unwrappedErrors => unwrappedErrors.SequenceEqual(errors),
                }).ToProperty()
                .Trivial(errors.Count == 0));
}