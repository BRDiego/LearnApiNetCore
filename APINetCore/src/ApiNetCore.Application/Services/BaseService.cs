using ApiNetCore.Business.AlertsManagement;

namespace ApiNetCore.Business.Services
{
    public abstract class BaseService
    {
        protected readonly IAlertManager alertManager;

        public BaseService(IAlertManager alertManager)
        {
            this.alertManager = alertManager;
        }
    }
}