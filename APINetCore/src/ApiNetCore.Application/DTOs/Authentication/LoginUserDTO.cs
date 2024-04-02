using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiNetCore.Application.DTOs.Authentication
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is invalid")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(30, ErrorMessage = "{0} must contain {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; } = "";
    }
}
