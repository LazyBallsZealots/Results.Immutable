namespace Results.Immutable.Tests;

internal sealed class Fn<TOut>
{
    private readonly Func<TOut> fn;

    public Fn(Func<TOut> fn)
    {
        this.fn = fn;
    }

    public Func<TOut> Callable =>
        () =>
        {
            CallCount++;
            return fn();
        };

    public uint CallCount { get; private set; }

    public static implicit operator Func<TOut>(Fn<TOut> fn) => fn.Callable;

    public static implicit operator Fn<TOut>(Func<TOut> fn) => new(fn);
}

internal sealed class Fn<TIn, TOut>
{
    private readonly Func<TIn, TOut> fn;

    public Fn(Func<TIn, TOut> fn)
    {
        this.fn = fn;
    }

    public Func<TIn, TOut> Callable =>
        @in =>
        {
            CallCount++;
            return fn(@in);
        };

    public uint CallCount { get; private set; }
}

internal sealed class Fn<TIn, TIn2, TOut>
{
    private readonly Func<TIn, TIn2, TOut> fn;

    public Fn(Func<TIn, TIn2, TOut> fn)
    {
        this.fn = fn;
    }

    public Func<TIn, TIn2, TOut> Callable =>
        (@in, in2) =>
        {
            CallCount++;
            return fn(@in, in2);
        };

    public uint CallCount { get; private set; }
}