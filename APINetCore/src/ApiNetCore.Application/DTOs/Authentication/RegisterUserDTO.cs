
using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Application.DTOs.Authentication
{
    public class RegisterUserDTO : LoginUserDTO
    {
        [Compare(nameof(Password), ErrorMessage = "the passwords does not match")]
        public string ConfirmPassword { get; set; } = "";
    }
}
