using Results.Immutable.Metadata;
using static Results.Immutable.Collection.Collection;

namespace Results.Immutable;

public readonly partial struct Result<T>
{
    public Result<TNew> SelectMany<TNew>(
        Func<T, Result<TNew>> selector)
    {
        if (!(some is var (v)))
        {
            return new Result<TNew>(errors, successes);
        }
        var result = selector(v);
        if (!(result.some is var (v1)))
        {
            return new Result<TNew>(errors, successes);
        }

        return result.some is { }
            ? new Result<TNew>(result.some, ConcatLists(successes, result.successes))
            : new Result<TNew>(ConcatLists(errors, result.errors), ConcatLists(successes, result.successes));
    }

    public Result<TNew> SelectMany<T1, TNew>(
        Func<T, Result<T1>> firstSelector,
        Func<T, T1, Result<TNew>> resultSelector)
    {
        if (!(some is var (v)))
        {
            return new Result<TNew>(errors, successes);
        }
        var firstResult = firstSelector(v);
        if (!(firstResult.some is var (v1)))
        {
            return new Result<TNew>(errors, successes);
        }
        var result = resultSelector(v, v1);

        return result.some is { }
            ? new Result<TNew>(result.some, ConcatLists(successes, firstResult.successes, result.successes))
            : new Result<TNew>(result.errors,
                ConcatLists(
                    successes,
                    firstResult.successes,
                    result.successes));
    }

    public Result<TNew> SelectMany<T1, T2, TNew>(
        Func<T, Result<T1>> firstSelector,
        Func<T, T1, Result<T2>> secondSelector,
        Func<T, T1, T2, Result<TNew>> resultSelector)
    {
        if (!(some is var (v)))
        {
            return new Result<TNew>(errors, successes);
        }
        var firstResult = firstSelector(v);
        if (!(firstResult.some is var (v1)))
        {
            return new Result<TNew>(errors, successes);
        }
        var secondResult = secondSelector(v, v1);
        if (!(secondResult.some is var (v2)))
        {
            return new Result<TNew>(errors, successes);
        }
        var result = resultSelector(v, v1, v2);

        return result.some is { }
            ? new Result<TNew>(result.some, ConcatLists(successes, firstResult.successes, secondResult.successes, result.successes))
            : new Result<TNew>(result.errors,
                ConcatLists(
                    successes,
                        firstResult.successes,
                        secondResult.successes,
                        result.successes));

    }

    public Result<TNew> SelectMany<T1, T2, T3, TNew>(
        Func<T, Result<T1>> firstSelector,
        Func<T, T1, Result<T2>> secondSelector,
        Func<T, T1, T2, Result<T3>> thirdSelector,
        Func<T, T1, T2, T3, Result<TNew>> resultSelector)
    {
        if (!(some is var (v)))
        {
            return new Result<TNew>(errors, successes);
        }
        var firstResult = firstSelector(v);
        if (!(firstResult.some is var (v1)))
        {
            return new Result<TNew>(errors, successes);
        }
        var secondResult = secondSelector(v, v1);
        if (!(secondResult.some is var (v2)))
        {
            return new Result<TNew>(errors, successes);
        }
        var thirdResult = thirdSelector(v, v1, v2);
        if (!(thirdResult.some is var (v3)))
        {
            return new Result<TNew>(errors, successes);
        }
        var result = resultSelector(v, v1, v2, v3);

        return result.some is { }
            ? new Result<TNew>(result.some, ConcatLists(successes, firstResult.successes, secondResult.successes, thirdResult.successes, result.successes))
            : new Result<TNew>(result.errors,
                ConcatLists(
                    successes,
                    firstResult.successes,
                    secondResult.successes,
                    thirdResult.successes,
                    result.successes));
    }

}