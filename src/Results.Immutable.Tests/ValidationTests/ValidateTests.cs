using Results.Immutable.Validation;

namespace Results.Immutable.ValidationTests;

public class ValidateTests
{
    [Fact(DisplayName = "Validate a value")]
    public void ValidateAValue()
    {
        var command = new Command("R", 13);
        var resultAge = command.Validate(c => c.Age, age => Result.FailIf(age < 18, "Too young"));
        var resultName = command.Validate(
            c => c.Name,
            name => name.Length > 3 ? Result.Ok(name) : Result.Fail<string>("Too short"));

        var validated = resultAge.ZipWith(resultName, (unit, name) => new ValidCommand(name, command.Age));
        validated.Errors.Should()
            .BeEquivalentTo(
                ImmutableList.Create(
                    new ContextualError(
                        "Age",
                        "Too young",
                        ImmutableList<Error>.Empty),
                    new ContextualError(
                        "Name",
                        "Too short",
                        ImmutableList<Error>.Empty)));
    }

    private record struct Command(string Name, int Age);

    private record struct ValidCommand(string Name, int Age);
}