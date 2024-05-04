using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Business.AlertsManagement;
using AutoMapper;

namespace ApiNetCore.Application.Services
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