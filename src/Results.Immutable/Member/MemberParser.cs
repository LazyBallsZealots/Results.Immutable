using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Results.Immutable.Member;

/// <summary>
///     A set of extensions methods for all types for parsing or validating members of an object by selecting it and then
///     evaluating a function that returns a result. The returned error is wrapped in a <see cref="MemberError" /> with
///     information about the member that caused the error.
/// </summary>
public static class MemberParser
{
    /// <summary>
    ///     Gets or sets the default name policy to use when validating a member.
    ///     This is optional and used to modify the casing of the member name.
    ///     If not set, the original name is used as is.
    /// </summary>
    public static JsonNamingPolicy? DefaultNamingPolicy { get; set; }

    /// <summary>
    ///     Parses a member of an object and returns a result containing a <see cref="MemberError" />
    ///     with information of the path to the member and the error message.
    /// </summary>
    /// <param name="obj">The object containing the member to parse/validate.</param>
    /// <param name="memberSelector">The selector for the member.</param>
    /// <param name="parser">A function that validates and parses the member.</param>
    /// <param name="message">A message to add to the error in case of failure.</param>
    /// <param name="propertyNamingPolicy">
    ///     The name policy to use when validating the member, if not provided
    ///     <see cref="DefaultNamingPolicy" /> is used.
    /// </param>
    /// <typeparam name="TObject">The type of the object containing the member.</typeparam>
    /// <typeparam name="TMember">The type of the member.</typeparam>
    /// <typeparam name="TParsed">The type of the produced value after parsing the member.</typeparam>
    /// <returns></returns>
    public static Result<TParsed> ParseMember<TObject, TMember, TParsed>(
        this TObject obj,
        Expression<Func<TObject, TMember>> memberSelector,
        Func<TMember, Result<TParsed>> parser,
        string message = "",
        JsonNamingPolicy? propertyNamingPolicy = null) =>
        MapErrorsAsMemberErrors(
            memberSelector,
            parser(memberSelector.Compile()(obj)),
            message,
            propertyNamingPolicy);

    /// <inheritdoc
    ///     cref="ParseMember{TObject, TMember, TParsed}(TObject, Expression{Func{TObject, TMember}}, Func{TMember, Result{TParsed}}, string, JsonNamingPolicy?)" />
    public static async ValueTask<Result<TParsed>> ParseMemberAsync<TObject, TMember, TParsed>(
        this TObject obj,
        Expression<Func<TObject, TMember>> memberSelector,
        Func<TMember, ValueTask<Result<TParsed>>> parser,
        string message = "",
        JsonNamingPolicy? propertyNamingPolicy = null) =>
        MapErrorsAsMemberErrors(
            memberSelector,
            await parser(memberSelector.Compile()(obj)),
            message,
            propertyNamingPolicy);

    private static Result<TParsed> MapErrorsAsMemberErrors<TObject, TResult, TParsed>(
        Expression<Func<TObject, TResult>> memberExpression,
        Result<TParsed> result,
        string message,
        JsonNamingPolicy? propertyNamingPolicy)
    {
        if (result.IsOk)
        {
            return result;
        }

        return Result.Fail<TParsed>(
            new MemberError(
                SerializeExpression(memberExpression, propertyNamingPolicy),
                message,
                result.Errors));
    }

    private static string SerializeExpression(LambdaExpression expression, JsonNamingPolicy? propertyNamingPolicy)
    {
        if (expression.Body is not MemberExpression { Member: { Name: var name, CustomAttributes: var attributes, }, })
        {
            return expression.Body.ToString();
        }

        var firstPropName =
            attributes.FirstOrDefault(a => a.AttributeType.Equals(typeof(JsonPropertyNameAttribute)));

        if (firstPropName is { ConstructorArguments: { Count: 1, } arguments, } && arguments.First() is { } arg)
        {
            return (string)arg.Value!;
        }

        if ((propertyNamingPolicy ?? DefaultNamingPolicy) is { } policy)
        {
            return policy.ConvertName(name);
        }

        return name;
    }
}