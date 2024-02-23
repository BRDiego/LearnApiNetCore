namespace ApiNetCore.Business.AlertsManagement
{
    public interface IAlertManager
    {
        public bool HasAlerts { get; }

        void CheckAlerts();
        List<Alert> GetAlerts();
        void Handle(Alert alert);
    }
}