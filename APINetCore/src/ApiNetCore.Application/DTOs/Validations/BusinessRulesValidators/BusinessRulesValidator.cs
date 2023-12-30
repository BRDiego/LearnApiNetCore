using ApiNetCore.Application.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiNetCore.Application.DTOs.Validations.BusinessRulesValidators
{
    public class BusinessRulesValidator : IBusinessRules
    {
        public bool IsValidBandName(string name)
        {
            return string.IsNullOrEmpty(name) || name.Length > 20;
        }

        public bool IsValidMusicianAge(int age)
        {
            return !(age < 18 || age > (DateTime.Now.Date.Year - 1920));
        }

        public bool IsValidMusicianNickname(string nickname)
        {
            return !string.IsNullOrEmpty(nickname) && nickname.Length < 21;
        }

        public bool IsValidMusicianSurname(string surname)
        {
            return !string.IsNullOrEmpty(surname) && surname.Length < 51;
        }
    }
}
