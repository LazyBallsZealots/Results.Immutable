namespace Results.Immutable;

/// <summary>
///     Wraps any value in a <see cref="Some{T}"/>.
/// </summary>
public record struct Some<T>(T Value);