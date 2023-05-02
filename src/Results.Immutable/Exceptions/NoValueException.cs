public class NoValueException : Exception
{
    public NoValueException(string message)
        : base(message)
    {
    }

    public NoValueException()
        : base()
    {
    }

}