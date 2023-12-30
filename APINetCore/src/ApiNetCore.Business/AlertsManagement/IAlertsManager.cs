namespace ApiNetCore.Business.AlertsManagement
{
    public interface IAlertManager
    {
        public bool HasAlerts { get; }
        List<Alert> GetAlerts();
        void Handle(Alert alert);
    }
}