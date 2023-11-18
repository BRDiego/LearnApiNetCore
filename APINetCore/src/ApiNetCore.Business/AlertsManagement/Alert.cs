namespace ApiNetCore.Business.AlertsManagement
{
    public class Alert
    {
        public string Message { get; }
        public Alert(string message)
        {
            Message = message;
        }
    }
}