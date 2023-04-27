using Results.Immutable.Metadata;

namespace Results.Immutable;

public static class ErrorExtensions
{
    /// <summary>
    /// Adds a nested error to the current error.
    /// </summary>
    /// <param name="error">The current error</param>
    /// <param name="newInnerError">The new error to add as nested.</param>
    /// <returns>A new error with the new nested error.</returns>
    public static Error AddInnerError(this Error error, Error newInnerError)
    {
        return new Error(
            error.Message,
            error.InnerErrors.Add(newInnerError));
    }

    /// <summary>
    /// Adds nested errors to the current error.
    /// </summary>
    /// <param name="error">The current error.</param>
    /// <param name="newInnerErrors">The new errors to add as nested.</param>
    /// <returns></returns>
    public static Error AddInnerErrors(this Error error, IEnumerable<string> newInnerErrors)
    {
        return new Error(
            error.Message,
            error.InnerErrors.AddRange(newInnerErrors.Select(newError => new Error(newError))));
    }
}
