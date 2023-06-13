namespace Results.Immutable.Tests;

internal sealed class Fn<TIn, TOut>
{
    private readonly Func<TIn, TOut> fn;

    public uint CallCount { get; private set; }

    public Fn(Func<TIn, TOut> fn)
    {
        this.fn = fn;
    }

    public Func<TIn, TOut> Callable => @in =>
    {
        CallCount++;
        return fn(@in);
    };
}

internal sealed class Fn<TIn, TIn2, TOut>
{
    private readonly Func<TIn, TIn2, TOut> fn;

    public uint CallCount { get; private set; }

    public Fn(Func<TIn, TIn2, TOut> fn)
    {
        this.fn = fn;
    }

    public Func<TIn, TIn2, TOut> Callable => (@in, in2) =>
    {
        CallCount++;
        return fn(@in, in2);
    };
}