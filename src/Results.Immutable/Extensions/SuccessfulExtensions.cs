namespace Results.Immutable;

using Results.Immutable.Metadata;
using static Results.Immutable.Collection.Collection;


public static class SucccessfulExtensions
{
    public static Result<Successful<TNew>> Select<T, TNew>(this Result<Successful<T>> result, Func<T, TNew> func) =>
        result.Some is var ((x, s))
            ? Result.Ok(Successful.Create(func(x), s))
            : Result.Fail<Successful<TNew>>(result.Errors);

    public static Option<Successful<TNew>> Select<T, TNew>(this Option<Successful<T>> result, Func<T, TNew> func) =>
        result.Some is var ((x, s))
            ? Option.Some(Successful.Create(func(x), s))
            : Option.None<Successful<TNew>>();

    public static Option<Successful<TNew>> SelectMany<T, TNew>(this Option<Successful<T>> result, Func<T, Option<Successful<TNew>>> optionSelector)
    {
        if (!(result.Some is var ((x, s))))
        {
            return Option.None<Successful<TNew>>();
        }

        return optionSelector(x).Some is var ((y, ss))
            ? Option.Some(Successful.Create(y, ConcatLists(s, ss)))
            : Option.None<Successful<TNew>>();
    }

    public static Option<Successful<TNew>> SelectMany<T, T1, TNew>(this Option<Successful<T>> option, Func<T, Option<Successful<T1>>> optionSelector, Func<T, T1, TNew> selector)
    {
        if (!(option.Some is var ((value, successes))))
        {
            return Option.None<Successful<TNew>>();
        }

        var combinedResult = optionSelector(value);
        if (!(combinedResult.Some is var ((combinedValue, combinedSuccesses))))
        {
            return Option.None<Successful<TNew>>();
        }

        return Option.Some(Successful.Create(selector(value, combinedValue), ConcatLists(successes, combinedSuccesses)));
    }

    public static Result<Successful<TNew>> AndThen<T, TNew>(this Result<Successful<T>> result, Func<T, Result<Successful<TNew>>> func)
    {
        if (result.Some is var ((x, s)))
        {
            var newResult = func(x);
            if (newResult.Some is var ((y, ss)))
            {
                return Result.Ok(Successful.Create(y, ConcatLists(s, ss)));
            }
            return newResult;
        }
        return Result.Fail<Successful<TNew>>(result.Errors);
    }

    public static Result<Successful<TNew>> SelectMany<T, TNew>(this Result<Successful<T>> result, Func<T, Result<Successful<TNew>>> func) =>
        result.AndThen(func);

    public static Result<Successful<TNew>> SelectMany<T, T1, TNew>(this Result<Successful<T>> result, Func<T, Result<Successful<T1>>> combinator, Func<T, T1, Result<Successful<TNew>>> selector)
    {
        if (!(result.Some is var ((value, successes))))
        {
            return Result.Fail<Successful<TNew>>(result.Errors);
        }

        var combinedResult = combinator(value);
        if (!(combinedResult.Some is var ((combinedValue, combinedSuccesses))))
        {
            return Result.Fail<Successful<TNew>>(combinedResult.Errors);
        }

        var newResult = selector(value, combinedValue);
        if (newResult.Some is var ((newValue, newSuccesses)))
        {
            return Result.Ok(Successful.Create(newValue, ConcatLists(successes, combinedSuccesses, newSuccesses)));
        }

        return Result.Fail<Successful<TNew>>(newResult.Errors);
    }

    public static Result<Successful<T>> AddSuccess<T>(this Result<T> result, string success) =>
        result.Some is var (value)
            ? Result.Ok(Successful.Create(value, success))
            : Result.Fail<Successful<T>>(result.Errors);

    public static Result<Successful<T>> AddSuccess<T>(this Result<Successful<T>> result, string success) =>
        result.Some is var ((value, successes))
            ? Result.Ok(Successful.Create(value, successes.Add(new Success(success))))
            : Result.Fail<Successful<T>>(result.Errors);
}