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
            alertManager.AddAlert(message);
        }

        public void ValidateBandName(ref string? name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 20)
                Alert("Invalid name provided");

            name = CheckString(name);
        }

        public void ValidateMusicianAge(ref int? age)
        {
            if (age is not null && age > 0 && (age < 18 || age > (DateTime.Now.Date.Year - 1920)))
                Alert("Invalid age provided");

            age ??= 0;
        }

        public void ValidateMusicianSurname(ref string? surname)
        {
            if (string.IsNullOrEmpty(surname) || surname.Length > 51)
                Alert("Invalid surname provided");

            surname = CheckString(surname);
        }

        public void ValidateMusicianNickname(ref string? nickname)
        {
            if (string.IsNullOrEmpty(nickname) && nickname.Length > 21)
                Alert("Invalid nickname provided");

            nickname = CheckString(nickname);
        }

        private string CheckString(string? value)
        {
            value ??= "";
            value = value.Trim();

            return value;
        }
    }
}
