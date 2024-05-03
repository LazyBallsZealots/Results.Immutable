using System.Linq.Expressions;

namespace Results.Immutable.Validation;

public static class ObjectExt
{
    public static Result<TParsed> Validate<TObject, TResult, TParsed>(
        this TObject obj,
        Expression<Func<TObject, TResult>> memberExpression,
        Func<TResult, Result<TParsed>> validator)
    {
        var context = memberExpression is { Body: var body, } && body is MemberExpression { Member.Name: var name, }
            ? name
            : "";

        var fn = memberExpression.Compile();
        var result = validator(fn(obj));

        if (result.IsOk)
        {
            return result;
        }

        var errors = result.Errors.Select(
            e => new ContextualError(
                context,
                e.Message,
                e.InnerErrors));

        return Result.Fail<TParsed>(errors);
    }
}