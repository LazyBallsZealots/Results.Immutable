using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Results.Immutable;

[Union]
public readonly struct Result<T> : IEquatable<Result<T>>, Result<T>.IUnionMembers
{
    private readonly T? successValue;

    private readonly ImmutableList<Error>? errorsValue;

    private readonly Tag tag;

    public Result()
    {
        successValue = default;
        errorsValue = null;
        tag = Tag.Unknown;
    }

    private Result(T? value)
    {
        successValue = value;
        errorsValue = null;
        tag = GetTagFrom(value);
    }

    private Result(ImmutableList<Error> errors)
    {
        successValue = default;
        errorsValue = errors;
        tag = Tag.Errors;
    }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="IResult{T}" />
    ///     represents a failed operation.
    ///     A failed operation does not contain a value, and may contain errors.
    /// </summary>
    public bool IsErrored => tag is Tag.Errors;

    /// <inheritdoc cref="IsErrored" />
    public bool HasFailed => IsErrored;

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="IResult{T}" />
    ///     represents a successful operation.
    ///     An OK operation contains a value, no errors are present.
    /// </summary>
    public bool IsOk => tag is Tag.Value or Tag.NullValueType;

    /// <inheritdoc cref="IsOk" />
    public bool HasSucceeded => IsOk;

    object? IUnionMembers.Value =>
        tag switch
        {
            Tag.Value or Tag.NullValueType => successValue,
            Tag.Errors => errorsValue,
            _ => null,
        };

    /// <remarks>
    ///     <see cref="IUnionMembers.HasValue" /> has to return <see langword="false" />
    ///     for successful <see cref="Result{T}" />s containing nullable <see cref="ValueType" />s.
    /// </remarks>
    bool IUnionMembers.HasValue => tag is Tag.Value or Tag.Errors;

    bool IUnionMembers.TryGetValue(out T? value)
    {
        value = successValue;
        return IsOk;
    }

    bool IUnionMembers.TryGetValue([NotNullWhen(true)] out ImmutableList<Error>? errors)
    {
        errors = errorsValue;
        return IsErrored;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is Result<T> other &&
        other.tag == tag &&
        tag switch
        {
            Tag.Value or Tag.NullValueType => (successValue, other.successValue) switch
            {
                (null, null) => true,
                ({ } first, { } second) => first.Equals(second),
                _ => false,
            },
            Tag.Errors => other.errorsValue?.SequenceEqual(other.errorsValue) is true,
            // default instances?
            _ => this == default && other == default,
        };

    /// <inheritdoc />
    public bool Equals(Result<T> other) => Equals(other as object);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(successValue);

        foreach (var error in errorsValue ?? [])
        {
            hashCode.Add(error);
        }

        return hashCode.ToHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

    public static bool operator !=(Result<T> left, Result<T> right) => !(left == right);

    private static Tag GetTagFrom(T? value) =>
        typeof(T).IsAssignableTo(typeof(ValueType)) && value is null ? Tag.NullValueType : Tag.Value;

    private enum Tag
    {
        Unknown,

        Value,

        Errors,

        NullValueType,
    }

    public interface IUnionMembers
    {
        object? Value { get; }

        bool HasValue { get; }

        static Result<T> Create(T? value) => new(value);

        static Result<T> Create(ImmutableList<Error> errors) => new(errors);

        bool TryGetValue(out T? value);

        bool TryGetValue([NotNullWhen(true)] out ImmutableList<Error>? errors);
    }
}