namespace ApiNetCore.Application.CustomExceptions;

public class ShowAlertsException : Exception
{
    private ShowAlertsException()
    {
    }

    public static void Throw()
    {
        throw new ShowAlertsException();
    }
}