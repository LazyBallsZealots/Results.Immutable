namespace Results.Immutable;

using Results.Immutable.Metadata;


public static class Successful
{
    public static Successful<T> Create<T>(T value)
    {
        return new Successful<T>(value, ImmutableList<Success>.Empty);
    }

    public static Successful<T> Create<T>(T value, Success success)
    {
        return new Successful<T>(value, ImmutableList<Success>.Empty.Add(success));
    }

    public static Successful<T> Create<T>(T value, ImmutableList<Success> successes)
    {
        return new Successful<T>(value, successes);
    }

    public static Successful<T> Create<T>(T value, string message)
    {
        return new Successful<T>(value, ImmutableList.Create(new Success(message)));
    }

    public static Option<Successful<T>> CreateOption<T>(T value)
    {
        return Option.Some(Create(value));
    }

    public static Result<Successful<T>> CreateResult<T>(T value)
    {
        return Result.Ok(Create(value));
    }

    public static Result<Successful<T>> CreateResult<T>(T value, Success success)
    {
        return Result.Ok(Create(value, success));
    }

    public static Result<Successful<T>> CreateResult<T>(T value, ImmutableList<Success> successes)
    {
        return Result.Ok(Create(value, successes));
    }
}

public readonly struct Successful<T> : IEquatable<Successful<T>>
{
    public T Value { get; }

    public ImmutableList<Success> Successes { get; }

    internal Successful(T value, ImmutableList<Success> successes)
    {
        Value = value;
        Successes = successes ?? ImmutableList<Success>.Empty;
    }

    public void Deconstruct(out T value, out ImmutableList<Success> successes)
    {
        value = Value;
        successes = Successes;
    }

    public bool Equals(Successful<T> other) => Equals(other as object);

    public override bool Equals(object? obj) => obj is Successful<T> other &&
        (Value is null ? other.Value is null : Value.Equals(other.Value)) &&
        other.Successes.SequenceEqual(Successes);

    public override int GetHashCode() => HashCode.Combine(Value, Successes);
}
