
namespace ApiNetCore.Business.AlertsManagement
{
    public class AlertManager : IAlertManager
    {
        List<Alert> alerts;

        public AlertManager()
        {
            alerts =  new ();
        }
        public void Handle(Alert alert)
        {
            alerts.Add(alert);
        }

        public bool HasAlerts
        {
            get
            {
                return alerts.Any();
            }
        }

        public List<Alert> GetAlerts()
        {
            return alerts;
        }
    }
}