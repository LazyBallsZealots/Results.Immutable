using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Results.Immutable.Extensions.FluentAssertions.Assertions;

/// <summary>
///     Provides assertions for <see cref="Option{T}" />.
/// </summary>
/// <typeparam name="T">The type of the value of the <see cref="Option{T}" /> to assert.</typeparam>
public class OptionAssertions<T> : OptionAssertions<T, OptionAssertions<T>>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="OptionAssertions{T}" /> class.
    /// </summary>
    /// <param name="subject">The <see cref="Option{T}" /> to assert.</param>
    public OptionAssertions(Option<T> subject)
        : base(subject)
    {
    }
}

/// <summary>
///     Provides assertions for <see cref="Option{T}" />.
/// </summary>
/// <typeparam name="T">The type of the value of the <see cref="Option{T}" /> to assert.</typeparam>
public class OptionAssertions<TSubject, TAssertions> : ObjectAssertions<Option<TSubject>, TAssertions>
    where TAssertions : OptionAssertions<TSubject, TAssertions>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="OptionAssertions{T}" /> class.
    /// </summary>
    /// <param name="subject">The <see cref="Option{T}" /> to assert.</param>
    public OptionAssertions(Option<TSubject> subject)
        : base(subject)
    {
    }

    protected override string Identifier => nameof(Option<TSubject>);

    /// <summary>
    ///     Asserts that the <see cref="Option{T}" /> contains a value.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, TSubject> BeSome(string because = "", params object[] becauseArgs)
    {
        if (Subject.Some is var (some))
        {
            return new((TAssertions)this, some);
        }

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:option} to be some{reason}, but found none.");

        return new((TAssertions)this, default(TSubject)!);
    }

    /// <summary>
    ///     Asserts that the <see cref="Option{T}" /> does not contain a value.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeNone(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsNone)
            .FailWith("Expected {context:option} to be none{reason}, but found some.");

        return new((TAssertions)this);
    }
}