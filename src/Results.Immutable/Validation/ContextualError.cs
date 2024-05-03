namespace Results.Immutable.Validation;

public record ContextualError(
    string Context,
    string Message,
    ImmutableList<Error> InnerErrors) : Error(Message, InnerErrors);