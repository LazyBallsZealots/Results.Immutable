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

## Parsing collections

Parsing collections is done in a similar way, but with the `ParseEach` method and its async variants. Example of parsing a list:

```cs
int[] ages = [5, 20];
var result = ages.ParseEach(
    age => Result.OkIf(
        age >= 18,
        age,
        new Error("Too young")),
    "The user should be older than 18");
```

The resulting `Result` contains an `IndexError` in case of failure. The `IndexError` contains the index of the element in the collection and the `Message` and `InnerErrors` of the `Error` returned by the parser.

Additionally, there is an overload of `ParseEach` which allows to create a custom `Error` type instead of `IndexError`.

Parsing a dictionary with strings as keys can be done via the `ParseEachPair` method and its async variants. Example of parsing a dictionary:

```cs
Dictionary<string, int> userAges = new()
    {
        ["Simon"] = 7,
        ["Robert"] = 18,
    };
var result = dictionary.ParsePairs(
    kv => Result.OkIf(
        kv.Value >= 18,
        KeyValuePair.Create(kv.Key.ToUpperInvariant(), kv.Value),
        new Error("Too young")),
    "The user should be older than 18");
```

In case of errors, `MemberError`s will be returned with the `InnerErrors` of the `Error` returned by the parser, similar to `ParseMember`. The function for parsing can return a `Result` of `KeyValuePair` which its key can be of any type.

For validating dictionaries which its keys are not of type `string`, use the `ParseEach` method and its variants.

## Creating custom errors

The true richness of this library lies in the ability to inform accurately the error of some operation. For this, it is advice to create your own error types instead of using the plain `Error` type included in this library. Said `Error` should be seen as a base type for more specific errors or as an inner error. It is a good practice to always create concrete types for your domain or infrastructure.

It is advised to extend the type `Error<TError>` instead of `Error` to get a better developer experience thanks to its proper typing of method such as `WithRootCause`.

Example:

```cs
private record PiggyBankError(
    string Message,
    ImmutableList<Error> InnerErrors) : Error<PiggyBankError>(Message, InnerErrors);
```
