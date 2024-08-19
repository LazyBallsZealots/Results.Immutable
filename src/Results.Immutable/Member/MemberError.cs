using System.Text.Json.Serialization;

namespace Results.Immutable.Member;

/// <summary>
///     A piece of information that describes an expected failure in an operation related to a member of a type.
///     Generally used for direct members of a type, and yet it is possible to have nested members
///     via the <see cref="Error.InnerErrors" />.
/// </summary>
public record MemberError : Error<MemberError>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MemberError" /> class.
    /// </summary>
    /// <param name="member">The name of the member.</param>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="innerErrors">A list of errors that caused this error, related to member.</param>
    [JsonConstructor]
    public MemberError(
        string member,
        string message,
        ImmutableList<Error> innerErrors)
        : base(message, innerErrors)
    {
        Member = member;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MemberError" /> class.
    /// </summary>
    /// <param name="member">The path to the member.</param>
    /// <param name="message">A message that describes the error.</param>
    public MemberError(string member, string message)
        : this(
            member,
            message,
            ImmutableList<Error>.Empty)
    {
    }

    /// <summary>
    ///     Gets the name of the member.
    /// </summary>
    public string Member { get; init; }
}