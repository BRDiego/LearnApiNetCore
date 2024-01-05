namespace ApiNetCore.Application.CustomExceptions;

public class InvalidRequestValueException : Exception
{
    private InvalidRequestValueException()
    {
    }

    public static void AlertValidationException()
    {
        throw new InvalidRequestValueException();
    }
}