namespace Results.Immutable.Member;

/// <summary>
///     A piece of information that describes an expected failure in an operation related to a member of a type.
///     Generally used for direct members of a type, and yet it is possible to have nested members
///     via the <paramref name="InnerErrors" /> parameter.
/// </summary>
/// <param name="Member">The name of the member.</param>
/// <param name="Message">A message that describes the error.</param>
/// <param name="InnerErrors">A list of errors that caused this error, related to member.</param>
public record MemberError(
    string Member,
    string Message,
    ImmutableList<Error> InnerErrors) : Error<MemberError>(Message, InnerErrors)
{
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
}