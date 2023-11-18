namespace ApiNetCore.Business.AlertsManagement
{
    public interface IAlertManager
    {
        bool HasAlerts();
        List<Alert> GetAlerts();
        void Handle(Alert alert);
    }
}