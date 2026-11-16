using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Results.Immutable.Union;

[Union]
public readonly struct Result<T> : IEquatable<Result<T>>
{
    private readonly T? successValue;

    private readonly ImmutableList<Error>? errorsValue;

    private readonly Tag tag;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Result(T? value)
    {
        successValue = value;
        errorsValue = null;
        tag = Tag.Value;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Result(ImmutableList<Error> errors)
    {
        successValue = default;
        errorsValue = errors;
        tag = Tag.Errors;
    }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a failed operation.
    ///     A failed operation does not contain a value, and may contain errors.
    /// </summary>
    public bool IsErrored
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => tag is Tag.Errors;
    }

    /// <inheritdoc cref="IsErrored" />
    public bool HasFailed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsErrored;
    }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a successful operation.
    ///     An OK operation contains a value, no errors are present.
    /// </summary>
    public bool IsOk
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => tag is Tag.Value;
    }

    /// <inheritdoc cref="IsOk" />
    public bool HasSucceeded
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsOk;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public object? Value =>
        tag switch
        {
            Tag.Value => successValue,
            Tag.Errors => errorsValue,
            _ => null,
        };

    /// <remarks>
    ///     <see cref="HasValue" /> has to return <see langword="false" />
    ///     for successful <see cref="Result{T}" />s containing <see langword="null" /> values.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool HasValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => tag is Tag.Errors || (tag is Tag.Value && successValue is not null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(out T? value)
    {
        value = successValue;
        return IsOk;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue([NotNullWhen(true)] out ImmutableList<Error>? errors)
    {
        errors = errorsValue;
        return IsErrored;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetOk(out T? value) => TryGetValue(out value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetErrors([NotNullWhen(true)] out ImmutableList<Error>? errors) => TryGetValue(out errors);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Result<T> other && Equals(other);

    /// <inheritdoc />
    public bool Equals(Result<T> other)
    {
        if (tag != other.tag)
        {
            return false;
        }

        return tag switch
        {
            Tag.Value =>
                EqualityComparer<T>.Default.Equals(successValue, other.successValue),
            Tag.Errors =>
                ReferenceEquals(errorsValue, other.errorsValue) ||
                (errorsValue is not null &&
                    other.errorsValue is not null &&
                    errorsValue.SequenceEqual(other.errorsValue)),
            _ => true,
        };
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(tag);

        if (IsOk)
        {
            hashCode.Add(successValue);
        }
        else if (errorsValue is not null)
        {
            foreach (var error in errorsValue)
            {
                hashCode.Add(error);
            }
        }

        return hashCode.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() =>
        tag switch
        {
            Tag.Value => $"Ok({successValue})",
            Tag.Errors => $"Fail({errorsValue?.Count ?? 0} errors)",
            _ => "Unknown",
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Result<T> left, Result<T> right) => !left.Equals(right);

    private enum Tag : byte
    {
        Value = 1,

        Errors = 2,
    }
}

public static class Result
{
    /// <summary>
    ///     Returns successful <see cref="Result{T}" />, containing provided <paramref name="value" />.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <returns>
    ///     A successful <see cref="Result{T}" />.
    /// </returns>
    public static Result<T> Ok<T>(T value) => new(value);

    /// <summary>
    ///     Returns a failed <see cref="Result{T}" />, containing provided <paramref name="errors" />.
    /// </summary>
    /// <param name="errors"><see cref="Error" />s to be passed to the returned <see cref="Result{T}" />.</param>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <returns>
    ///     A failed <see cref="Result{T}" />
    /// </returns>
    public static Result<T> Fail<T>(IEnumerable<Error> errors) => new(errors.ToImmutableList());
}