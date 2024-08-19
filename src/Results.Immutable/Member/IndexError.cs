using System.Text.Json.Serialization;

namespace Results.Immutable.Member;

/// <summary>
///     A piece of information that describes an expected failure in an operation related to a member of a type.
///     Generally used for direct members of a type, and yet it is possible to have nested members
///     via the <see cref="Error.InnerErrors" />.
/// </summary>
public record IndexError : Error<IndexError>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="IndexError" /> class.
    /// </summary>
    /// <param name="index">Index of the member.</param>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="innerErrors">A list of errors that caused this error, related to item.</param>
    [JsonConstructor]
    public IndexError(
        int index,
        string message,
        ImmutableList<Error> innerErrors)
        : base(message, innerErrors)
    {
        Index = index;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="IndexError" /> class.
    /// </summary>
    /// <param name="index">The path to the member.</param>
    /// <param name="message">A message that describes the error.</param>
    public IndexError(int index, string message)
        : this(
            index,
            message,
            ImmutableList<Error>.Empty)
    {
    }

    /// <summary>
    ///     Gets the index of the member where this error occurred.
    /// </summary>
    public int Index { get; init; }
}