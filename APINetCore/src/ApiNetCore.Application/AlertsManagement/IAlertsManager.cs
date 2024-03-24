namespace ApiNetCore.Business.AlertsManagement
{
    public interface IAlertManager
    {
        public bool HasAlerts { get; }

        void CheckAlerts();
        List<Alert> GetAlerts();
        void AddAlert(string message);
    }
}