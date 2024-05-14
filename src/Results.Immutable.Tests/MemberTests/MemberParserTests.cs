using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Results.Immutable.Member;
using static Results.Immutable.Result;

namespace Results.Immutable.Tests.MemberTests;

public sealed class MemberParserTests
{
    public MemberParserTests()
    {
        MemberParser.DefaultNamingPolicy = null;
    }

    [Fact(DisplayName = "Validates values without errors")]
    public void ValidateAValueWithoutErrors()
    {
        var result = new Command("R", 20).ParseMember(
            c => c.Age,
            age => OkIf(
                age > 18,
                age,
                new Error("Too young")));

        result.Should()
            .ContainValue();
    }

    [Fact(DisplayName = "Validates values that produce errors")]
    public void ValidateAValueThatProducesErrors()
    {
        var command = new Command("R", 13);

        var errorTooYoung = new Error("Too young");
        var resultAge = command.ParseMember(
            c => c.Age,
            age => OkIf(
                age > 18,
                age,
                errorTooYoung),
            "The user should be an adult");

        Expression<Func<Command, string>> nameSelector = nameSelector = c => c.Name;
        var errorTooShort = new Error("Too short");
        var resultName = command.ParseMember(
            nameSelector = c => c.Name,
            name => OkIf(
                name.Length > 3,
                name,
                errorTooShort));

        var valid = resultAge.ZipWith(resultName, (age, name) => new ValidCommand(name, age));
        valid.Errors.Should()
            .BeEquivalentTo(
                ImmutableList.Create(
                    new MemberError(
                        nameof(Command.Age),
                        "The user should be an adult",
                        ImmutableList.Create(errorTooYoung)),
                    new MemberError(
                        nameof(Command.Name),
                        "",
                        ImmutableList.Create(errorTooShort))));
    }

    [Fact(DisplayName = "Validates asynchronously values without errors")]
    public async Task ValidateAsynchronouslyValuesWithoutErrors()
    {
        var result = await new Command("R", 20).ParseMemberAsync(
            c => c.Age,
            age => ValueTask.FromResult(
                OkIf(
                    age > 18,
                    age,
                    new Error("Too young"))));

        result.Should()
            .ContainValue();
    }

    [Fact(DisplayName = "Validates asynchronously values that produce errors")]
    public async Task ValidateAsynchronouslyValuesThatProducesErrors()
    {
        var command = new Command("R", 13);

        var resultAge = await command.ParseMemberAsync(
            c => c.Age,
            age => ValueTask.FromResult(
                OkIf(
                    age > 18,
                    age,
                    new Error("Too young"))),
            "The user should be an adult");

        var resultName = await command.ParseMemberAsync(
            c => c.Name,
            name => ValueTask.FromResult(
                OkIf(
                    name.Length > 3,
                    name,
                    new Error("Too short"))));

        var valid = resultAge.ZipWith(resultName, (age, name) => new ValidCommand(name, age));
        valid.Errors.Should()
            .BeEquivalentTo(
                ImmutableList.Create(
                    new MemberError(
                        nameof(Command.Age),
                        "The user should be an adult",
                        ImmutableList.Create(new Error("Too young"))),
                    new MemberError(
                        nameof(Command.Name),
                        "",
                        ImmutableList.Create(new Error("Too short")))));
    }

    [Fact(DisplayName = "Uses the JsonPropertyNameAttribute for a member name")]
    public void UsesTheJsonPropertyNameAttributeForAMemberName()
    {
        var result = new Named("R", 7).ParseMember(
            n => n.Name,
            name => OkIf(
                name.Length > 3,
                name,
                new Error("Too short")));

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .Match<MemberError>(x => x.Member == "custom");
    }

    [Fact(DisplayName = "Uses the Naming Policy for a member name")]
    public void UsesTheNamingPolicyForAMemberName()
    {
        MemberParser.DefaultNamingPolicy = new MyPolicy();

        var result = new Named("R", 2).ParseMember(
            n => n.CoVariant,
            roads => OkIf(
                roads > 3,
                roads,
                new Error("Not enough")),
            propertyNamingPolicy: JsonNamingPolicy.CamelCase);

        result.Errors.Should()
            .SatisfyRespectively(
                static single =>
                    single.Should()
                        .Match<MemberError>(static x => x.Member == "coVariant"));
    }

    [Fact(DisplayName = "Uses the default Naming Policy for a member name")]
    public void UsesTheDefaultNamingPolicyForAMemberName()
    {
        MemberParser.DefaultNamingPolicy = new MyPolicy();

        var result = new Named("R", 2).ParseMember(
            n => n.CoVariant,
            roads => OkIf(
                roads > 3,
                roads,
                new Error("Not enough")));

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .Match<MemberError>(x => x.Member == "covariant");
    }

    [Fact(DisplayName = "Prints the expression if it is not a member")]
    public void PrintsTheExpressionIfItIsNotAMember()
    {
        var result = new Named("R", 2).ParseMember(
            n => "I am using this wrong",
            roads => Fail(new Error("Weird case")));

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new MemberError(
                    "\"I am using this wrong\"",
                    "",
                    ImmutableList.Create(new Error("Weird case"))));
    }

    private record struct Command(string Name, int Age);

    private record struct Named(
        [property: JsonPropertyName("custom")]
        string Name,
        int CoVariant);

    private record struct ValidCommand(string Name, int Age);

    private class MyPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLowerInvariant();
    }
}