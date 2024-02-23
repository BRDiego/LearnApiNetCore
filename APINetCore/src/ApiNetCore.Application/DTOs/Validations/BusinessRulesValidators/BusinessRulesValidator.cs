using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Business.AlertsManagement;

namespace ApiNetCore.Application.DTOs.Validations.BusinessRulesValidators
{
    public class BusinessRulesValidator : IBusinessRules
    {
        private readonly IAlertManager alertManager;
        public BusinessRulesValidator(IAlertManager alertManager)
        {
            this.alertManager = alertManager;            
        }
        private void Alert(string message)
        {
            alertManager.Handle(new Alert(message));
        }

        public void ValidateBandName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 20)
                Alert("Invalid name provided");
        }

        public void ValidateMusicianAge(int age)
        {
            if (age > 0 && (age < 18 || age > (DateTime.Now.Date.Year - 1920)))
                Alert("Invalid age provided");
        }

        public void ValidateMusicianSurname(string surname)
        {
            if (!string.IsNullOrEmpty(surname) && surname.Length < 51)
                Alert("Invalid surname provided");
        }

        public void ValidateMusicianNickname(string nickname)
        {
            if (!string.IsNullOrEmpty(nickname) && nickname.Length < 21)
                Alert("Invalid nickname provided");
        }
    }
}
