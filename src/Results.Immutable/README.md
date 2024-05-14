# Results.Immutable

## Parsing and validating members of object

This library provides an API to quickly validate/parse user input such as queries and requests and convert them into a well-typed
structure as in Type-Driven Development (TDD) or Domain-Driven Development (DDD). Also, this technique allows to
reuse fetched data used for validation for further use without having to re-fetch it.

### Usage

Example, using repository patterns to fetch data.

```cs
// User input
record AddCityCommand(string CityName, string StateName);
// Validated and typed input to be used further in the application
record AddCityCommandParsed (City City, State State);

// ...
private async Task<Result<AddCityCommandParsed>> ParseInput(AddCityCommand addCityCommand)
{
    var city = await addCityCommand.ParseMemberAsync(
        c => c.CityName,
        async cityName =>
        {
            var city = await CityRepository.GetCity(cityName);
            return Result.OkIf(city is not null, city, new NotFound());

        },
        "City name is required"
    );

    var state = await addCityCommand.ParseMemberAsync(
        c => c.StateName,
        async stateName =>
        {
            var state = await StateRepository.GetState(stateName);
            return Result.OkIf(state is not null, state, new NotFound());

        },
        "State name is required"
    );

    return city.ZipWith(state, (city, state) => new AddCityCommandParsed(city, state));
}
// ...
```

In case that there is no `State` for the given `StateName`, the `Result` will return an `Error` with a `NotFound` error, like this:

```json
[
  {
    "Member": "State",
    "Message": "State name is required",
    "InnerErrors": [{ "Message": "not found" }]
  }
]
```

The casing of the member name in the error can be controlled using the `propertyNamingPolicy` of the `ParseMember` and `ParseMemberAsync` methods. Additionally, globally via the static property `MemberParser.DefaultNamingPolicy`. Both require a `JsonNamingPolicy` from the `System.Text.Json` namespace. This is simply reusing the included naming policies included in .NET, however, you can use any library for serializing in any format.

To rename the name reported in the `Member` property, a `[JsonPropertyName]` can be used in the member, this also doesn't hinder the usage with other serializing libraries.

Needless to say, this functionality only works with direct members. If the expression doesn't contain a member access, it will return the stringified version of the expression in the `Member` property. If nested accessing is used in the expression, only the direct member is reported in the `Member` property. For nesting, it is recommended to use the `ParseMember` in nested way.
