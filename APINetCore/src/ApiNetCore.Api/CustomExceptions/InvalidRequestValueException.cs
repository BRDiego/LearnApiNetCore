namespace ApiNetCore.Api.CustomExceptions;

public class InvalidRequestValueException : Exception
{
    public InvalidRequestValueException(string message) : base (message)
    {
    }
}