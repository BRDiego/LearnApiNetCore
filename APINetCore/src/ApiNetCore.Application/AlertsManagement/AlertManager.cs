
using ApiNetCore.Application.CustomExceptions;

namespace ApiNetCore.Business.AlertsManagement
{
    public class AlertManager : IAlertManager
    {
        List<Alert> alerts;

        public AlertManager()
        {
            alerts =  new ();
        }
        private void Handle(Alert alert)
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

        public void CheckAlerts()
        {
            if (HasAlerts)
            {
                ShowAlertsException.Throw();
            }
        }

        public void AddAlert(string message)
        {
            Handle(new Alert(message));
        }
    }
}