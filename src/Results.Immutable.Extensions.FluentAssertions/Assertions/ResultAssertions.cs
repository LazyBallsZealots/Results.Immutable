using System.Collections.Immutable;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Results.Immutable.Extensions.FluentAssertions;

/// <summary>
///     Provides assertions for <see cref="Result{T}" />.
/// </summary>
/// <typeparam name="T">The type of the value of the <see cref="Result{T}" /> to assert.</typeparam>
public class ResultAssertions<T> : ResultAssertions<T, ResultAssertions<T>>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResultAssertions{T}" /> class.
    /// </summary>
    /// <param name="subject">The <see cref="Result{T}" /> to assert.</param>
    public ResultAssertions(Result<T> subject)
        : base(subject)
    {
    }
}

public class ResultAssertions<TSubject, TAssertions> : ObjectAssertions<Result<TSubject>, TAssertions>
    where TAssertions : ResultAssertions<TSubject, TAssertions>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResultAssertions{T}" /> class.
    /// </summary>
    /// <param name="subject">The <see cref="Result{T}" /> to assert.</param>
    public ResultAssertions(Result<TSubject> subject)
        : base(subject)
    {
    }

    protected override string Identifier => nameof(Result<TSubject>);

    /// <summary>
    ///     Asserts that the <see cref="Result{T}" /> contains a value.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, TSubject> BeOk(string because = "", params object[] becauseArgs)
    {
        if (Subject.Some is var (some))
        {
            return new((TAssertions)this, some);
        }

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:result} to be ok{reason}, but found error.");

        return new((TAssertions)this, default(TSubject)!);
    }

    /// <summary>
    ///     Asserts that the <see cref="Result{T}" /> does not contain a value.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, ImmutableList<Error>> BeFailed(
        string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsErrored)
            .FailWith("Expected {context:result} to be failed{reason}, but found ok.");

        return new((TAssertions)this, Subject.Errors);
    }

    /// <summary>
    ///     Asserts that the <see cref="Result{T}" /> contains errors of type <typeparamref name="TError" />.
    /// </summary>
    /// <typeparam name="TError">
    ///     The type of the error.
    /// </typeparam>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndWhichConstraint<TAssertions, IEnumerable<TError>> ContainErrorsOfType<TError>(
        string because = "",
        params object[] becauseArgs)
        where TError : Error
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.HasError<TError>())
            .FailWith(
                "Expected {context:result} to contain errors of type {0}{reason}, but found none.",
                typeof(TError));

        return new(
            (TAssertions)this,
            Subject.Errors.OfType<TError>());
    }
}