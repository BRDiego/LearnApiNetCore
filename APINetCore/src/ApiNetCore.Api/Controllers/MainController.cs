using ApiNetCore.Application.CustomExceptions;
using ApiNetCore.Business.AlertsManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiNetCore.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected readonly IAlertManager alertManager;

        protected MainController(IAlertManager alertManager)
        {
            this.alertManager = alertManager;
        }

        protected bool IsValidOperation()
        {
            return !alertManager.HasAlerts;
        }

        protected ActionResult CustomResponse(object? result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = alertManager.GetAlerts().Select(n => n.Message)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) AlertInvalidModel(modelState);
            return CustomResponse();
        }

        protected void AlertInvalidModel(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in erros)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                AlertValidation(errorMsg);
            }
        }

        protected void AlertValidation(string message)
        {
            alertManager.Handle(new Alert(message));
        }

        protected void AlertException(Exception exception)
        {
            if (exception is not ShowAlertsException)
            {
                //TODO - Log server errors
                //var message = exception.Message;
                //message += exception.InnerException is null ? "" : Environment.NewLine + exception.InnerException.Message;

                AlertValidation("Internal server error");
            }
        }

    }
}
